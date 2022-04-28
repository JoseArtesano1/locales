using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using System.Data.SqlClient;
using Common.Cache;
using System.Data;
using System.Windows;

namespace Domain
{
    public class UserModel
    {
        UserDao userDao = new UserDao();
        MetodosCommon metodos = new MetodosCommon();

        //Propiedades
        int idUsuario;
        string nombre;
        string passUser;
        string rol;
        string nombreUsuario;


        //constructores
        public UserModel(int idUsuario, string nombre, string passUser, string rol, string nombreUsuario)
        {
            this.idUsuario = idUsuario;
            this.nombre = nombre;
            this.passUser = passUser;
            this.rol = rol;
            this.nombreUsuario = nombreUsuario;
        }

        public UserModel( string nombre, string passUser, string rol, string nombreUsuario)
        {           
            this.nombre = nombre;
            this.passUser = passUser;
            this.rol = rol;
            this.nombreUsuario = nombreUsuario;
        }

        public UserModel()
        {

        }

       


    //metodos
    public bool LoginUser(string user, string pass)
        {
            return userDao.Login(user, pass);
        }


        public string EditaUser( )
        {
            try
            {                    //controlar si contraseña text = confirmar text
                if (ControlRol())
                   {                    
                     userDao.EditarUsuario(idUsuario, nombre, rol, nombreUsuario);
                    
                   }
                    else {
                        if (controlPass(passUser))
                       {                        
                        userDao.EditarUsuarioNormal("Update Usuarios set  nombreUsuario =" + nombreUsuario + "  where idUsuarios =" +  UserLoginCache.idUsuario); 
                       }
                       else
                       {
                        passUser = userDao.GetSHA256 (passUser);
                        userDao.EditarUsuarioNormal("Update Usuarios set passUser =" + passUser + ", nombreUsuario =" + nombreUsuario + "  where idUsuarios =" + UserLoginCache.idUsuario); 
                       }
                    }
             
                return "Modificado correctamente";
            }
            catch (Exception ex)
            {
                return "El usuario no se puede modificar" + ex;
            }

        }


        public string NuevoUser()
        {
            try
            {  
                userDao.NuevoUsuario(nombre, passUser, rol, nombreUsuario);
                return  "Creado correctamente";
            }
            catch(Exception ex)
            {
                return "El usuario ya existe" + ex;
            }

        }


        //public void RecargarUsuario(string pass)
        //{
        //     userDao.datosUsuario(pass,"select * from Usuarios where idUsuario=" + UserLoginCache.idUsuario );
        //}

               


        //public bool OpcionEditarUsuario(int user)
        //{
        //    if (user == UserLoginCache.idUsuario)
        //    {
        //        return true;
        //    }
        //    else { return false; }
        //}


        public bool ControlRol()
        {
            if (UserLoginCache.rol == Cargos.SuperRol)
            {
                return true;
            }
            return false;
        }


        public DataTable Carga()
        {
            return metodos.CargarGridoCmb("select * from Usuarios");
        }

        public bool controlPass( string pword)
        {
            string pass = UserLoginCache.passUser;
            if (pword == pass)
            {
              return  true; // metemos el editar sin pass
            }
            else
            {
              return  false;// metemos el editar con pass
            }
           
        }


        public string EliminarUsuario(int id)
        {
            try
            {
                userDao.EliminarUsuario(id);
                return "eliminado Correctamente";
            }catch(Exception ex)
            {
                return "no se ha podido eliminar" + ex;
            }
        }

        public void IniciarBd()
        {
            if(!metodos.Existe("select*from Usuarios"))
            {
                userDao.InsertStartData(); 
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

        //public  string Encriptar(string pas)
        //{
        //    return userDao.GetSHA256(pas);
        //}
    }
}
