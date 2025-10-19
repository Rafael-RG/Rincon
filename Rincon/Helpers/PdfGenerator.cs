using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using Rincon.Models;
using TextAlignment = iText.Layout.Properties.TextAlignment;
using Image = iText.Layout.Element.Image;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using Color = iText.Kernel.Colors.Color;
using Border = iText.Layout.Borders.Border;
using Cell = iText.Layout.Element.Cell;
using Path = System.IO.Path;
using iText.Kernel.Pdf.Canvas.Draw;

public class PdfGenerator
{
    public static void GenerateBookingOrderPdf(BookingOrder bookingOrder, List<Order> orders, string outputPath)
    {
        try
        {
            using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            using (PdfWriter writer = new PdfWriter(fs))
            using (PdfDocument pdf = new PdfDocument(writer))
            using (Document document = new Document(pdf, PageSize.A4))
            {
                document.SetMargins(30, 40, 30, 40);
                var colorMarron = new DeviceRgb(173, 107, 42);
                var colorFondoClaro = new DeviceRgb(255, 246, 237);

                // === HEADER SECTION ===
                // Logo y título en la misma línea
                Table headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 30, 70 })).UseAllAvailableWidth();
                
                // Celda del logo
                Cell logoCell = new Cell();
                try
                {
                    using var stream = FileSystem.Current.OpenAppPackageFileAsync("logopdf.png").Result;
                    using var memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);
                    var imageData = ImageDataFactory.Create(memoryStream.ToArray());
                    Image logo = new Image(imageData);
                    logo.ScaleToFit(80, 80);
                    logoCell.Add(logo);
                }
                catch (Exception)
                {
                    try
                    {
                        using var stream = FileSystem.Current.OpenAppPackageFileAsync("logo_login.png").Result;
                        using var memoryStream = new MemoryStream();
                        stream.CopyTo(memoryStream);
                        var imageData = ImageDataFactory.Create(memoryStream.ToArray());
                        Image logo = new Image(imageData);
                        logo.ScaleToFit(80, 80);
                        logoCell.Add(logo);
                    }
                    catch (Exception)
                    {
                        logoCell.Add(new Paragraph("RINCÓN").SetFontColor(colorMarron).SetFontSize(16).SimulateBold());
                    }
                }
                logoCell.SetBorder(Border.NO_BORDER).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);

                // Celda del título y subtítulo
                Cell titleCell = new Cell()
                    .Add(new Paragraph("ORDEN DE PEDIDO").SetFontColor(colorMarron).SetFontSize(20).SimulateBold().SetTextAlignment(TextAlignment.CENTER))
                    .Add(new Paragraph("Registro de Productos Solicitados").SetFontSize(14).SetTextAlignment(TextAlignment.CENTER))
                    .SetBorder(Border.NO_BORDER)
                    .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);

                headerTable.AddCell(logoCell);
                headerTable.AddCell(titleCell);
                document.Add(headerTable);

                // Línea separadora
                SolidLine solidLine = new SolidLine();
                solidLine.SetColor(colorMarron);
                LineSeparator lineSeparator = new LineSeparator(solidLine);
                lineSeparator.SetMarginTop(10).SetMarginBottom(15);
                document.Add(lineSeparator);

                // === INFORMACIÓN DEL PEDIDO ===
                document.Add(new Paragraph("INFORMACIÓN DEL PEDIDO").SetFontColor(colorMarron).SetFontSize(14).SimulateBold().SetMarginBottom(10));

                Table clientInfoTable = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 })).UseAllAvailableWidth();
                clientInfoTable.SetMarginBottom(20);

                // Información del cliente
                clientInfoTable.AddCell(new Cell()
                    .Add(new Paragraph("CLIENTE:").SetFontSize(10).SimulateBold().SetFontColor(colorMarron))
                    .Add(new Paragraph(bookingOrder.Client ?? "N/A").SetFontSize(12))
                    .SetBorder(Border.NO_BORDER)
                    .SetBackgroundColor(colorFondoClaro)
                    .SetPadding(8));

                clientInfoTable.AddCell(new Cell()
                    .Add(new Paragraph("TELÉFONO:").SetFontSize(10).SimulateBold().SetFontColor(colorMarron))
                    .Add(new Paragraph(bookingOrder.Phone ?? "N/A").SetFontSize(12))
                    .SetBorder(Border.NO_BORDER)
                    .SetBackgroundColor(colorFondoClaro)
                    .SetPadding(8));

                clientInfoTable.AddCell(new Cell()
                    .Add(new Paragraph("DIRECCIÓN:").SetFontSize(10).SimulateBold().SetFontColor(colorMarron))
                    .Add(new Paragraph(bookingOrder.Address ?? "N/A").SetFontSize(12))
                    .SetBorder(Border.NO_BORDER)
                    .SetBackgroundColor(colorFondoClaro)
                    .SetPadding(8));

                clientInfoTable.AddCell(new Cell()
                    .Add(new Paragraph("FECHA DE PEDIDO:").SetFontSize(10).SimulateBold().SetFontColor(colorMarron))
                    .Add(new Paragraph(bookingOrder.OrderDate.ToString("dd/MM/yyyy")).SetFontSize(12))
                    .SetBorder(Border.NO_BORDER)
                    .SetBackgroundColor(colorFondoClaro)
                    .SetPadding(8));

                clientInfoTable.AddCell(new Cell()
                    .Add(new Paragraph("ESTADO:").SetFontSize(10).SimulateBold().SetFontColor(colorMarron))
                    .Add(new Paragraph(bookingOrder.Status.ToString()).SetFontSize(12))
                    .SetBorder(Border.NO_BORDER)
                    .SetBackgroundColor(colorFondoClaro)
                    .SetPadding(8));

                clientInfoTable.AddCell(new Cell()
                    .Add(new Paragraph("ENVÍO:").SetFontSize(10).SimulateBold().SetFontColor(colorMarron))
                    .Add(new Paragraph(bookingOrder.Shipment ? "Sí" : "No").SetFontSize(12))
                    .SetBorder(Border.NO_BORDER)
                    .SetBackgroundColor(colorFondoClaro)
                    .SetPadding(8));

                document.Add(clientInfoTable);

                // Comentarios si existen
                if (!string.IsNullOrEmpty(bookingOrder.Comments))
                {
                    document.Add(new Paragraph("COMENTARIOS ADICIONALES").SetFontColor(colorMarron).SetFontSize(12).SimulateBold().SetMarginBottom(5));
                    Table commentsTable = new Table(1).UseAllAvailableWidth();
                    commentsTable.AddCell(new Cell()
                        .Add(new Paragraph(bookingOrder.Comments).SetFontSize(11))
                        .SetBorder(new SolidBorder(colorMarron, 1))
                        .SetBackgroundColor(colorFondoClaro)
                        .SetPadding(10)
                        .SetMarginBottom(15));
                    document.Add(commentsTable);
                }

                // === TABLA DE PRODUCTOS ===
                document.Add(new Paragraph("DETALLE DE PRODUCTOS").SetFontColor(colorMarron).SetFontSize(14).SimulateBold().SetMarginBottom(10));

                // Tabla de productos simplificada (sin subtotal)
                Table productTable = new Table(UnitValue.CreatePercentArray(new float[] { 20, 60, 20 })).UseAllAvailableWidth();
                productTable.SetBorder(new SolidBorder(colorMarron, 1));

                // Encabezados
                productTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("ID").SimulateBold().SetFontColor(ColorConstants.WHITE).SetFontSize(12))
                    .SetBackgroundColor(colorMarron)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(10));

                productTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("PRODUCTO").SimulateBold().SetFontColor(ColorConstants.WHITE).SetFontSize(12))
                    .SetBackgroundColor(colorMarron)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(10));

                productTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("CANTIDAD").SimulateBold().SetFontColor(ColorConstants.WHITE).SetFontSize(12))
                    .SetBackgroundColor(colorMarron)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(10));

                // Filas de datos
                bool alternateColor = false;
                int totalItems = 0;
                if (orders != null && orders.Any())
                {
                    foreach (var order in orders)
                    {
                        var rowColor = alternateColor ? ColorConstants.WHITE : colorFondoClaro;
                        totalItems += order.Quantity;

                        productTable.AddCell(new Cell()
                            .Add(new Paragraph(order.ProductId ?? "N/A").SetFontSize(10))
                            .SetBackgroundColor(rowColor)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetPadding(8)
                            .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f)));

                        productTable.AddCell(new Cell()
                            .Add(new Paragraph(order.ProductName ?? "N/A").SetFontSize(11))
                            .SetBackgroundColor(rowColor)
                            .SetPadding(8)
                            .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f)));

                        productTable.AddCell(new Cell()
                            .Add(new Paragraph(order.Quantity.ToString()).SetFontSize(11))
                            .SetBackgroundColor(rowColor)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetPadding(8)
                            .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f)));

                        alternateColor = !alternateColor;
                    }

                    // Fila de total de productos (calculado automáticamente)
                    productTable.AddCell(new Cell(1, 2)
                        .Add(new Paragraph("TOTAL DE PRODUCTOS").SetFontSize(14).SimulateBold())
                        .SetBackgroundColor(colorMarron)
                        .SetFontColor(ColorConstants.WHITE)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPadding(12)
                        .SetBorder(new SolidBorder(colorMarron, 2)));

                    productTable.AddCell(new Cell()
                        .Add(new Paragraph(totalItems.ToString()).SetFontSize(14).SimulateBold())
                        .SetBackgroundColor(colorMarron)
                        .SetFontColor(ColorConstants.WHITE)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPadding(12)
                        .SetBorder(new SolidBorder(colorMarron, 2)));
                }
                else
                {
                    productTable.AddCell(new Cell(1, 3)
                        .Add(new Paragraph("No hay productos en este pedido").SetTextAlignment(TextAlignment.CENTER).SetFontSize(11))
                        .SetPadding(15)
                        .SetBackgroundColor(colorFondoClaro));
                }

                document.Add(productTable);

                // === SECCIÓN DE COSTOS ===
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("RESUMEN DE COSTOS").SetFontColor(colorMarron).SetFontSize(14).SimulateBold().SetMarginBottom(10));

                // Tabla de costos para completar manualmente
                Table costsTable = new Table(UnitValue.CreatePercentArray(new float[] { 70, 30 })).UseAllAvailableWidth();
                costsTable.SetBorder(new SolidBorder(colorMarron, 1));

                // Subtotal
                costsTable.AddCell(new Cell()
                    .Add(new Paragraph("SUBTOTAL:").SetFontSize(12).SimulateBold())
                    .SetBackgroundColor(colorFondoClaro)
                    .SetPadding(10)
                    .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f)));

                costsTable.AddCell(new Cell()
                    .Add(new Paragraph("$ ___________").SetFontSize(12).SetFontColor(ColorConstants.GRAY))
                    .SetBackgroundColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(10)
                    .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f)));

                // Impuestos/Descuentos
                costsTable.AddCell(new Cell()
                    .Add(new Paragraph("IMPUESTOS/DESCUENTOS:").SetFontSize(12).SimulateBold())
                    .SetBackgroundColor(colorFondoClaro)
                    .SetPadding(10)
                    .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f)));

                costsTable.AddCell(new Cell()
                    .Add(new Paragraph("$ ___________").SetFontSize(12).SetFontColor(ColorConstants.GRAY))
                    .SetBackgroundColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(10)
                    .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f)));

                // Total final
                costsTable.AddCell(new Cell()
                    .Add(new Paragraph("TOTAL FINAL:").SetFontSize(14).SimulateBold())
                    .SetBackgroundColor(colorMarron)
                    .SetFontColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(12)
                    .SetBorder(new SolidBorder(colorMarron, 2)));

                costsTable.AddCell(new Cell()
                    .Add(new Paragraph("$ ___________").SetFontSize(14).SimulateBold().SetFontColor(ColorConstants.GRAY))
                    .SetBackgroundColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(12)
                    .SetBorder(new SolidBorder(colorMarron, 2)));

                document.Add(costsTable);

                // === SECCIÓN DE OBSERVACIONES ===
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("OBSERVACIONES ADICIONALES").SetFontColor(colorMarron).SetFontSize(14).SimulateBold().SetMarginBottom(10));

                // Cuadro de observaciones en blanco
                Table observationsTable = new Table(1).UseAllAvailableWidth();
                Cell observationsCell = new Cell()
                    .SetHeight(60)
                    .SetBorder(new SolidBorder(colorMarron, 1))
                    .SetBackgroundColor(ColorConstants.WHITE);
                
                observationsTable.AddCell(observationsCell);
                document.Add(observationsTable);

                // === FIRMAS ===
                document.Add(new Paragraph("\n\n"));
                Table signaturesTable = new Table(UnitValue.CreatePercentArray(new float[] { 33, 34, 33 })).UseAllAvailableWidth();
                
                signaturesTable.AddCell(new Cell()
                    .Add(new Paragraph("_____________________").SetTextAlignment(TextAlignment.CENTER))
                    .Add(new Paragraph("Cliente").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10).SetFontColor(colorMarron))
                    .SetBorder(Border.NO_BORDER)
                    .SetPaddingTop(25));

                signaturesTable.AddCell(new Cell()
                    .Add(new Paragraph("_____________________").SetTextAlignment(TextAlignment.CENTER))
                    .Add(new Paragraph("Vendedor").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10).SetFontColor(colorMarron))
                    .SetBorder(Border.NO_BORDER)
                    .SetPaddingTop(25));

                signaturesTable.AddCell(new Cell()
                    .Add(new Paragraph("_____________________").SetTextAlignment(TextAlignment.CENTER))
                    .Add(new Paragraph("Supervisor").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10).SetFontColor(colorMarron))
                    .SetBorder(Border.NO_BORDER)
                    .SetPaddingTop(25));

                document.Add(signaturesTable);

                // === FOOTER ===
                document.Add(new Paragraph("\n"));
                
                // Línea separadora inferior
                LineSeparator footerLine = new LineSeparator(solidLine);
                footerLine.SetMarginTop(10).SetMarginBottom(5);
                document.Add(footerLine);
                
                document.Add(new Paragraph("RINCÓN - Sistema de Gestión de Pedidos")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(10)
                    .SetFontColor(colorMarron));

                document.Flush();
            }

            Console.WriteLine($"PDF de pedido generado correctamente en: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar PDF de pedido: {ex.Message}");
            throw;
        }
    }

    public static void GenerateStockPdf(List<ProductStock> productsStock, string outputPath, string userName = "Sistema")
    {
        try
        {
            using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            using (PdfWriter writer = new PdfWriter(fs))
            using (PdfDocument pdf = new PdfDocument(writer))
            using (Document document = new Document(pdf, PageSize.A4))
            {
                document.SetMargins(30, 40, 30, 40);
                var colorMarron = new DeviceRgb(173, 107, 42);
                var colorFondoClaro = new DeviceRgb(255, 246, 237);

                // === HEADER SECTION ===
                // Logo y título en la misma línea
                Table headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 30, 70 })).UseAllAvailableWidth();
                
                // Celda del logo
                Cell logoCell = new Cell();
                try
                {
                    using var stream = FileSystem.Current.OpenAppPackageFileAsync("logopdf.png").Result;
                    using var memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);
                    var imageData = ImageDataFactory.Create(memoryStream.ToArray());
                    Image logo = new Image(imageData);
                    logo.ScaleToFit(80, 80);
                    logoCell.Add(logo);
                }
                catch (Exception)
                {
                    try
                    {
                        using var stream = FileSystem.Current.OpenAppPackageFileAsync("logo_login.png").Result;
                        using var memoryStream = new MemoryStream();
                        stream.CopyTo(memoryStream);
                        var imageData = ImageDataFactory.Create(memoryStream.ToArray());
                        Image logo = new Image(imageData);
                        logo.ScaleToFit(80, 80);
                        logoCell.Add(logo);
                    }
                    catch (Exception)
                    {
                        logoCell.Add(new Paragraph("RINCÓN").SetFontColor(colorMarron).SetFontSize(16).SimulateBold());
                    }
                }
                logoCell.SetBorder(Border.NO_BORDER).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);

                // Celda del título y subtítulo
                Cell titleCell = new Cell()
                    .Add(new Paragraph("CONTROL DE STOCK").SetFontColor(colorMarron).SetFontSize(20).SimulateBold().SetTextAlignment(TextAlignment.CENTER))
                    .Add(new Paragraph("Registro de Productos Agregados").SetFontSize(14).SetTextAlignment(TextAlignment.CENTER))
                    .SetBorder(Border.NO_BORDER)
                    .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);

                headerTable.AddCell(logoCell);
                headerTable.AddCell(titleCell);
                document.Add(headerTable);

                // Línea separadora
                SolidLine solidLine = new SolidLine();
                solidLine.SetColor(colorMarron);
                LineSeparator lineSeparator = new LineSeparator(solidLine);
                lineSeparator.SetMarginTop(10).SetMarginBottom(15);
                document.Add(lineSeparator);

                // === INFORMACIÓN DEL DOCUMENTO ===
                Table infoTable = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 })).UseAllAvailableWidth();
                infoTable.SetMarginBottom(20);

                // Información de fecha y usuario
                infoTable.AddCell(new Cell()
                    .Add(new Paragraph("FECHA DE GENERACIÓN:").SetFontSize(10).SimulateBold().SetFontColor(colorMarron))
                    .Add(new Paragraph(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")).SetFontSize(12))
                    .SetBorder(Border.NO_BORDER)
                    .SetBackgroundColor(colorFondoClaro)
                    .SetPadding(8));

                infoTable.AddCell(new Cell()
                    .Add(new Paragraph("USUARIO RESPONSABLE:").SetFontSize(10).SimulateBold().SetFontColor(colorMarron))
                    .Add(new Paragraph(userName).SetFontSize(12))
                    .SetBorder(Border.NO_BORDER)
                    .SetBackgroundColor(colorFondoClaro)
                    .SetPadding(8));

                // Total de productos
                int totalProductos = productsStock?.Count ?? 0;
                int totalCantidad = productsStock?.Sum(p => p.Quantity) ?? 0;

                infoTable.AddCell(new Cell()
                    .Add(new Paragraph("TOTAL DE PRODUCTOS:").SetFontSize(10).SimulateBold().SetFontColor(colorMarron))
                    .Add(new Paragraph($"{totalProductos} productos").SetFontSize(12))
                    .SetBorder(Border.NO_BORDER)
                    .SetBackgroundColor(colorFondoClaro)
                    .SetPadding(8));

                infoTable.AddCell(new Cell()
                    .Add(new Paragraph("CANTIDAD TOTAL:").SetFontSize(10).SimulateBold().SetFontColor(colorMarron))
                    .Add(new Paragraph($"{totalCantidad} unidades").SetFontSize(12))
                    .SetBorder(Border.NO_BORDER)
                    .SetBackgroundColor(colorFondoClaro)
                    .SetPadding(8));

                document.Add(infoTable);

                // === TABLA DE PRODUCTOS ===
                document.Add(new Paragraph("DETALLE DE PRODUCTOS").SetFontColor(colorMarron).SetFontSize(14).SimulateBold().SetMarginBottom(10));

                // Tabla simplificada con solo Producto y Cantidad
                Table productTable = new Table(UnitValue.CreatePercentArray(new float[] { 70, 30 })).UseAllAvailableWidth();
                productTable.SetBorder(new SolidBorder(colorMarron, 1));

                // Encabezados
                productTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("PRODUCTO").SimulateBold().SetFontColor(ColorConstants.WHITE).SetFontSize(12))
                    .SetBackgroundColor(colorMarron)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(10));

                productTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("CANTIDAD").SimulateBold().SetFontColor(ColorConstants.WHITE).SetFontSize(12))
                    .SetBackgroundColor(colorMarron)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(10));

                // Filas de datos
                bool alternateColor = false;
                if (productsStock != null && productsStock.Any())
                {
                    foreach (var product in productsStock)
                    {
                        var rowColor = alternateColor ? ColorConstants.WHITE : colorFondoClaro;

                        productTable.AddCell(new Cell()
                            .Add(new Paragraph(product.Product?.Description ?? "N/A").SetFontSize(11))
                            .SetBackgroundColor(rowColor)
                            .SetPadding(8)
                            .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f)));

                        productTable.AddCell(new Cell()
                            .Add(new Paragraph(product.Quantity.ToString()).SetFontSize(11))
                            .SetBackgroundColor(rowColor)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetPadding(8)
                            .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f)));

                        alternateColor = !alternateColor;
                    }
                }
                else
                {
                    productTable.AddCell(new Cell(1, 2)
                        .Add(new Paragraph("No hay productos registrados").SetTextAlignment(TextAlignment.CENTER).SetFontSize(11))
                        .SetPadding(15)
                        .SetBackgroundColor(colorFondoClaro));
                }

                document.Add(productTable);

                // === SECCIÓN DE OBSERVACIONES ===
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("OBSERVACIONES").SetFontColor(colorMarron).SetFontSize(14).SimulateBold().SetMarginBottom(10));

                // Cuadro de observaciones en blanco
                Table observationsTable = new Table(1).UseAllAvailableWidth();
                Cell observationsCell = new Cell()
                    .SetHeight(80)
                    .SetBorder(new SolidBorder(colorMarron, 1))
                    .SetBackgroundColor(ColorConstants.WHITE);
                
                observationsTable.AddCell(observationsCell);
                document.Add(observationsTable);

                // === FIRMAS ===
                document.Add(new Paragraph("\n\n"));
                Table signaturesTable = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 })).UseAllAvailableWidth();
                
                signaturesTable.AddCell(new Cell()
                    .Add(new Paragraph("_________________________").SetTextAlignment(TextAlignment.CENTER))
                    .Add(new Paragraph("Responsable de Stock").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10).SetFontColor(colorMarron))
                    .SetBorder(Border.NO_BORDER)
                    .SetPaddingTop(30));

                signaturesTable.AddCell(new Cell()
                    .Add(new Paragraph("_________________________").SetTextAlignment(TextAlignment.CENTER))
                    .Add(new Paragraph("Supervisor").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10).SetFontColor(colorMarron))
                    .SetBorder(Border.NO_BORDER)
                    .SetPaddingTop(30));

                document.Add(signaturesTable);

                // === FOOTER ===
                document.Add(new Paragraph("\n"));
                
                // Línea separadora inferior
                LineSeparator footerLine = new LineSeparator(solidLine);
                footerLine.SetMarginTop(10).SetMarginBottom(5);
                document.Add(footerLine);
                
                document.Add(new Paragraph("RINCÓN - Sistema de Gestión de Stock")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(10)
                    .SetFontColor(colorMarron));

                document.Flush();
            }

            Console.WriteLine($"PDF de stock generado correctamente en: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar PDF de stock: {ex.Message}");
            throw;
        }
    }

}
