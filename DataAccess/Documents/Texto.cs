using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using objWord = Microsoft.Office.Interop.Word;

namespace DataAccess.Documents
{
   public class Texto 
    {

        public void GenerarWordContrato(string elnombre, string ladire, string eldni, string elLug, int minum, DateTime fech, 
            int mifianza, decimal mivalor, int miTelef, string micorreo, decimal miacumul, string ruta)
        {            

              object ObjMiss = System.Reflection.Missing.Value;

               // Trabajador trabajador = moduloInicio.TrabajadoresEmpresa().Where(x => x.Nombre == valor).FirstOrDefault();
                //documento referencias en el word las pasamos a objetos

                objWord.Application objAplicacion = new objWord.Application();

                object miruta = ruta;
                object nombre = "cliente"; object direccion = "direccion"; object dni = "dni";
                object lugar = "lugar"; object num = "numero";
                object fecha1 = "fech1"; object fecha2 = "fech2"; object fecha3 = "fech3"; 
                object fecha4 = "fech4";  object fecha5 = "fech5";
                object alquiler = "valor"; object fianza = "fianz"; object acumulado = "electrico";
                object movil = "telefono"; object correo = "email";
               
                
                objWord.Document objDocumento = objAplicacion.Documents.Open(miruta, ref ObjMiss);
                // los objetos los pasamos a objWord
                objWord.Range nom = objDocumento.Bookmarks.get_Item(ref nombre).Range;
                nom.Text = elnombre;
                objWord.Range di = objDocumento.Bookmarks.get_Item(ref direccion).Range;
                di.Text = ladire;
                objWord.Range nie = objDocumento.Bookmarks.get_Item(ref dni).Range;
                nie.Text = eldni;
                objWord.Range lug = objDocumento.Bookmarks.get_Item(ref lugar).Range;
                lug.Text = elLug;
                objWord.Range nume = objDocumento.Bookmarks.get_Item(ref num).Range;
                nume.Text = minum.ToString();
                objWord.Range f1 = objDocumento.Bookmarks.get_Item(ref fecha1).Range;
                f1.Text = fech.ToShortDateString();
                objWord.Range f2 = objDocumento.Bookmarks.get_Item(ref fecha2).Range;
                f2.Text = fech.ToShortDateString(); 
                objWord.Range f3 = objDocumento.Bookmarks.get_Item(ref fecha3).Range;
                f3.Text = fech.ToShortDateString();
                objWord.Range f4 = objDocumento.Bookmarks.get_Item(ref fecha4).Range;
                f4.Text = fech.ToShortDateString();
                objWord.Range f5 = objDocumento.Bookmarks.get_Item(ref fecha5).Range;
                f5.Text = fech.ToShortDateString();
                objWord.Range al = objDocumento.Bookmarks.get_Item(ref alquiler).Range;
                al.Text = mivalor.ToString();
                objWord.Range fi = objDocumento.Bookmarks.get_Item(ref fianza).Range;
                fi.Text = mifianza.ToString();
                objWord.Range acu = objDocumento.Bookmarks.get_Item(ref acumulado).Range;
                acu.Text = miacumul.ToString();
                objWord.Range tel = objDocumento.Bookmarks.get_Item(ref movil).Range;
                tel.Text = miTelef.ToString();
                objWord.Range corr = objDocumento.Bookmarks.get_Item(ref correo).Range;
                corr.Text = micorreo;
                //rango
                object rango1 = nom; object rango5 = nume; object rango6 = f1; object rango7 = f2; object rango8 = f3;
                object rango2 = di; object rango9 = f4; object rango10 = f5; object rango11 = al; object rango12 = fi;
                object rango3 = nie; object rango13 = acu; object rango14 = tel; object rango15 = corr;
                object rango4 = lug; 
                objDocumento.Bookmarks.Add("cliente", ref rango1); objDocumento.Bookmarks.Add("direccion", ref rango2);
                objDocumento.Bookmarks.Add("dni", ref rango3); objDocumento.Bookmarks.Add("lugar", ref rango4);
                objDocumento.Bookmarks.Add("numero", ref rango5);
                objDocumento.Bookmarks.Add("fech1", ref rango6); objDocumento.Bookmarks.Add("fech2", ref rango7);
                objDocumento.Bookmarks.Add("fech3", ref rango8); objDocumento.Bookmarks.Add("fech4", ref rango9);
                objDocumento.Bookmarks.Add("fech5", ref rango10); objDocumento.Bookmarks.Add("valor", ref rango11);
                objDocumento.Bookmarks.Add("fianz", ref rango12); objDocumento.Bookmarks.Add("electrico", ref rango13);
                objDocumento.Bookmarks.Add("telefono", ref rango14); objDocumento.Bookmarks.Add("email", ref rango15);
                
                objDocumento.Close();
                objAplicacion.Quit();


        }


       


    }
}
