using DataAccess;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
  public  class ClienteModel
    {
        MetodosCommon metodos = new MetodosCommon();
        ClienteDao clienteDao = new ClienteDao();

        int idCliente;
        string nombre;
        string dni;
        string direccion;
        bool activo;
        bool tipo;
        List<int> telefonos;
        List<string> correos;

        public ClienteModel(int idCliente, string nombre, string dni, string direccion, bool activo, bool tipo)
        {
            this.idCliente = idCliente;
            this.nombre = nombre;
            this.dni = dni;
            this.direccion = direccion;
            this.activo = activo;
            this.tipo = tipo;
        }

        public ClienteModel()
        {
        }

        public ClienteModel(string nombre, string dni, string direccion, bool activo, bool tipo)
        {
            this.nombre = nombre;
            this.dni = dni;
            this.direccion = direccion;
            this.activo = activo;
            this.tipo = tipo;
        }

        public string NuevoCliente()
        {
            try
            {
                if (!metodos.Existe("Select * from Clientes where dni='" + dni + "';"))
                {
                    clienteDao.NuevoCliente(nombre, dni, direccion, activo,tipo);
                    return "Guardado";
                }
                else
                {
                    return "Ya Existe";
                }

            }
            catch (Exception ex)
            {
                return "No se ha podido Guardar";
            }

        }


        public string EditarCliente()
        {
            try
            {               
               if(!metodos.Existe("Select * from Clientes where dni='" + dni + "';"))
                {
                    clienteDao.ModificarCliente(nombre, dni, direccion, activo, tipo, idCliente,true);
                    return "Modificado";
                }
                else
                {
                    clienteDao.ModificarCliente(nombre, dni, direccion, activo, tipo, idCliente, false);
                    return "Modificado, menos el dni/nie/cif";
                }
               
               
            }
            catch (Exception ex)
            {
                return "No se ha modificado";
            }
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


       

        public bool ComprobarTelefono(string valor)
        {
            if (metodos.esNumero(valor))
            {
                return true;
            }
            return false;
        }


        public DataTable CargarCliente()
        {
            return metodos.CargarGridoCmb("Select * from Clientes");
        }



        public string EliminarClientes(int idC)
        {
            try
            { if(!metodos.Existe("select * from Telefono where idClient=" + idC + ";") && !metodos.Existe("select * from Electricidad where idCli=" + idC + ";") && !metodos.Existe("select * from Alquiler where idCl=" + idC + ";"))
                {
                    clienteDao.EliminarCliente(idC);
                    return "Cliente eliminado";
                }
                else
                {
                    return "El cliente tiene vinculaciones, electricidad, alquiler y contactos";
                }

            }catch(Exception ex)
            {
                return "no puede eliminarse" + ex;
            }
        }


        public List<string> CargaConsulta()
        {
            return metodos.CreaLista<string>("CONTACTO", "ALQUILER", "ELECTRICIDAD");
        }


        public List<string> CargaConsulta2()
        {
            return metodos.CreaLista<string>( "VACIOS", "CARTERA", "ELECTRICIDAD", "OBSERVACIONES");
        }





    }
}
