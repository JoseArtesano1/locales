using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    //clase para compensar periodos
    public class ObjetoAuxiliar
    {
        public DateTime fecha;
        public decimal gasto;

        public ObjetoAuxiliar(DateTime fecha, decimal gasto)
        {
            this.fecha = fecha;
            this.gasto = gasto;
        }

        public ObjetoAuxiliar()
        {

        }
    }

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
                    switch (opcion)
                    {
                        case 0:
                             comando.CommandText = "Update Electricidad set fechaInicio=@fi, fechaFin=@ff, consumo=@consumo, estado=@estado, importe=@impor where idElectricidad=@idElec";
                            comando.Parameters.AddWithValue("@fi", fini);
                            comando.Parameters.AddWithValue("@ff", ffin);
                            comando.Parameters.AddWithValue("@consumo", cons);
                            comando.Parameters.AddWithValue("@estado", est);
                            comando.Parameters.AddWithValue("@idElec", idEl);
                            comando.Parameters.AddWithValue("@impor", importe);
                            break;

                        case 1:
                            comando.CommandText = "Update Electricidad set consumo=@consumo, estado=@estado, importe=0 where idElectricidad=@idElec";
                            comando.Parameters.AddWithValue("@consumo", cons);
                            comando.Parameters.AddWithValue("@estado", est);
                            comando.Parameters.AddWithValue("@idElec", idEl);
                            break;

                        case 2:
                            comando.CommandText = "Update Electricidad set estado=@estado where idElectricidad=@idElec";
                            comando.Parameters.AddWithValue("@estado", est);
                            comando.Parameters.AddWithValue("@idElec", idEl);
                            break;
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


        public void EditarEstadoZona(int idloc)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();

                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "update Electricidad set estado=1 where idLoca=@idlocal";
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


        public decimal listaCompensacion(string lugar, DateTime f1, DateTime f2, int posicion, decimal tiempo)
        {
            ObjetoAuxiliar objetoList = new ObjetoAuxiliar();
            List<ObjetoAuxiliar> listado = new List<ObjetoAuxiliar>();
            decimal comp = 0;
            using (var conexion = GetConnection())
            {
                conexion.Open();

                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;

                    comando.CommandText = "select (consumo - acumulado) as energia, fechaInicio from Locales, Electricidad, Lugares where idLocal = idLoca and idLug = idLugar and estado=0 and importe>0 and nombreLugar =@lug and fechaInicio!=@fe1  and fechaFin=@fe2";
                    comando.Parameters.AddWithValue("@lug", lugar);
                    comando.Parameters.AddWithValue("@fe2", f2);
                    comando.Parameters.AddWithValue("@fe1", f1);

                    comando.CommandType = CommandType.Text;
                    SqlDataReader reader = comando.ExecuteReader();

                    while (reader.Read())
                    {
                        ObjetoAuxiliar objeto = new ObjetoAuxiliar();

                        objeto.fecha = reader.GetDateTime(1);
                        objeto.gasto = reader.GetDecimal(posicion);
                        listado.Add(objeto);
                    }
                }

            }

            decimal dias = 30; 
            foreach (var i in listado)
            {
                decimal annos = (((f2 - i.fecha).Days) / dias) / 12;
                comp += (i.gasto / annos) * Decimal.Round(tiempo, 00);
            }

            return Math.Round(comp, 2);
        }

    }
}
