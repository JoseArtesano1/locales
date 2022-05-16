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
        Excels excels = new Excels();

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

        public AlquilerModel(int idAlquiler, DateTime fechaIni, int fianza, decimal importe, string observaciones, bool modelo)
        {
            this.idAlquiler = idAlquiler;
            this.fechaIni = fechaIni;
            this.fianza = fianza;
            this.importe = importe;
            this.observaciones = observaciones;
            this.modelo = modelo;
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


        public string EditarAlquiler(int opcion, int idlocal, int idcliente)
        {
            try
            { 
                if(!metodos.Existe("select * from  Electricidad where idLoca=" + idlocal+ "and idCli=" + idcliente+" and fechaInicio <='" + fechaIni + "';"))
                {
                    if (opcion == 1)
                    {
                        alquilerDao.EditarAlquiler(fechaIni, fianza, importe, observaciones, idLoc, idAlquiler, modelo, 1);
                        return "Alquiler modificado";
                    }
                    else
                    {
                        alquilerDao.EditarAlquiler(fechaIni, fianza, importe, observaciones, 0, idAlquiler, modelo, 2);
                        return "Alquiler modificado, Salvo el local";
                    }

                }
                else
                {
                    return "la fecha de Inicio no puede ser mayor que las fechas de electricidad";
                }
               
                
            }
            catch(Exception ex)
            {
                return "no se ha podido modificar";
            }
        }



        public string EliminarAlquiler(int id, int idc)
        {
            try
            {
                if (!metodos.Existe("select * from Clientes where activo= 1 and idCliente=" + idc + ";"))
                {
                    alquilerDao.EliminarAlquiler(id);
                    return "Alquiler Eliminado";
                }
                else { return "No es posible, Tiene vinculaciones activas, Desactivar cliente"; }
            }
            catch (Exception ex)
            {
                return "no se ha podido eliminar" + ex;
            }
        }


        public DataTable CargarTablaAlquiler(int id)
        {
            return metodos.CargarGridoCmb("Select idAlquiler as id, fechaIni as Inicio, fianza, importe, observaciones as Notas, idLoc as idLocal, modelo,numero as Trastero from Alquiler, Locales where idLoc=idLocal and idCl=" + id + ";");
        }



       public DataTable CargarComboLocal(string lug)
        {
            return metodos.CargarGridoCmb("Select idLocal,numero from Lugares, Locales where not exists (select * from Alquiler where idLoc=idLocal and not exists(select*from Clientes where idCl=idCliente and activo=0)) and idLug=idLugar and nombreLugar='" + lug+ "';");
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
            } catch(Exception ex)
            {
                return "error" + ex;
            }
        }


        public DateTime FechaContrato(int idlo, int idc)
        {
            return metodos.GetFecha("select fechaIni from Alquiler where idLoc=" + idlo + " and idCl=" + idc + ";", 0);
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



        public DataTable ConsultaCliente(int lugar)
        {
            return metodos.CargarGridoCmb("select idCliente,  nombre, dni, direccion, activo, tipo from Clientes, Alquiler, Locales  where idCliente=idCl and idLoc=idLocal and activo=1 and idLug=" + lugar + ";");
        }


        public DataTable ConsultaCartera(int lugar)
        {
            return metodos.CargarGridoCmb("select idCliente,  nombre, dni, direccion, observaciones, importe from Clientes, Alquiler, Locales  where idCliente=idCl and idLoc=idLocal and activo=1 and tipo=1 and modelo=1 and idLug=" + lugar + ";");
        }

        public DataTable ConsultaInactivo(int lugar)
        {
            return metodos.CargarGridoCmb("select idLocal, numero as Numero_Trastero from Locales where not exists(select*from Alquiler where idLoc = idLocal or exists(select*from Clientes where idCl=idCliente and activo=0)) and idLug =" + lugar + ";");

        }

        public DataTable ConsultaAlquiler(int cliente, int lugar)
        {
            return metodos.CargarGridoCmb("select idAlquiler,  fechaIni, fianza, importe, observaciones, modelo from  Alquiler, Locales  where idLoc=idLocal and idLug=" + lugar + "and  idCl=" + cliente + ";");
        }


        public DataTable ConsultaElectricidad(int cliente, int lugar)
        {
           return metodos.CargarGridoCmb("select idElectricidad as id,  numero as Trastero,  fechaInicio as Inicio, fechaFin as Fin, DATEDIFF(day, fechaInicio,fechaFin)/30 as Meses, (consumo-acumulado) as Gasto,  consumo, importe as Importe from Electricidad,Locales,Clientes where idLocal=idLoca  and idCliente=idCli and idLug=" + lugar + "and idCli=" + cliente + ";");
        }

        public DataTable ConsultaTelefono(int id)
        {
            return metodos.CargarGridoCmb("select * from Telefono  where  idClient=" + id + ";");
        }


        public string NuevasFacturas(string carpeta, string archivo, string lugar, DateTime fecha, int numero)
        {
            try
            {
                if (System.IO.File.Exists(carpeta+archivo))
                {
                    excels.GenerarExcel(carpeta, archivo, lugar, fecha, numero);
                    return "facturas generadas";
                }
                else
                {
                    return "no se ha encontrado el archivo en el escritorio";
                }

            } catch(Exception ex)
            {
                return "No se pueden crear las facturas" +ex;
            }
           
        }


        public string NuevaFactura(string carpeta, string archivo, string nombre, string dir, string dni, string lugar, double importe, DateTime fecha, int numero, int nlocal)
        {
            try
            {
                if (System.IO.File.Exists(carpeta + archivo))
                {
                    excels.GenerarExcelIndividual(carpeta, archivo, nombre,dir,dni, lugar, importe, fecha, numero, nlocal);
                    return "factura generada";
                }
                else
                {
                    return "no se ha encontrado el archivo en el escritorio";
                }

            }
            catch (Exception ex)
            {
                return "No se pueden crear la factura" + ex;
            }

        }



    }


   

}
