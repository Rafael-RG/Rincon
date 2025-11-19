using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Rincon.DataAccess;

namespace Rincon.BackgroundServices
{
    public class DatabaseBackupService : IDisposable
    {
        private readonly ILogger<DatabaseBackupService> _logger;
        private Timer _timer;
        private bool _disposed = false;

        public DatabaseBackupService(ILogger<DatabaseBackupService> logger)
        {
            _logger = logger;
            StartService();
        }

        private void StartService()
        {
            _logger.LogInformation("DatabaseBackupService iniciado con Timer");
            
            try
            {
                // Ejecutar backup inmediatamente para pruebas
                _logger.LogInformation("Ejecutando backup inmediato al iniciar servicio...");
                Task.Run(async () => 
                {
                    try 
                    {
                        await PerformBackup();
                        _logger.LogInformation("Backup inmediato completado exitosamente");
                        
                        // Configurar timer para próximos backups (cada 12 horas)
                        var nextBackupTime = CalculateNextBackupTime();
                        var delay = nextBackupTime - DateTime.Now;
                        
                        if (delay < TimeSpan.Zero)
                            delay = TimeSpan.FromMinutes(2); // Si ya pasó la hora, programar en 2 minutos
                        
                        _logger.LogInformation($"Próximo backup programado para: {nextBackupTime:yyyy-MM-dd HH:mm:ss} (en {delay.TotalHours:F1} horas)");
                        
                        // Timer que se ejecuta cada 12 horas (17:00 y 11:00)
                        _timer = new Timer(async _ => await PerformBackup(), null, delay, TimeSpan.FromHours(12));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error en backup inmediato: {Error}", ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar DatabaseBackupService: {Error}", ex.Message);
            }
        }

        private DateTime CalculateNextBackupTime()
        {
            var now = DateTime.Now;
            
            // Horarios de backup: 11:00 AM y 17:00 PM (5:00 PM)
            var backupTimes = new[]
            {
                new DateTime(now.Year, now.Month, now.Day, 11, 0, 0), // 11:00 AM
                new DateTime(now.Year, now.Month, now.Day, 17, 0, 0)  // 17:00 PM
            };
            
            // Buscar el próximo horario de backup
            foreach (var backupTime in backupTimes)
            {
                if (backupTime > now)
                {
                    return backupTime;
                }
            }
            
            // Si ya pasaron ambos horarios de hoy, programar para las 11:00 AM de mañana
            return new DateTime(now.Year, now.Month, now.Day, 11, 0, 0).AddDays(1);
        }

        private async Task PerformBackup()
        {
            try
            {
                _logger.LogInformation("Iniciando backup automático de la base de datos SQL Server");
                
                var backupFileName = $"RinconDB_backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql";
                var backupPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "RinconBackups");
                
                // Crear directorio si no existe
                Directory.CreateDirectory(backupPath);
                _logger.LogInformation($"Directorio de backup creado/verificado: {backupPath}");
                
                var fullBackupPath = Path.Combine(backupPath, backupFileName);
                
                // Para Azure SQL Database, hacer backup usando script SQL
                try
                {
                    using (var context = new DatabaseContext())
                    {
                        var connectionString = context.Database.GetConnectionString();
                        _logger.LogInformation($"Verificando tipo de base de datos. Connection string: {(connectionString != null ? connectionString.Substring(0, Math.Min(50, connectionString.Length)) : "null")}...");
                        
                        if (connectionString?.Contains("database.windows.net") == true)
                        {
                            _logger.LogInformation("Azure SQL Database detectada. Creando backup de script SQL...");
                            await CreateSqlScriptBackup(context, fullBackupPath);
                        }
                        else
                        {
                            _logger.LogInformation("SQL Server local detectado. Intentando BACKUP DATABASE...");
                            
                            var backupSql = $@"
                                BACKUP DATABASE [RinconDBTest] 
                                TO DISK = '{fullBackupPath.Replace("\\", "\\\\").Replace(".sql", ".bak")}'
                                WITH FORMAT, 
                                     COMPRESSION,
                                     CHECKSUM,
                                     STATS = 10";
                            
                            _logger.LogInformation($"Ejecutando backup de SQL Server a: {fullBackupPath.Replace(".sql", ".bak")}");
                            
                            await context.Database.ExecuteSqlRawAsync(backupSql);
                            
                            _logger.LogInformation($"Backup de SQL Server completado exitosamente");
                        }
                        
                        // Verificar tamaño del archivo
                        var finalPath = connectionString?.Contains("database.windows.net") == true ? fullBackupPath : fullBackupPath.Replace(".sql", ".bak");
                        if (File.Exists(finalPath))
                        {
                            var fileSize = new FileInfo(finalPath).Length / (1024 * 1024); // MB
                            _logger.LogInformation($"Tamaño del backup: {fileSize} MB - Archivo: {Path.GetFileName(finalPath)}");
                        }
                        else
                        {
                            _logger.LogWarning($"El archivo de backup no se encontró en: {finalPath}");
                        }
                    }
                }
                catch (Exception sqlEx)
                {
                    _logger.LogError(sqlEx, "Error específico en operación de backup SQL: {SqlError}", sqlEx.Message);
                    throw;
                }
                
                // Limpiar backups antiguos (mantener solo los últimos 7)
                await CleanOldBackups(backupPath);
                
                _logger.LogInformation("Backup automático completado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al realizar el backup automático: {ErrorMessage}", ex.Message);
            }
        }

        private async Task CreateSqlScriptBackup(DatabaseContext context, string filePath)
        {
            try
            {
                _logger.LogInformation("Iniciando backup de script SQL para Azure SQL Database...");
                
                var sqlScript = new System.Text.StringBuilder();
                
                // Encabezado del script
                sqlScript.AppendLine($"-- Backup de RinconDBTest generado en {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sqlScript.AppendLine("-- Este es un backup de datos para Azure SQL Database");
                sqlScript.AppendLine();
                
                // Obtener todas las tablas principales (las que tienen datos)
                var tableQueries = new Dictionary<string, string>
                {
                    { "User", "SELECT * FROM [User]" },
                    { "Product", "SELECT * FROM [Product]" },
                    { "ProductStock", "SELECT * FROM [ProductStock]" },
                    { "Movement", "SELECT * FROM [Movement]" },
                    { "Operator", "SELECT * FROM [Operator]" },
                    { "Note", "SELECT * FROM [Note]" },
                    { "Task", "SELECT * FROM [Task]" },
                    { "Booking", "SELECT * FROM [Booking]" },
                    { "Order", "SELECT * FROM [Order]" },
                    { "BookingOrder", "SELECT * FROM [BookingOrder]" }
                };
                
                _logger.LogInformation($"Procesando {tableQueries.Count} tablas para backup...");
                
                // Usar la misma conexión para todas las consultas
                using (var connection = context.Database.GetDbConnection())
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        await connection.OpenAsync();
                
                    foreach (var tableQuery in tableQueries)
                    {
                        try
                        {
                            sqlScript.AppendLine($"-- Datos de tabla {tableQuery.Key}");
                            sqlScript.AppendLine($"-- DELETE FROM [{tableQuery.Key}];");
                            
                            using (var command = connection.CreateCommand())
                            {
                                command.CommandText = tableQuery.Value;
                                using (var reader = await command.ExecuteReaderAsync())
                                {
                                    var recordCount = 0;
                                    var columnNames = new List<string>();
                                    
                                    // Obtener nombres de columnas
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        columnNames.Add(reader.GetName(i));
                                    }
                                    
                                    while (await reader.ReadAsync())
                                    {
                                        var values = new List<string>();
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            var value = reader.GetValue(i);
                                            if (value == null || value == DBNull.Value)
                                            {
                                                values.Add("NULL");
                                            }
                                            else if (value is string || value is DateTime || value is Guid)
                                            {
                                                values.Add($"'{value.ToString().Replace("'", "''")}'");
                                            }
                                            else if (value is bool)
                                            {
                                                values.Add(((bool)value) ? "1" : "0");
                                            }
                                            else
                                            {
                                                values.Add(value.ToString());
                                            }
                                        }
                                        
                                        sqlScript.AppendLine($"INSERT INTO [{tableQuery.Key}] ([{string.Join("], [", columnNames)}]) VALUES ({string.Join(", ", values)});");
                                        recordCount++;
                                    }
                                    
                                    _logger.LogInformation($"Tabla {tableQuery.Key}: {recordCount} registros procesados");
                                }
                            }
                            
                            sqlScript.AppendLine();
                        }
                        catch (Exception tableEx)
                        {
                            _logger.LogWarning(tableEx, $"Error procesando tabla {tableQuery.Key}: {tableEx.Message}");
                            sqlScript.AppendLine($"-- ERROR en tabla {tableQuery.Key}: {tableEx.Message}");
                            sqlScript.AppendLine();
                        }
                    }
                }
                
                // Escribir el script al archivo
                await File.WriteAllTextAsync(filePath, sqlScript.ToString(), System.Text.Encoding.UTF8);
                _logger.LogInformation($"Script SQL backup guardado en: {filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando backup de script SQL: {Error}", ex.Message);
                throw;
            }
        }

        private async Task CleanOldBackups(string backupPath)
        {
            try
            {
                // Buscar tanto archivos .bak como .sql
                var backupFiles = Directory.GetFiles(backupPath, "RinconDB_backup_*.*")
                    .Where(f => f.EndsWith(".bak") || f.EndsWith(".sql"))
                    .Select(f => new FileInfo(f))
                    .OrderByDescending(f => f.CreationTime)
                    .ToList();

                // Mantener solo los últimos 14 backups (7 días x 2 backups por día)
                var filesToDelete = backupFiles.Skip(14);
                
                foreach (var file in filesToDelete)
                {
                    file.Delete();
                    _logger.LogInformation($"Backup antiguo eliminado: {file.Name}");
                }
                
                _logger.LogInformation($"Limpieza completada. Backups mantenidos: {Math.Min(backupFiles.Count, 14)}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al limpiar backups antiguos");
            }
        }



        public void Dispose()
        {
            if (!_disposed)
            {
                _timer?.Dispose();
                _logger.LogInformation("DatabaseBackupService disposed");
                _disposed = true;
            }
        }
    }
}