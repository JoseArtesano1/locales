using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Cache;
using DataAccess;
using DataAccess.Data;

namespace Domain
{
    

    public class LocalModel
    {
        MetodosCommon metodos = new MetodosCommon();
        LocalDao localDao = new LocalDao();

        //propiedades
        int idLocal;
        int numero;
        decimal acumulado;
        int idLug;
       

        public LocalModel(int idLocal,  int numero, decimal acumulado, int idLug) 
        {
            this.idLocal = idLocal;
            this.numero = numero;
            this.acumulado = acumulado;
            this.idLug = idLug;
           
        }

        public LocalModel( int numero, decimal acumulado, int idLug)
        {  
            this.numero = numero;
            this.acumulado = acumulado;
            this.idLug = idLug;
        }

      

        public LocalModel()
        {
        }

        public LocalModel(int idLocal, decimal acumulado)
        {
            this.idLocal = idLocal;
            this.acumulado = acumulado;
        }

        public string NuevoLocal()
        {
            try
            {
                if(!metodos.Existe("Select * from Locales where numero= " + numero + "and idLug=" + idLug + ";"))
                {
                   localDao.NuevoLocal( numero, acumulado, idLug);
                    return "Guardado";
                }
                else
                {
                    return "Ya Existe";
                }

            }catch(Exception ex)
            {
                return "No se ha podido Guardar";
            }
        }


        public string EditarLocal()
        {
            try
            {
                if (!metodos.Existe("Select * from Locales where numero= " + numero + "and idLug=" + idLug + ";"))
                {
                    localDao.EditarLocal(idLocal,idLug, numero, acumulado,true);
                    return "Modificado";
                }
                else
                {
                    localDao.EditarLocal(idLocal, idLug, numero, acumulado, false);
                    return "Solo se puede modificar El acumulado";
                }

            }
            catch (Exception ex)
            {
                return "No se ha podido Modificar";
            }
        }


        public string ActualizarAcumulado()
        {
            try
            {
                if (metodos.Existe("select * from Electricidad, Locales where idLoca=idLocal and consumo>acumulado and estado=1 and importe>0 and idLoca=" + idLocal + ";"))
                {
                    localDao.EditarLocal(idLocal, idLug, numero, acumulado, false);
                    return "Acumulado Actualizado";
                }
                else
                {
                    return "Debe cambiar el estado o consumo es igual al acumulado";
                }
               
            }
            catch(Exception ex)
            {
                return "Sin actualizar acumulado";
            }
        }

        public string EliminarLocal(int id)
        {
            try
            {
                if (!metodos.Existe("Select*from Electricidad where idLoca=" +id+";")|| !metodos.Existe("Select*from Alquiler where idLoc=" + id + ";"))
                {
                    localDao.EliminarLocal(id);
                    return "Local Eliminado";
                }
                else { return "No es posible, Tiene vinculaciones activas"; }
            }
            catch(Exception ex)
            {
                return "no se ha podido eliminar";
            }
        }


        public DataTable CargaLocales()
        {
            return metodos.CargarGridoCmb("select * from Locales, Lugares where idLugar=idLug");
        }


        public bool ComprobarString(string valor)
        {
            if (metodos.Esnum(valor))
            {
                return true;
            }
            return false;
        }


        public bool ComprobarDecimal(string valor)
        {
            if (metodos.isnumericDecimal(valor))
            {
                return true;
            }
            return false;
        }


        public decimal GetConsumo(int idl, int idc)
        {
            return metodos.ObtenerNumero("select consumo from Electricidad where estado=1 and importe>0 and idLoca=" + idl + " and idCli=" + idc + ";", 0);
        }



    }
}
