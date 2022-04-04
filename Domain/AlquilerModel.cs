using DataAccess;
using DataAccess.Data;
using DataAccess.Documents;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
   public class AlquilerModel
    {
        MetodosCommon metodos = new MetodosCommon();
        AlquilerDao alquilerDao = new AlquilerDao();
        Texto texto = new Texto();

        int idAlquiler;
        DateTime fechaIni;
        int fianza;
        decimal importe;
        string observaciones;
        int idLoc;
        int idCl;
        bool modelo;

        public AlquilerModel(int idAlquiler, DateTime fechaIni, int fianza, decimal importe, string observaciones, int idLoc,  bool modelo)
        {
            this.idAlquiler = idAlquiler;
            this.fechaIni = fechaIni;
            this.fianza = fianza;
            this.importe = importe;
            this.observaciones = observaciones;
            this.idLoc = idLoc;
            this.modelo = modelo;
        }

        public AlquilerModel()
        {
        }

     
        public AlquilerModel(DateTime fechaIni, int fianza, decimal importe, string observaciones, int idLoc, int idCl, bool modelo)
        {
            this.fechaIni = fechaIni;
            this.fianza = fianza;
            this.importe = importe;
            this.observaciones = observaciones;
            this.idLoc = idLoc;
            this.idCl = idCl;
            this.modelo = modelo;
        }

        public string NuevoAlquiler()
        {
            try
            {
                if (!metodos.Existe("Select * from Alquiler where idLoc=" + idLoc + "and idCl=" + idCl + ";"))
                {
                    if(!metodos.Existe("Select* from Clientes where activo=0 and idCliente=" + idCl + ";"))
                    {
                      alquilerDao.NuevoAlquiler(fechaIni, fianza, importe, observaciones, idLoc, idCl, modelo);
                       return "Alquiler Creado";
                    }
                    else
                    {
                        return "El cliente no está activo";
                    }
                   
                }
                else
                {
                    return "Ya existe";
                }

            }
            catch(Exception ex)
            {
                return "No se ha podido guardar";
            }
        }


        public string EditarAlquiler()
        {
            try
            {  
                alquilerDao.EditarAlquiler(fechaIni, fianza, importe, observaciones, idLoc, idCl, idAlquiler, modelo);
                return "Alquiler modificado";
            }
            catch(Exception ex)
            {
                return "no se ha podido modificar";
            }
        }



        public string EliminarAlquiler(int id)
        {
            try
            {
                if (!metodos.ObtenerBooleano("select * from Clientes where idCliente=" + idCl + ";",4))
                {
                    alquilerDao.EliminarAlquiler(id);
                    return "Alquiler Eliminado";
                }
                else { return "No es posible, Tiene vinculaciones activas"; }
            }
            catch (Exception ex)
            {
                return "no se ha podido eliminar";
            }
        }


        public DataTable CargarTablaAlquiler(int id)
        {
            return metodos.CargarGridoCmb("Select * from Alquiler where idCl=" + id + ";");
        }



       public DataTable CargarComboLocal(string lug)
        {
            return metodos.CargarGridoCmb("Select idLocal,numero, nombreLugar  from Lugares, Locales where not exists (select * from Alquiler where idLoc=idLocal and not exists(select*from Clientes where idCl=idCliente and activo=0)) and idLug=idLugar and nombreLugar='" + lug+ "';");
        }


        public DataTable CargarComboLugar()
        {
            return metodos.CargarGridoCmb("Select idLugar, nombreLugar from Lugares");
        }

        public string GetLugar(int id)
        {
            try
            {
                return metodos.Obtenerdato("select nombreLugar from Lugares, Locales where idLug=idLugar and idLocal=" + id + ";",0);
            }catch(Exception ex)
            {
                return "error" + ex;
            }
        }


        public DateTime FechaContrato(int idlo, int idc)
        {
            return metodos.GetFecha("select fechaIni from Alquiler where idLoc=" + idlo + "and idCl=" + idc + ";", 0);
        }

        public int GetNumero(int id)
        {         
                return metodos.ObtenerInt("select * from Locales where idLocal=" + id + ";", 1);
        }



        public string NuevoContrato( string nombre, string direccion, string dni, string lugar, int idlocal, int telefono, string correo, string ruta)
        {
            
            try
            {
                if (System.IO.File.Exists(ruta))
                {
                    int numero = metodos.ObtenerInt("select * from Locales where idLocal=" + idlocal + ";", 1);
                    decimal acumulado= metodos.ObtenerNumero("select * from Locales where idLocal=" + idlocal + ";", 2);

                    texto.GenerarWordContrato(nombre, direccion, dni, lugar, numero, fechaIni, fianza, importe, telefono, correo, acumulado,ruta);
                    return "contrato creado";
                }
                else
                {
                    return "no se ha encontrado el archivo en el escritorio";
                }
            }
            catch (Exception ex)
            {
                return "No se puede generar contrato";
            }
        }


        public bool isFileOpen(string ruta)
        {
            try
            {
                System.IO.FileStream fs = System.IO.File.OpenWrite(ruta);
                fs.Close();
            }
            catch (IOException) { return true; }

            return false;
        }

    }
}
