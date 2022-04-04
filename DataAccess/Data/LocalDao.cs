using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.Data
{
   public class LocalDao:ConnectionToSql
    {

        public void NuevoLocal( int num, decimal total, int idlug)
        {
            using(var conexion= GetConnection()) 
            {
                conexion.Open();

                using(var comando= new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Insert into Locales (numero,acumulado,idLug) values(@numero,@suma,@idlg)";
                    
                    comando.Parameters.AddWithValue("@numero", num);
                    comando.Parameters.AddWithValue("@suma", total);
                    comando.Parameters.AddWithValue("@idlg", idlug);
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }



        public void EditarLocalAcumladoZona(int id, decimal total, int idloc)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                  
                        comando.CommandText = "Update Locales set  acumulado=@ttal where idLug=@id and idLocal=@idlo";
                        comando.Parameters.AddWithValue("@id", id);
                        comando.Parameters.AddWithValue("@ttal", total);
                        comando.Parameters.AddWithValue("@idlo", idloc);
                         comando.CommandType = CommandType.Text;
                         comando.ExecuteNonQuery();
                }
            }
        }


        public void EditarLocal(int id, int idLug, int numero, decimal total, bool opcion)
        {
            using(var conexion = GetConnection())
            {
                conexion.Open();
                using(var comando= new SqlCommand())
                {
                    comando.Connection = conexion;
                    if (opcion)
                    {
                        comando.CommandText = "Update Locales set numero=@num, acumulado=@ttal, idLug=@idLugar where idLocal=@id";
                        comando.Parameters.AddWithValue("@idLugar", idLug);
                        comando.Parameters.AddWithValue("@num", numero);
                        comando.Parameters.AddWithValue("@id", id);
                        comando.Parameters.AddWithValue("@ttal", total);
                    }
                    else
                    {
                        comando.CommandText = "Update Locales set  acumulado=@ttal where idLocal=@id";
                        comando.Parameters.AddWithValue("@id", id);
                        comando.Parameters.AddWithValue("@ttal", total);
                    }
                  
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }


        public void EliminarLocal(int id)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Delete from Locales where idLocal=@id";
                    comando.Parameters.AddWithValue("@id", id);
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void NuevoPotEng(int year, decimal energia, decimal potencia, int idlugar,decimal importeEnergy)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();

                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Insert into PotEnerg (anno, Energia, Potencia, idLu, importeEnergia) values(@año, @energy, @pot, @idloc, @impEnerg)";
                    comando.Parameters.AddWithValue("@año", year);
                    comando.Parameters.AddWithValue("@energy", energia);
                    comando.Parameters.AddWithValue("@pot", potencia);
                    comando.Parameters.AddWithValue("@idloc", idlugar);
                    comando.Parameters.AddWithValue("@impEnerg", importeEnergy);
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }


        public void EditarPotEng(int idpe, decimal pot, decimal ener, int idlugar, int anno, decimal importeEnergy, bool opcion)
        {
            using(var conexion= GetConnection())
            {
                conexion.Open();
                using(var comando= new SqlCommand())
                {
                    comando.Connection = conexion;

                    if (opcion)
                    {
                        comando.CommandText = "Update PotEnerg set anno=@a, Energia=@energ, Potencia=@pote, idLu=@idl, importeEnergia=@impEnerg where idPotEnergy=@idpeng";
                        comando.Parameters.AddWithValue("@a", anno);
                        comando.Parameters.AddWithValue("@energ", ener);
                        comando.Parameters.AddWithValue("@pote", pot);
                        comando.Parameters.AddWithValue("@idl", idlugar);
                        comando.Parameters.AddWithValue("@idpeng", idpe);
                        comando.Parameters.AddWithValue("@impEnerg", importeEnergy);
                    }
                    else
                    {
                        comando.CommandText = "Update PotEnerg set Energia=@energ, Potencia=@pote,idLu=@idl,importeEnergia=@impEnerg where idPotEnergy=@idpeng";
                       
                        comando.Parameters.AddWithValue("@energ", ener);
                        comando.Parameters.AddWithValue("@pote", pot);
                        comando.Parameters.AddWithValue("@idl", idlugar);
                        comando.Parameters.AddWithValue("@idpeng", idpe);
                        comando.Parameters.AddWithValue("@impEnerg", importeEnergy);
                    }
                   
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }


      


        public void EliminarPotenciaEnergia(int id, int year)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "Delete from PotEnerg where idLu=@id and anno=@year";
                    comando.Parameters.AddWithValue("@id", id);
                    comando.Parameters.AddWithValue("@year", year);
                    comando.CommandType = CommandType.Text;
                    comando.ExecuteNonQuery();
                }
            }
        }


        public List<int> listadoYears(string sql)
        {
            List<int> listado = new List<int>();
            using (var conexion = GetConnection())
            {
                conexion.Open();
                try
                {
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexion;
                        comando.CommandText = sql;
                        comando.CommandType = CommandType.Text;
                        SqlDataReader reader = comando.ExecuteReader();

                        while (reader.Read())
                        {
                            listado.Add((int)reader[0]);
                           // listado.Add(reader.GetInt32(0));
                        }
                    }
                }
                catch (Exception ex) { }
            }
            return listado;
        }



       


    }
}
