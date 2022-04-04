using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class ElectricidadDao: ConnectionToSql
    {

        public void NuevoElectricidad(DateTime fini, DateTime ffin, int idlo, int idcl, decimal cons, bool est, decimal importe)
        {
            using (var conexion=GetConnection())
            {
                conexion.Open();

                using(var comando= new SqlCommand())
                { 
                    comando.Connection = conexion;
                    comando.CommandText = "Insert into Electricidad (fechaInicio,fechaFin,idLoca, idCli,consumo,estado,importe) values (@fi,@ff,@idlocal,@idcliente,@consumo,@estado,@impor)";
                    comando.Parameters.AddWithValue("@fi",fini);
                    comando.Parameters.AddWithValue("@ff",ffin);
                    comando.Parameters.AddWithValue("@idlocal",idlo);
                    comando.Parameters.AddWithValue("@idcliente",idcl);
                    comando.Parameters.AddWithValue("@consumo",cons);
                    comando.Parameters.AddWithValue("@estado",est);
                    comando.Parameters.AddWithValue("@impor", importe);
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }

            }
        }


        public void EditarElectricidad(DateTime fini, DateTime ffin,  decimal cons, bool est, int idEl, decimal importe, int opcion)
        {
            using(var conexion= GetConnection())
            {
                conexion.Open();

                using(var comando= new SqlCommand())
                {
                    comando.Connection = conexion;
                    if (opcion == 1)
                    {
                        comando.CommandText = "Update Electricidad set fechaInicio=@fi, fechaFin=@ff, consumo=@consumo, estado=@estado, importe=@impor where idElectricidad=@idElec";
                        comando.Parameters.AddWithValue("@fi", fini);
                        comando.Parameters.AddWithValue("@ff", ffin);
                        comando.Parameters.AddWithValue("@consumo", cons);
                        comando.Parameters.AddWithValue("@estado", est);
                        comando.Parameters.AddWithValue("@idElec", idEl);
                        comando.Parameters.AddWithValue("@impor", importe);
                    }
                    else
                    {
                        comando.CommandText = "Update Electricidad set consumo=@consumo, estado=@estado, importe=0 where idElectricidad=@idElec";
                        comando.Parameters.AddWithValue("@consumo", cons);
                        comando.Parameters.AddWithValue("@estado", est);
                        comando.Parameters.AddWithValue("@idElec", idEl);
                       
                    }
                  
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void EditarTotalElectricidad( decimal importe,int idloc)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();

                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Update Electricidad set importe= @impor where importe=0 and idLoca=@idlocal";
                    comando.Parameters.AddWithValue("@impor", importe);
                    comando.Parameters.AddWithValue("@idlocal", idloc);
                    comando.CommandType = CommandType.Text;
                   comando.ExecuteNonQuery();
                }
            }
        }


        public void EditarIndividuoElectricidad(int id, decimal importe)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();

                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Update Electricidad set importe=@impor where idElectricidad=@idElec";
                    comando.Parameters.AddWithValue("@impor", importe);
                    comando.Parameters.AddWithValue("@idElec", id);
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }


        public void EliminarElectricidad(int id)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Delete from Electricidad where idElectricidad=@id";
                    comando.Parameters.AddWithValue("@id", id);
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }

    }
}
