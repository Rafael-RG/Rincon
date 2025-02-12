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
                document.SetMargins(30, 40, 20, 40);



                // Cargar el logo desde recursos de MAUI
                try
                {
                    using var stream = FileSystem.Current.OpenAppPackageFileAsync("logopdf.png").Result;
                    using var memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);
                    var imageData = ImageDataFactory.Create(memoryStream.ToArray());
                    Image logo = new Image(imageData);
                    logo.ScaleToFit(120, 120);
                    logo.SetFixedPosition(30, PageSize.A4.GetHeight() - 100);
                    document.Add(logo);
                }
                catch (Exception)
                {
                    Console.WriteLine("No se pudo cargar el logo desde los recursos de MAUI.");
                }

                var colorMarron = new DeviceRgb(173, 107, 42);


                // Título
                document.Add(new Paragraph("Pedido").SetFontColor(colorMarron)
                    .SetFontSize(18).SimulateBold().SetTextAlignment(TextAlignment.CENTER));


                // Línea separadora con color AD6B2A
                SolidLine solidLine = new SolidLine();
                solidLine.SetColor(new DeviceRgb(173, 107, 42)); // AD6B2A en RGB

                LineSeparator lineSeparator = new LineSeparator(solidLine);
                lineSeparator.SetMarginTop(5).SetMarginBottom(10);

                document.Add(lineSeparator);

                // Información general del pedido en dos columnas
                Table infoTable = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 })).UseAllAvailableWidth();
                infoTable.AddCell(new Cell().Add(new Paragraph($"Cliente: {bookingOrder.Client}")).SetBorder(Border.NO_BORDER));
                infoTable.AddCell(new Cell().Add(new Paragraph($"Dirección: {bookingOrder.Address}")).SetBorder(Border.NO_BORDER));
                infoTable.AddCell(new Cell().Add(new Paragraph($"Teléfono: {bookingOrder.Phone}")).SetBorder(Border.NO_BORDER));
                infoTable.AddCell(new Cell().Add(new Paragraph($"Fecha de Pedido: {bookingOrder.OrderDate:dd/MM/yyyy}")).SetBorder(Border.NO_BORDER));
                infoTable.AddCell(new Cell().Add(new Paragraph($"Entrega: {bookingOrder.Status}")).SetBorder(Border.NO_BORDER));
                infoTable.AddCell(new Cell().Add(new Paragraph($"Comentarios: {bookingOrder.Comments}")).SetBorder(Border.NO_BORDER));

                document.Add(infoTable);
                document.Add(new Paragraph(" "));  // Separación

                // Crear tabla de productos
                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 20, 40, 20, 20 })).UseAllAvailableWidth();

                // Encabezados de la tabla
                table.AddHeaderCell(new Cell().Add(new Paragraph("ID Producto")).SetBackgroundColor(colorMarron).SimulateBold());
                table.AddHeaderCell(new Cell().Add(new Paragraph("Nombre")).SetBackgroundColor(colorMarron).SimulateBold());
                table.AddHeaderCell(new Cell().Add(new Paragraph("Cantidad")).SetBackgroundColor(colorMarron).SimulateBold());

                // Colorear las filas alternadas
                bool alternateColor = false;
                foreach (var order in orders)
                {
                    var rowColor = alternateColor ? ColorConstants.WHITE : new DeviceRgb(255, 246, 237);

                    table.AddCell(new Cell().Add(new Paragraph(order.ProductId)).SetBackgroundColor(rowColor));
                    table.AddCell(new Cell().Add(new Paragraph(order.ProductName)).SetBackgroundColor(rowColor));
                    table.AddCell(new Cell().Add(new Paragraph(order.Quantity.ToString())).SetBackgroundColor(rowColor));

                    alternateColor = !alternateColor;
                }

                // Agregar la tabla al documento
                document.Add(table);

                // Cerrar el documento
                document.Flush();
            }

            Console.WriteLine($"PDF generado correctamente en: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar PDF: {ex.Message}");
        }
    }



}
