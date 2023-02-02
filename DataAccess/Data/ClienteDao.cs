using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
 public class ClienteDao:ConnectionToSql
    {

        public void NuevoCliente(string nom, string dni, string dir, bool activo, bool tipo)
        {
            using(var conexion= GetConnection())
            {
                conexion.Open();
                using(var comando= new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Insert into Clientes (nombre, dni, direccion, activo,tipo) values(@nombre,@dni,@dir,@act,@tp)";
                    comando.Parameters.AddWithValue("@nombre",nom.ToUpper());
                    comando.Parameters.AddWithValue("@dni", dni.ToUpper());
                    comando.Parameters.AddWithValue("@dir", dir.ToUpper());
                    comando.Parameters.AddWithValue("@act", activo);
                    comando.Parameters.AddWithValue("@tp", tipo);
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }




        public void ModificarCliente(string nombre, string dni ,string dire, bool activo, bool tip, int id, bool opcion)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    if (opcion)
                    {
                        comando.CommandText = "Update Clientes set nombre=@no, dni=@dni, direccion=@dir, activo=@act, tipo=@tp where idCliente=@id";
                        comando.Parameters.AddWithValue("@no", nombre.ToUpper());
                        comando.Parameters.AddWithValue("@dni", dni.ToUpper());
                        comando.Parameters.AddWithValue("@dir", dire.ToUpper());
                        comando.Parameters.AddWithValue("@act", activo);
                        comando.Parameters.AddWithValue("@tp", tip);
                        comando.Parameters.AddWithValue("@id", id);
                    }
                    else
                    {
                        comando.CommandText = "Update Clientes set nombre=@no, direccion=@dir, activo=@act, tipo=@tp where idCliente=@id";
                        comando.Parameters.AddWithValue("@no", nombre.ToUpper());
                        comando.Parameters.AddWithValue("@dir", dire.ToUpper());
                        comando.Parameters.AddWithValue("@act", activo);
                        comando.Parameters.AddWithValue("@tp", tip);
                        comando.Parameters.AddWithValue("@id", id);
                    }
                   
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }


        public void EliminarCliente(int id)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Delete from Clientes where idCliente=@id";
                    comando.Parameters.AddWithValue("@id", id);
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }



        public void NuevoTelefonoEmail(int movil, int id, string email )
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Insert into Telefono (movil, idClient, email) values(@mv,@id,@correo)";
                    comando.Parameters.AddWithValue("@mv", movil);
                    comando.Parameters.AddWithValue("@id", id);
                    comando.Parameters.AddWithValue("@correo", email);
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }


        public void EditarTelefono(int movil,  int idFon, string correo, int opcion)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;

                    switch (opcion)
                    {
                        case 1:
                            comando.CommandText = "Update Telefono set movil=@mv, email=@emails where idTelefono=@idTef";
                            comando.Parameters.AddWithValue("@mv", movil);
                            comando.Parameters.AddWithValue("@idTef", idFon);
                            comando.Parameters.AddWithValue("@emails", correo);
                            break;

                        case 2:
                            comando.CommandText = "Update Telefono set movil=@mv where idTelefono=@idTef";
                            comando.Parameters.AddWithValue("@mv", movil);
                            comando.Parameters.AddWithValue("@idTef", idFon);
                            break;

                        case 3:
                            comando.CommandText = "Update Telefono set  email=@emails where idTelefono=@idTef";
                            comando.Parameters.AddWithValue("@idTef", idFon);
                            comando.Parameters.AddWithValue("@emails", correo);
                            break;
                    }
                   
                   
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }


        public void EliminarTelefono( int idFon)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Delete from Telefono where idTelefono=@idTef";
                    comando.Parameters.AddWithValue("@idTef", idFon);
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }


        public string ClienteMail(string dni, string mensaje, bool opcion)
        {
            string dato = "", dato1 = "";
            using (var conexion = GetConnection())
            {
                conexion.Open();

                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "select nombre, email from Clientes, Telefono where idCliente=idClient and activo=1 and dni=@dni";
                    comando.Parameters.AddWithValue("@dni", dni);
                    comando.CommandType = CommandType.Text;
                    SqlDataReader reader = comando.ExecuteReader();

                    while (reader.Read())
                    {
                        dato = reader.GetString(0);
                        dato1 = reader.GetString(1);
                        if (opcion)
                        {
                            List<String> listEmail = new List<string>();
                            listEmail.Add(dato1);
                            foreach (var item in listEmail)
                            {
                                var mail = new MailServicios.SystemMail();
                                mail.EnvioMail(subject: "TRASTEROS", body: mensaje, destinatarios: new List<string>() { item });
                            }

                        }
                        else
                        {
                            var mail = new MailServicios.SystemMail();
                            mail.EnvioMail(subject: "TRASTEROS", body: mensaje, destinatarios: new List<string>() { dato1 });
                        }

                        return "Correo Enviado";

                    }

                    return "Correo sin Enviar";
                }
            }


        }


    }
}
