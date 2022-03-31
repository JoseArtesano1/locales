using DataAccess;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain
{
  public  class TelefonoModel
    {
        MetodosCommon metodos = new MetodosCommon();
        ClienteDao clienteDao = new ClienteDao();

        int idTelefono;
        int movil;
        int idClient;
        string email;

        public TelefonoModel(int idTelefono, int movil, int idClient, string email)
        {
            this.idTelefono = idTelefono;
            this.movil = movil;
            this.idClient = idClient;
            this.email = email;
        }

        public TelefonoModel()
        {
        }

        public TelefonoModel(int movil, int idClient, string email)
        {
            this.movil = movil;
            this.idClient = idClient;
            this.email = email;
        }

        public string NuevoTelefono()
        {
            try
            {
                if (!metodos.Existe("select * from Telefono where movil=" + movil + ";"))
                {
                    if(!metodos.Existe("select * from Telefono where email='" + email + "';"))
                    {                      
                            clienteDao.NuevoTelefonoEmail(movil, idClient, email );
                            return "Creado";
                    }
                    else
                    {
                        return "ya existe el correo";
                    }
              
                }
                else
                {
                    return "Ya Existe el teléfono";
                }

            }catch(Exception ex)
            {
                return "No se ha podido Guardar" + ex;
            }
        }


        public string EditarTelefono()
        {
            try
            {
                switch (ControlMovilCorreo())
                {
                    case 0:
                        
                        break;

                    case 1:

                        clienteDao.EditarTelefono(movil, idTelefono, email, 1);
                        // return "Teléfono y correo modificado";
                       
                        break;

                    case 2:
                        clienteDao.EditarTelefono(movil, idTelefono, email, 2);
                        break;

                    case 3:
                        clienteDao.EditarTelefono(movil, idTelefono, email, 3);
                        break;

                   
                }
                return "Modificado";

            }catch(Exception ex)
            {
                return "No se puede modificar";
            }
        }


        public string EliminarTelefono(int id)
        {
            try
            {                
                    clienteDao.EliminarTelefono(id);
                    return "Teléfono y correo Eliminado";
               
            }
            catch (Exception ex)
            {
                return "no se ha podido eliminar";
            }
        }



        public DataTable CargarTablaTef(int id)
        {
            return metodos.CargarGridoCmb("Select * from Telefono where idClient=" +id+";");
        }


        public int ControlMovilCorreo()
        {
            if (!metodos.Existe("select* from Telefono where movil=" + movil + " and email='" + email + "';"))
            {                
                return 1;
            }
            if(!metodos.Existe("select* from Telefono where movil=" + movil + ";"))
            {
                return 2;
            }

            if (!metodos.Existe("select* from Telefono where email='" + email + "';"))
            {
                return 3;
            }

            return 0;
        }



        public Boolean emailOk(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }



    }
}
