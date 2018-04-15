using System;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using FirstCargoApp.Models;
using System.Data;



namespace FirstCargoApp.Report
{
    public class ReportManager
    {
        

        public void generateReport(object dataTable)
        {
            DataTable dt = ObjectToData(dataTable);

            DataRow dr = dt.Rows[0];

            Document document = new Document(PageSize.A4, 88f, 88f, 10f, 10f);
            Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                Phrase phrase = null;
                PdfPCell cell = null;
                PdfPTable table = null;
                BaseColor color = null;

                document.Open();

                //Header Table
                table = new PdfPTable(2);
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                table.SetWidths(new float[] { 0.3f, 0.7f });

                //Company Name and Address
                phrase = new Phrase();
                phrase.Add(new Chunk(@ViewResources.Resource.FirstCargo + "\n\n", FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.RED)));
                phrase.Add(new Chunk("Franz-Grashof Strasse 16-18" + "\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("68199 Mannheim" + "\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("BW, Germany", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT);
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                table.AddCell(cell);
   
                //Company Logo
                cell = ImageCell("~/fonts/FirstCargoLogo.png", 30f, PdfPCell.ALIGN_RIGHT);
                table.AddCell(cell);
                table.SpacingBefore = 40f;

                //Separater Line
                color = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#A9A9A9"));
                DrawLine(writer, 25f, document.Top - 80f, document.PageSize.Width - 25f, document.Top - 80f, color);
                document.Add(table);

                table = new PdfPTable(3);
                //table.HorizontalAlignment = Element.ALIGN_LEFT;
                //table.WidthPercentage = 100;
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                table.SetWidths(new float[] { 0.4f, 0.2f, 0.5f });
                
                

                phrase = new Phrase();
                phrase.Add(new Chunk("Herr / Frau " + dr["senderName"] + "\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk(dr["senderAdress"] + "\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk(dr["senderPhoneNumber"] + "\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk(dr["senderEmail"].ToString(), FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT);
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Colspan = 2;
                table.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("Steve Mensah" + "\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("Fist Cargo Manheim" + "\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("Franz-Grashof Strasse 16-18" + "\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("68199 Mannheim,\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("Tel: 0049 6218 033 397" + "\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("E-Mail: info@stevemensah.com"+ "\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("www.stevemensah.com" + "\n\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("Date: " + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year, FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT);
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                table.AddCell(cell);
                table.SpacingBefore = 20f;
                document.Add(table);

                // Rechnung Nr
                table = new PdfPTable(1);
                table.TotalWidth = 500f;
                table.SetWidths(new float[] { 0.5f });
                table.LockedWidth = true;
                table.SpacingBefore = 40f;
                //table.HorizontalAlignment = Element.ALIGN_LEFT;

                phrase = new Phrase();
                phrase.Add(new Chunk("RECHNUNG NR/" + @ViewResources.Resource.OrderNumber + ": " + dr["orderNumber"], FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.BLACK)));
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT);
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                table.AddCell(cell);
                document.Add(table);

                //Anrede + Text

                table = new PdfPTable(1);
                //table.SetWidths(new float[] { 0.5f, 2f });
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                table.SpacingBefore = 30f;
                //table.HorizontalAlignment = Element.ALIGN_LEFT;

                phrase = new Phrase();
                phrase.Add(new Chunk("Sehr geehrte(r) Herr/Frau " + dr["senderName"] +" "+ "\n\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("hiermit stellen wir Ihnen folgende Posten in Rechnung: Transport Ihre Ware von Mannheim nach "+ dr["destination"].ToString() + "." +
                    "\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT);
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                table.AddCell(cell);
                document.Add(table);

                //Reciever Data
                table = new PdfPTable(2);
                table.SetWidths(new float[] { 0.5f, 2f });
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                //table.HorizontalAlignment = Element.ALIGN_LEFT;

                table.AddCell(PhraseCell(new Phrase(@ViewResources.Resource.RecieverName + " :", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                phrase = new Phrase(new Chunk(dr["recieverName"] + "\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk(dr["recieverAdress"] + "\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk(dr["recieverPhoneNumber"] + " / " + dr["SenderEmail"], FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                table.AddCell(PhraseCell(phrase, PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.PaddingBottom = 10f;
                table.AddCell(cell);
                table.SpacingBefore = 20f;
                document.Add(table);

                // Data
                table = new PdfPTable(2);
                table.SetWidths(new float[] { 0.5f, 2f });
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                phrase = new Phrase();

                table.AddCell(PhraseCell(new Phrase("Object" + " :", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                if (dr.Table.Columns.Contains("otherType"))
                {
                    phrase.Add(new Chunk(dr["otherType"] + "\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                }
                else if (dr.Table.Columns.Contains("vehiculeType"))
                {
                    phrase.Add(new Chunk(dr["vehiculeType"] + " / " + dr["frameNumber"] + "\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                }
                else
                {
                    phrase.Add(new Chunk(dr["packageType"].ToString()+ "\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                    phrase.Add(new Chunk(ViewResources.Resource.Weight + "/" + ViewResources.Resource.Height + "/" + ViewResources.Resource.Length + "/" + ViewResources.Resource.Depth + ": " +
                        dr["weight"] + "/" + dr["height"] + "/" + dr["length"] + "/" + dr["depth"] + "\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                }
                    phrase.Add(new Chunk(dr["destination"].ToString(), FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                
                table.AddCell(PhraseCell(phrase, PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.PaddingBottom = 10f;
                table.AddCell(cell);
                table.SpacingBefore = 20f;
                document.Add(table);

                // Data
                table = new PdfPTable(2);
                table.SetWidths(new float[] { 0.5f, 2f });
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                table.AddCell(PhraseCell(new Phrase(@ViewResources.Resource.Price + " :", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                phrase = new Phrase();
                phrase.Add(new Chunk(dr["price"].ToString(), FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                table.AddCell(PhraseCell(phrase, PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.PaddingBottom = 10f;
                table.AddCell(cell);
                table.SpacingBefore = 20f;
                document.Add(table);

                // Paid
                table = new PdfPTable(2);
                table.SetWidths(new float[] { 0.5f, 2f });
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                table.AddCell(PhraseCell(new Phrase(@ViewResources.Resource.Paid + " :", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                phrase = new Phrase();
                phrase.Add(new Chunk(dr["alReadyPaid"].ToString(), FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                table.AddCell(PhraseCell(phrase, PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.PaddingBottom = 10f;
                table.AddCell(cell);
                table.SpacingBefore = 20f;
                document.Add(table);

                // Rest to pay
                table = new PdfPTable(2);
                table.SetWidths(new float[] { 0.5f, 2f });
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                table.AddCell(PhraseCell(new Phrase(@ViewResources.Resource.RestToPay + " :", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                phrase = new Phrase();
                phrase.Add(new Chunk(dr["paidRest"].ToString(), FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                table.AddCell(PhraseCell(phrase, PdfPCell.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.PaddingBottom = 10f;
                table.AddCell(cell);
                table.SpacingBefore = 20f;
                document.Add(table);

                //Anrede + Text

                table = new PdfPTable(1);
                //table.SetWidths(new float[] { 0.5f, 2f });
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                table.SpacingBefore = 20f;
                //table.HorizontalAlignment = Element.ALIGN_LEFT;

                phrase = new Phrase();
                phrase.Add(new Chunk("Hierbei handelt es sich um eine im Inland nicht steuerbare sonstige Leistung nach der Reverse-Charge-Regelung." + "\n\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("Siehe Seevertrag  und Parkplatz Bedingungen" +
                    "\n\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("Mit freundlichen Grüßen", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT);
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                table.AddCell(cell);
                document.Add(table);

                // Gruß Formel + Unterschrift
                
                table = new PdfPTable(2);
                //table.SetWidths(new float[] { 0.5f, 2f });
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                table.AddCell(PhraseCell(new Phrase("Ihr First Cargo Mannheim", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)), PdfPCell.ALIGN_LEFT));
                phrase = new Phrase();
                phrase.Add(new Chunk("Unterschrift Kunde", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)));
                table.AddCell(PhraseCell(phrase, PdfPCell.ALIGN_RIGHT));
                cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
                cell.PaddingBottom = 10f;
                table.AddCell(cell);
                table.SpacingBefore = 10f;
                document.Add(table);

                // Bank Daten

                table = new PdfPTable(3);
                //table.HorizontalAlignment = Element.ALIGN_LEFT;
                //table.WidthPercentage = 100;
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                table.SetWidths(new float[] { 0.4f, 0.2f, 0.5f });

                phrase = new Phrase();
                phrase.Add(new Chunk("Commerz-Bank-Mannheim" + "\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("Konto Nr. 0664251202" + "\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("BLZ  67080050" + "\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT);
                cell.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                cell.Colspan = 2;
                table.AddCell(cell);

                phrase = new Phrase();
                phrase.Add(new Chunk("BIC DRESDEFF670" + "\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                phrase.Add(new Chunk("IBAN DE 63 67080050 0664251202" + "\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)));
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT);
                cell.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                table.AddCell(cell);
                table.SpacingBefore = 70f;
                document.Add(table);
                




                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();

                try
                {
                HttpContext.Current.Response.Clear();

                System.Web.HttpContext.Current.Response.AddHeader("Transfer-Encoding", "identity");

                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=Report.pdf");
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.BinaryWrite(bytes);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                HttpContext.Current.Response.End();
                HttpContext.Current.Response.Close();

                }
                catch (Exception ex)
                {

                }

             
            }
        }

        private static void DrawLine(PdfWriter writer, float x1, float y1, float x2, float y2, BaseColor color)
        {
            PdfContentByte contentByte = writer.DirectContent;
            contentByte.SetColorStroke(color);
            contentByte.MoveTo(x1, y1);
            contentByte.LineTo(x2, y2);
            contentByte.Stroke();
        }
        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.BorderColor = BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 0f;
            return cell;
        }
        private static PdfPCell ImageCell(string path, float scale, int align)
        {
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(path));
            image.ScalePercent(scale);
            PdfPCell cell = new PdfPCell(image);
            cell.BorderColor = BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 0f;
            cell.PaddingTop = 0f;
            return cell;
        }

        public static DataTable ObjectToData(object o)
        {
            DataTable dt = new DataTable("OutputData");

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            o.GetType().GetProperties().ToList().ForEach(f =>
            {
                try
                {
                    f.GetValue(o, null);
                    if (f.PropertyType.Name.Contains("Nullable"))
                        dt.Columns.Add(f.Name, typeof(String));
                    else
                        dt.Columns.Add(f.Name, f.PropertyType);
                    dt.Rows[0][f.Name] = f.GetValue(o, null);
                }
                catch { }
            });
            return dt;
        }
    }
}