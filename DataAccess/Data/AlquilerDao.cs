using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
   public class AlquilerDao:ConnectionToSql
    {

        public void NuevoAlquiler(DateTime fecha, int fianza, decimal import,string obs, int idl, int idc, bool mod)
        {
            using(var conexion= GetConnection())
            {
                conexion.Open();
                using(var comando= new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Insert into Alquiler (fechaIni,fianza,importe,observaciones, idLoc, idCl,modelo) values(@fecha,@fia,@im,@obser,@idL,@idC,@model)";
                    comando.Parameters.AddWithValue("@fecha",fecha);
                    comando.Parameters.AddWithValue("@fia", fianza);
                    comando.Parameters.AddWithValue("@im", import);
                    comando.Parameters.AddWithValue("@obser", obs);
                    comando.Parameters.AddWithValue("@idL", idl);
                    comando.Parameters.AddWithValue("@idC", idc);
                    comando.Parameters.AddWithValue("@model", mod);
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }


        public void EditarAlquiler(DateTime fecha, int fianza, decimal import, string obs, int idl,  int idAlq, bool mod, int opcion)
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
                            comando.CommandText = "Update Alquiler set fechaIni=@fecha,fianza=@fia,importe=@im,observaciones=@obser, idLoc=@idL, modelo=@model where idAlquiler=@idA";
                            comando.Parameters.AddWithValue("@fecha", fecha);
                            comando.Parameters.AddWithValue("@fia", fianza);
                            comando.Parameters.AddWithValue("@im", import);
                            comando.Parameters.AddWithValue("@obser", obs);
                            comando.Parameters.AddWithValue("@idL", idl);
                            comando.Parameters.AddWithValue("@idA", idAlq);
                            comando.Parameters.AddWithValue("@model", mod);
                            break;

                        case 2:
                            comando.CommandText = "Update Alquiler set fechaIni=@fecha,fianza=@fia,importe=@im,observaciones=@obser,  modelo=@model where idAlquiler=@idA";
                            comando.Parameters.AddWithValue("@fecha", fecha);
                            comando.Parameters.AddWithValue("@fia", fianza);
                            comando.Parameters.AddWithValue("@im", import);
                            comando.Parameters.AddWithValue("@obser", obs);
                            comando.Parameters.AddWithValue("@idA", idAlq);
                            comando.Parameters.AddWithValue("@model", mod);
                            break;

                        case 3:
                            comando.CommandText = "Update Alquiler set importe=@im,observaciones=@obser where idAlquiler=@idA";
                            comando.Parameters.AddWithValue("@im", import);
                            comando.Parameters.AddWithValue("@obser", obs);
                            comando.Parameters.AddWithValue("@idA", idAlq);
                            break;

                    }

                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }


        public void EliminarAlquiler(int id)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Delete from Alquiler where idAlquiler=@id";
                    comando.Parameters.AddWithValue("@id", id);
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }


    }
}
