using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
  public class LugarDao: ConnectionToSql
    {

        public void  NuevoLugar(string lugares)
        {
            using(var conexion = GetConnection())
            {
                conexion.Open();
               

                using(var comando= new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Insert into Lugares (nombreLugar) values(@lugar)";
                    comando.Parameters.AddWithValue("@lugar", lugares.ToUpper());
                    comando.CommandType= CommandType.Text;
                    comando.ExecuteNonQuery();
                    
                }
            }
        }



        public void EditarLugar(string lugares, int id)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();

                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Update Lugares set nombreLugar=@lugar where idLugar=@idLugar";
                    comando.Parameters.AddWithValue("@lugar", lugares.ToUpper());
                    comando.Parameters.AddWithValue("@idLugar", id);
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void EliminarLugar(int id)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Delete from Lugares where idLugar=@id";
                    comando.Parameters.AddWithValue("@id", id);
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }



    }


}
