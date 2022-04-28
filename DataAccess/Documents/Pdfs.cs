using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Documents
{
  public  class Pdfs
    {
        MetodosCommon metodos = new MetodosCommon();

        public void GenerarPdf( int lugar)
        {
            string carpeta= Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/DocumentosLocales/";
            if (!Directory.Exists(carpeta)) { Directory.CreateDirectory(carpeta); }
         
            PdfWriter writer = new PdfWriter(carpeta + "\\Informe.pdf");
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            string dato = metodos.Obtenerdato("Select nombreLugar from Lugares where idLugar=" + lugar + ";", 0);

            Paragraph header = new Paragraph("LISTADO ELECTRICIDAD " + dato)  //titulo
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(20);

            Paragraph subheader = new Paragraph(DateTime.Now.ToString("dd-MM-yyyy"))
           .SetTextAlignment(TextAlignment.CENTER)
           .SetFontSize(12);

            PdfFont fontColumnas = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            PdfFont fontContenido = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            string[] columnas = { "Trastero", "Nombre", "Teléfono", "Inicio", "Fin", "Meses", "Gasto", "Importe" };
            float[] dimension = { 2, 5, 4, 4, 4, 2, 3, 3 };

            Table tabla = new Table(UnitValue.CreatePercentArray(dimension));
            tabla.SetWidth(UnitValue.CreatePercentValue(100));
            Style style = new Style().SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);

            foreach (string columna in columnas)
            {
                tabla.AddHeaderCell(new Cell().Add(new Paragraph(columna).AddStyle(style).SetFont(fontColumnas)));
            }

            var ContenidoTabla = metodos.CargarGridoCmb("select numero as Trastero, nombre as Cliente, movil as Movil, fechaInicio as Inicio, fechaFin as Fin, DATEDIFF(day, fechaInicio, fechaFin)/30 as Plazo, (consumo-acumulado) as Gasto, importe as Importe, activo from Electricidad,Locales,Clientes, Telefono where idLocal=idLoca  and idCliente=idCli and idCliente=idClient and estado=0 and idLug=" + lugar + ";");

          

            for (int i = 0; i < ContenidoTabla.Rows.Count; i++)
            {               
                tabla.AddCell(new Cell().Add(new Paragraph(ContenidoTabla.Rows[i]["Trastero"].ToString()).SetFont(fontContenido))).SetTextAlignment(TextAlignment.CENTER);
                tabla.AddCell(new Cell().Add(new Paragraph(ContenidoTabla.Rows[i]["Cliente"].ToString()).SetFont(fontContenido)));
                tabla.AddCell(new Cell().Add(new Paragraph(ContenidoTabla.Rows[i]["Movil"].ToString()).SetFont(fontContenido)));
                tabla.AddCell(new Cell().Add(new Paragraph(Convert.ToDateTime(ContenidoTabla.Rows[i]["Inicio"]).ToString("dd/MM/yyyy")).SetFont(fontContenido)));
                tabla.AddCell(new Cell().Add(new Paragraph(Convert.ToDateTime(ContenidoTabla.Rows[i]["Fin"]).ToString("dd/MM/yyyy")).SetFont(fontContenido)));
                tabla.AddCell(new Cell().Add(new Paragraph(ContenidoTabla.Rows[i]["Plazo"].ToString()).SetFont(fontContenido)));
                tabla.AddCell(new Cell().Add(new Paragraph(ContenidoTabla.Rows[i]["Gasto"].ToString()).SetFont(fontContenido)));
                tabla.AddCell(new Cell().Add(new Paragraph(ContenidoTabla.Rows[i]["Importe"].ToString()).SetFont(fontContenido)));
               
            }

                document.Add(header);
                document.Add(subheader);
                document.Add(tabla);

            document.Close();
        }
    }
}
