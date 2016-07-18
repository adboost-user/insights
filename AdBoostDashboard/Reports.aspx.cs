using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Google.Api.Ads.Dfp.Lib;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;



namespace AdBoostDashboard
{
    public partial class Reports : System.Web.UI.Page
    {
        db vBaseDatos;
        DfpUser user;
        DfpApi vGapi;
        ReportDfp vReport;
        string vTotImpresiones = "";
        string vTotClics = "";
        string vCTR = "";
        string LineTitle = "";
        string ReportDate = "";
        decimal vTotImpreDecimal=0;
        decimal vTotClicDecimal = 0;
        decimal vCTRdecimal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            user = new DfpUser();
            vGapi = new DfpApi();
            vBaseDatos = new db();
            vReport = new ReportDfp();
            try
            {
                LbError.Text = "";
                if (!Convert.ToBoolean(Session["autenticado"]))
                    Response.Redirect("Login.aspx");
                divCompanyId.Visible = false;
                
                String vCompany = Convert.ToString(Session["companyId"]);
                string vLista = "";
                string vListUsers = "";
                string vEstado="";
                string vListaCamp = "";

                string vJsonData = vGapi.GetOrder(vCompany);
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(vJsonData, (typeof(DataTable)));
                if (dt.Rows.Count > 0)
                {
                    vLista = "<option selected>Seleccione una opción</option>";
                    vListaCamp = "<option selected>Seleccione una opción</option>";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        vLista += "<option value='" + dt.Rows[i][2].ToString() + "'>" + dt.Rows[i][3].ToString() + "</option>";
                    }
                    LitLineaAnuncios.Text = vLista;
                   

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i != 0)
                        {
                            if (dt.Rows[i][1].ToString() != dt.Rows[i - 1][1].ToString())
                                vListaCamp += "<option value='" + dt.Rows[i][0].ToString() + "'>" + dt.Rows[i][1].ToString() + "</option>";
                        }
                        else
                        {
                            vListaCamp += "<option value='" + dt.Rows[i][0].ToString() + "'>" + dt.Rows[i][1].ToString() + "</option>";
                        }
                    }
                    //LitCamp.Text = vListaCamp;
                }
                
               
                
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        public DataTable GetUsuarios()
        {
            try
            {
                String vQuery = "CALL GetAllUsers();";
                DataTable vDatos = vBaseDatos.ObtenerTabla(vQuery);
                return vDatos;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable GetCompanyList()
        {
            try
            {
                String vQuery = "CALL GetAllCompanies();";
                DataTable vDatos = vBaseDatos.ObtenerTabla(vQuery);
                return vDatos;
            }
            catch (Exception)
            {
                
                throw;
            }
          

        }
        protected void LbSubmit_Click(object sender, EventArgs e)
        {
            string vResult = "";
            try
            {
               // string vPri=Request.Form["privilegio"];
                //string vCompany = Request.Form["Company"];
                string vLinea = Request.Form["Linea"];
               // string vCamp = Request.Form["Order"];
                long vLineaId = Convert.ToInt32(vLinea);
                string opcion = OpcionReporte.SelectedIndex.ToString();
                if (vLineaId > 0)
                {
                    if (opcion!="")
                    {
                        
                        if (rbExcel.Checked)
                        {
                            GenerateExcel(vLineaId, opcion);
                        }
                        else if (rbPdf.Checked)
                        {
                            GeneratePdf(vLineaId, opcion);
                            LbError.Text = "Selecciono" + rbPdf.Text;
                        }
                        else {
                            LbError.Text = "Seleccione una opción PDF/Excell";
                        }
                    }
                    else
                    {
                        LbError.Text = "Seleccione una opción";
                    }
                    
                }
                else
                {
                    LbError.Text = "Seleccione un anuncio";
                }
            
                //Response.Write("<script>window.open('"+vResultado+"','_blank');</script>");
                

            }
            catch (Exception ex)
            {

                vResult = ex.ToString();
            }           
        }
        private void GenerateExcel(long lineaId, string opcion)
        {
            try
            {
                string vResultado = vReport.CreateReportbyLine(lineaId, opcion);
                if (vResultado != "")
                {
                    bool vWeb = vResultado.Contains("https");
                    if (vWeb)
                        Response.Redirect(vResultado.Trim(), false);
                    LbError.Text = "Archivo descargado";
                }
                else
                {
                    LbError.Text = "No se descargo el documento, revise sus opciones";
                }

            }
            catch (Exception ex)
            {
                
                
            }
        }

        private void GeneratePdf(long lineaId,string opcion)
        {
            string fileName = Guid.NewGuid() + ".pdf";            
            DataTable dt = new DataTable();
            dt = vReport.CreatePdfReport(lineaId, opcion);
            /*System.DateTime TodayDate = System.DateTime.Now;
            Random rnd = new Random();
            int fileNum = rnd.Next(1, 1000);
            string CampTitle = Convert.ToString(dt.Rows[0][0]);
            string RegexTitle = Regex.Replace(CampTitle, "^[0-9A-Za-z ]+$", String.Empty);*/
            //string fileName = Convert.ToString(dt.Rows[0][0]) + " " + Convert.ToString(TodayDate) + Convert.ToString(fileNum) + ".pdf";
            //string fileName = CampTitle + " " + Convert.ToString(fileNum) + ".pdf";
            string filePath = Path.Combine(Server.MapPath("/PDFfiles"),fileName);
            MemoryStream memStream = new MemoryStream();
            Decimal vDecimal = 0;
            Decimal vDecTot = 0;
            Decimal vDecClic = 0;
            string vTotImpresiones = "";
            string vTotClics = "";
            string vCTR = "";
            
           
            Document doc = new Document(PageSize.A4, 2, 2, 2, 2);
            Paragraph p = new Paragraph("Reporte");
            p.Alignment = 1;

            try
            {
                if (dt.Rows.Count != 0)
                {
                    int i = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        vDecTot += Convert.ToDecimal(dt.Rows[i][3]);
                        vDecClic += Convert.ToDecimal(dt.Rows[i][4]);
                        //vDecimal += Convert.ToDecimal(dt.Rows[i][5]);
                        i++;
                    }
                    //vDecimal = 0;
                    vDecimal = (Convert.ToDecimal(vDecClic) / Convert.ToDecimal(vDecTot));
                    vTotImpresiones = vDecTot.ToString("0,0");
                    vTotClics = vDecClic.ToString("0,0");
                    vCTR = vDecimal.ToString("P");
                }
           
                PdfWriter writer = PdfWriter.GetInstance(doc, memStream);   
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                PdfPTable pdftab = new PdfPTable(5);
                doc.Open();
                ColumnText column = new ColumnText(writer.DirectContent);

                // COlumn definition
                 float[][] x = {
                new float[] { doc.Left, doc.Left + 380 },
                new float[] { doc.Right - 380, doc.Right }
                };
                pdftab.HorizontalAlignment = 1;//0-left 1-Center 2-Right
                //pdftab.SpacingBefore=10f;
                //pdftab.SpacingAfter = 10f;
            
                //ColumnText column = new ColumnText(writer.DirectContent);
                if (dt.Rows.Count > 0)
                {
                    /*HEADER*/
                    int count = 0;
                    float height = 0;
                    int status = 0;
                    int j = 0;
                    PdfPTable header = new PdfPTable(3);
                    header.WidthPercentage = 100;
                    header.DefaultCell.BackgroundColor = BaseColor.BLACK;
                    Font font = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.WHITE);
                    Phrase pheader = new Phrase("Reporte", font);
                    header.AddCell(pheader);
                    header.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pheader = new Phrase("Adboost", font);
                    header.AddCell(pheader);
                    header.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pheader = new Phrase(Convert.ToString(DateTime.Now), font);
                    header.AddCell(pheader);
                    doc.Add(header);
                    /*END HEADER*/
                    doc.Add(Chunk.NEWLINE);
                    doc.Add(Chunk.NEWLINE);
                    doc.Add(new Paragraph(" "));
                    /*Phrase phspace = new Phrase("", font);
                    header.AddCell(phspace);
                    header.AddCell(phspace);
                    header.AddCell(phspace);
                    doc.Add(header);
                    doc.Add(header);*/
                    
                    //doc.Add(header);
                    /*END HEADER*/
                    /*IMAGEN*/
                    iTextSharp.text.Image img;
                    float x1 = 11.5f;
                    float y = 769.7f;
                    var physicalPath = Server.MapPath("~/img/adboost-dc-logo.png");
                    img = iTextSharp.text.Image.GetInstance(physicalPath);
                    img.ScaleToFit(600, 60);
                    img.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                    img.Border = Rectangle.BOX;
                    //img.SetAbsolutePosition(x1 + (45 - img.ScaledWidth) / 2, y);
                    //img.SetAbsolutePosition(0, 480);                    
                    doc.Add(img);
                    doc.Add(Chunk.NEWLINE);
                    doc.Add(new Paragraph(" "));

                    /*IMAGEN*/

                    /*HEADER*/
                    //header.DefaultCell.BackgroundColor = BaseColor.tras;
                    PdfPTable headerTit = new PdfPTable(3);
                    Phrase phTitulos;
                    headerTit.WidthPercentage = 80;
                    headerTit.DefaultCell.BackgroundColor = BaseColor.WHITE;
                    font = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.BLACK);
                    phTitulos = new Phrase("IMPRESIONES " + vTotImpresiones, font);
                    headerTit.AddCell(phTitulos);
                    headerTit.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    phTitulos = new Phrase("CLICS " + vTotClics, font);
                    headerTit.AddCell(phTitulos);
                    headerTit.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    phTitulos = new Phrase("CTR " + vCTR, font);
                    headerTit.AddCell(phTitulos);
                    doc.Add(headerTit);
                    doc.Add(Chunk.NEWLINE);
                    

                    /*TABLE HEADER*/
                    pdftab.DefaultCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    Phrase TableTitHeader = new Phrase();
                    foreach (DataColumn dtColumn in dt.Columns)
                    {
                        if (j != 1)
                        {
                            string titulo = dtColumn.ColumnName;
                            TableTitHeader = new Phrase(titulo, font);
                            pdftab.WidthPercentage = 80;
                            pdftab.AddCell(TableTitHeader);
                        }
                        j++;
                    }
                    /*TABLE BODY*/
                    int i = 0;
                    pdftab.DefaultCell.BackgroundColor = null;
                    Phrase tableText = new Phrase();
                    Font tableFont = new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK);
                    foreach (DataRow dr in dt.Rows)
                    {
                        LineTitle = Convert.ToString(dt.Rows[i][0]);
                        ReportDate = Convert.ToString(dt.Rows[i][2]);
                        vTotImpreDecimal = Convert.ToDecimal(dt.Rows[i][3]);
                        vTotClicDecimal = Convert.ToDecimal(dt.Rows[i][4]);
                        vCTRdecimal = Convert.ToDecimal(dt.Rows[i][5]);
                        vTotImpresiones = vTotImpreDecimal.ToString("0,0");
                        vTotClics = vTotClicDecimal.ToString("0,0");
                        vCTR = vCTRdecimal.ToString("P");                        
                        tableText=new Phrase(LineTitle,tableFont);
                            pdftab.AddCell(tableText);
                        tableText=new Phrase(ReportDate,tableFont);
                            pdftab.AddCell(tableText);
                        tableText=new Phrase(vTotImpresiones,tableFont);
                            pdftab.AddCell(tableText);
                        tableText=new Phrase(vTotClics,tableFont);
                            pdftab.AddCell(tableText);
                        tableText=new Phrase(vCTR,tableFont);
                            pdftab.AddCell(tableText);
                        i++;
                    }
                    /*TABLE FOOTER*/
                    pdftab.DefaultCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    j = 0;
                    foreach (DataColumn dtColumn in dt.Columns)
                    {
                        if (j != 1)
                        {
                            string titulo = dtColumn.ColumnName;
                            TableTitHeader = new Phrase(titulo, font);
                            pdftab.WidthPercentage = 80;
                            pdftab.AddCell(TableTitHeader);
                        }
                        j++;
                    }
                }
                //List<string> days = new List<string>();
                //days.Add("Lunes");
                //days.Add("Martes");
                //days.Add("Miercoles");
                //days.Add("Jueves");
                //days.Add("Viernes");
                //foreach(string day in days)
                //{
                //    pdftab.AddCell("1");
                //    pdftab.AddCell("2");
                //    pdftab.AddCell("3");
                //    pdftab.AddCell("4");
                //    pdftab.AddCell("5");
                //}
             
                //doc.Add(p);
                doc.Add(pdftab);
                doc.Close();

                byte[] content = File.ReadAllBytes(filePath);
                HttpContext context = HttpContext.Current;

                context.Response.BinaryWrite(content);
                context.Response.ContentType = "application/pdf";
                context.Response.AppendHeader("Content-Disposition","attachment; filename="+fileName);
                context.Response.End();

            }
            catch (Exception ex)
            {

                throw;
            }
            finally {
                doc.Close();
            }

        }

       
        
    }
}