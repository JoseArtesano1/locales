using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Documents
{
   public class Excels
    {

        MetodosCommon metodos = new MetodosCommon();

        public void GenerarExcel(string carpeta, string archivo, string lugar, DateTime fecha, int numero)
        {
            if (!Directory.Exists(carpeta)) { Directory.CreateDirectory(carpeta); }

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook sheet = excel.Workbooks.Open(carpeta + archivo);
            Microsoft.Office.Interop.Excel.Worksheet x = excel.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;

            Microsoft.Office.Interop.Excel.Range userRange = x.UsedRange;
            int countRecords = userRange.Rows.Count;
            int add = countRecords + 1;

            var contenidoFactura = metodos.CargarGridoCmb("select nombre, dni, direccion, importe, numero from Clientes, Alquiler, Locales, Lugares where idCliente=idCl and idLoc=idLocal and activo=1 and tipo=0 and modelo=0 and idLug= idLugar and nombreLugar='" +lugar+"';");
            int fila = 3;
            int fila1 = 4, fila2 = 7, fila3 = 20, fila4=13;
            double iva=1.21;
            
            for(int i = 0; i < contenidoFactura.Rows.Count; i++)
            {
                x.Cells[fila, 7] = contenidoFactura.Rows[i]["nombre"].ToString();
                x.Cells[fila1, 7] = contenidoFactura.Rows[i]["direccion"].ToString();
                x.Cells[fila2, 7] = contenidoFactura.Rows[i]["dni"].ToString();
                x.Cells[fila4, 6] = fecha;
                x.Cells[fila3,3]="ALQUILER TRASTERO Nº: " + int.Parse( contenidoFactura.Rows[i]["numero"].ToString()) + " EN " + lugar;
                x.Cells[fila4,9]=numero+"/"+fecha.ToString("yy");
                x.Cells[fila3, 10] = double.Parse(contenidoFactura.Rows[i]["importe"].ToString())/iva;

                fila += 64; fila1 += 64; numero++;
                fila2 += 64; fila3 +=64; fila4 += 64; 
            }

           // sheet.SaveAs(carpeta + "\\FRA1.xlsx");
            sheet.Close(true, Type.Missing, Type.Missing);
            excel.Quit();
          

        }



        public void GenerarExcelIndividual(string carpeta, string archivo, string nombre, string dire, string dni, string lugar, double importe,
            DateTime fecha, int numero, int nlocal)
        {
            if (!Directory.Exists(carpeta)) { Directory.CreateDirectory(carpeta); }

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook sheet = excel.Workbooks.Open(carpeta + archivo);
            Microsoft.Office.Interop.Excel.Worksheet x = excel.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;

            Microsoft.Office.Interop.Excel.Range userRange = x.UsedRange;
            int countRecords = userRange.Rows.Count;
            int add = countRecords + 1;
          
            double iva = 1.21;

            x.Cells[3, 7] = nombre;
            x.Cells[4, 7] = dire;
            x.Cells[7, 7] = dni;
            x.Cells[20, 3] = "ALQUILER TRASTERO Nº: " + nlocal + " EN " + lugar;
            x.Cells[13, 6] = fecha;
            x.Cells[13, 9] = numero + "/" + fecha.ToString("yy"); ;
            x.Cells[20, 10] = importe/iva;
          
            // sheet.SaveAs(carpeta + "\\FRA1.xlsx");
            sheet.Close(true, Type.Missing, Type.Missing);
            excel.Quit();

        }



    }
}
