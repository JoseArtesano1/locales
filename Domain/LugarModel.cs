using DataAccess;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
  public  class LugarModel
    {
        MetodosCommon metodos = new MetodosCommon();
        LugarDao lugarDao = new LugarDao();

        int idLugar;
        string nombreLugar;

        public LugarModel(int idLugar, string nombreLugar)
        {
            this.idLugar = idLugar;
            this.nombreLugar = nombreLugar;
        }

        public LugarModel(string nombreLugar)
        {
            this.nombreLugar = nombreLugar;
        }

        public LugarModel()
        {
        }


        public string NuevoLugarlocal()
        {
            try
            {
                if (!metodos.Existe("select * from Lugares where nombreLugar='" + nombreLugar + "';"))
                {
                    lugarDao.NuevoLugar(nombreLugar);
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



        public string EditarLugarlocal()
        {
            try
            {
                if (!metodos.Existe("select * from Lugares where nombreLugar='" + nombreLugar + "';"))
                {
                    lugarDao.EditarLugar(nombreLugar,idLugar);
                    return "Modificado";
                }
                else
                {
                    return "Ya Existe";
                }
            }
            catch (Exception ex)
            {
                return "No se ha podido Modificar";
            }

        }


        public string EliminarLugar(int id)
        {
            try
            {
                if (!metodos.Existe("Select*from Locales where idLug=" + id + ";") || !metodos.Existe("Select*from PotEnerg where idLu=" + id + ";"))
                {
                    lugarDao.EliminarLugar(id);
                    return "Lugar Eliminado";
                }
                else { return "No es posible, Tiene vinculaciones activas"; }
            }
            catch (Exception ex)
            {
                return "no se ha podido eliminar";
            }
        }



        public bool ExisteLugar()
        {
            return metodos.Existe("select * from Lugares");
        }




    }
}
