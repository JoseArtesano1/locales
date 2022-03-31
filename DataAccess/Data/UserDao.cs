using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using Common.Cache;

namespace DataAccess
{
    public class UserDao : ConnectionToSql
    {
        
        

        public void EditarUsuario(int id, string nombre,  string roles, string nombUser)
        {
            using(var conexion = GetConnection())
            {
                conexion.Open();
                using(var comando= new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Update Usuarios set nombre=@nom, rol=@rol, nombreUsuario=@user where idUsuario=@id";
                    comando.Parameters.AddWithValue("@nom", nombre);
                    comando.Parameters.AddWithValue("@rol", roles);
                    comando.Parameters.AddWithValue("@id", id);
                    comando.Parameters.AddWithValue("@user", nombUser);
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();

                }


            }

        }

        public void EditarUsuarioNormal(string sql)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = sql;
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();

                }


            }

        }

        public void NuevoUsuario(string nombre, string pass, string rol, string usuario)
        {
            string valor = GetSHA256(pass);
            using (var conexion = GetConnection())
            {
                conexion.Open();
                using(var comando= new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Insert into Usuarios (nombre,passUser,rol,nombreUsuario) values(@nom,@pass,@rl,@nomUs)";
                    comando.Parameters.AddWithValue("@nom", nombre);
                    comando.Parameters.AddWithValue("@pass", valor);
                    comando.Parameters.AddWithValue("@rl", rol);
                    comando.Parameters.AddWithValue("@nomUs", usuario);
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }

        }


        public bool Login(string user, string pass)
        {
           string valor= GetSHA256(pass);
            using (var conexion = GetConnection())
            {
                conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "select *from Usuarios where nombreUsuario=@nom and passUser=@pas";
                    comando.Parameters.AddWithValue("@nom", user);
                    comando.Parameters.AddWithValue("@pas", valor);
                    comando.CommandType = CommandType.Text;
                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            UserLoginCache.idUsuario = reader.GetInt32(0);
                            UserLoginCache.nombreUsuario = reader.GetString(4);
                            UserLoginCache.passUser = pass;
                            UserLoginCache.rol = reader.GetString(3);
                            UserLoginCache.nombre = reader.GetString(1);
                        }


                        return true;
                    }
                    else { return false; }
                }
            }
        }

        public  string GetSHA256(string pas){

            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder stringBuilder = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(pas));
            for(int i = 0; i < stream.Length; i++)
            {
                stringBuilder.AppendFormat("{0:x2}", stream[i]);
            
            }
            return stringBuilder.ToString();
            }


        public void EliminarUsuario(int idUser)
        {
            using(var conexion=GetConnection())
            {
                conexion.Open();
                using(var comando=new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "delete from Usuarios where idUsuario= " + idUser + ";";
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }


        public void InsertStartData()
        {
            string valor = GetSHA256("123ja$");
            using (SqlConnection conexion = GetConnection())
            {
                conexion.Open();
                using (SqlCommand comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Insert into Usuarios (nombre,passUser,rol,nombreUsuario) values(@nom, @pass,@rl, @nomUs)";
                    comando.Parameters.AddWithValue("@nom", "Jose Antonio");
                    comando.Parameters.AddWithValue("@pass", valor);
                    comando.Parameters.AddWithValue("@rl", "ADMINISTRADOR");
                    comando.Parameters.AddWithValue("@nomUs", "Jose");
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void datosUsuario( string pass, string sql)
        {          
            using (var conexion = GetConnection())
            {
                conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = sql;
                    comando.CommandType = CommandType.Text;
                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {                           
                            UserLoginCache.nombreUsuario = reader.GetString(4);
                            UserLoginCache.passUser = pass;
                             UserLoginCache.nombre = reader.GetString(1);
                        }
                       
                    }
                    
                }
            }
        }





    }
}
