using Rincon.BackgroundServices;
using Rincon.Helpers;

namespace Rincon;

public partial class App : Application
{
	/// <summary>
	/// Main app
	/// </summary>
	public App()
	{
		InitializeComponent();
		MainPage = new AppShell();
		
		// Inicializar el servicio de backup
		var backupService = ServiceHelper.GetService<DatabaseBackupService>();
	}
}
