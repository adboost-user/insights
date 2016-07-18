using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web;
namespace AdBoostDashboard
{
    public class PdfCreate : IPdfPTableEvent
    {
        //string fileName = Guid.NewGuid() + ".pdf";
        //string filePath = Path.Combine(Server.MapPath(("/UploadedFiles")));
        /// <summary>
        /// metodo principal de escritura
        /// </summary>
        /// <param name="stream"></param>
        public void Write(Stream stream)
        {
            // step 1
            using (Document document = new Document(PageSize.A4.Rotate()))
            {
                // step 2
                PdfWriter.GetInstance(document, stream);
                // step 3
                document.Open();
                // step 4
                List<string> days = new List<string>();
                days.Add("Lunes");
                days.Add("Martes");
                days.Add("Miercoles");
                days.Add("Jueves");
                days.Add("Viernes");
                IPdfPTableEvent Pevent = new PdfCreate();
                foreach (string day in days)
                {
                    PdfPTable table = GetTable(day);
                    table.TableEvent = Pevent;
                    document.Add(table);
                    document.NewPage();
                }
            }
        }

        /// <summary>
        /// obtiene datos de tabla
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public PdfPTable GetTable(string day)
        {
            PdfPTable table = new PdfPTable(new float[] { 2, 1, 2, 5, 1 });
            table.WidthPercentage = 100f;
            table.DefaultCell.Padding = 3;
            table.DefaultCell.UseAscender = true;
            table.DefaultCell.UseDescender = true;
            table.DefaultCell.Colspan = 5;
            table.DefaultCell.BackgroundColor = BaseColor.RED;
            table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(day);
            table.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.DefaultCell.Colspan = 1;
            table.DefaultCell.BackgroundColor = BaseColor.ORANGE;
            for (int i = 0; i < 2; i++)
            {
                table.AddCell("Location");
                table.AddCell("Time");
                table.AddCell("Run Length");
                table.AddCell("Title");
                table.AddCell("Year");
            }
            table.DefaultCell.BackgroundColor = null;
            table.HeaderRows = 3;
            table.FooterRows = 1;
            List<string> screenings = new List<string>();
            screenings.Add("Pelicula");
            screenings.Add("aca");
            screenings.Add(Convert.ToString(DateTime.Now));
            screenings.Add("Duracion");
            screenings.Add("Mi pelicula");
            screenings.Add("2016");
            foreach (string screening in screenings)
            {
                table.AddCell("Movie");
                table.AddCell("Ubicacion");
                table.AddCell("Tiempo");
                table.AddCell("Duracion");
                table.AddCell("titulo");
                table.AddCell("2016");
            }
            return table;
        }
        /// <summary>
        /// bakground para las filas
        /// </summary>
        /// <param name="table"></param>
        /// <param name="widths"></param>
        /// <param name="heights"></param>
        /// <param name="headerRows"></param>
        /// <param name="rowStart"></param>
        /// <param name="canvases"></param>
        public void TableLayout(
              PdfPTable table, float[][] widths, float[] heights,
              int headerRows, int rowStart, PdfContentByte[] canvases
              )
            {
                int columns;
                Rectangle rect;
                int footer = widths.Length - table.FooterRows;
                int header = table.HeaderRows - table.FooterRows + 1;
                for (int row = header; row < footer; row += 2)
                {
                    columns = widths[row].Length - 1;
                    rect = new Rectangle(
                      widths[row][0], heights[row],
                      widths[row][columns], heights[row + 1]
                    );
                    rect.BackgroundColor = BaseColor.YELLOW;
                    rect.Border = Rectangle.NO_BORDER;
                    canvases[PdfPTable.BASECANVAS].Rectangle(rect);
                }
            }    
    }
}