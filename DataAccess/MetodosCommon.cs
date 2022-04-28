using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Common.Cache;
using System.Windows.Forms;

namespace DataAccess
{
  public  class MetodosCommon:ConnectionToSql
    {

        public bool Existe(string consulta)
        {
           
            bool es = false;     
            using (var conexion = GetConnection())
            {
                try
                {
                    conexion.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexion;
                        comando.CommandText = consulta;
                        comando.CommandType = CommandType.Text;
                        SqlDataReader reader = comando.ExecuteReader();
                        if (reader.HasRows)
                        { 
                            return true;
                        }
                        else { return false; }
                    }
                }catch(Exception ex) { MessageBox.Show("Error: " + ex.Message); }

                return es;
            }
        }


        public string Obtenerdato(string sql, int posicion)
        {
            string dato = "";
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
                        { dato = reader.GetString(posicion); }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
     
            return dato;
        }



        public decimal ObtenerNumero(string sql, int posicion)
        {
            decimal dato=0 ;
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
                        { dato = reader.GetDecimal(posicion); }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }

            return dato;
        }


        public int ObtenerInt(string sql, int posicion)
        {
            int dato = 0;
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
                        { dato = reader.GetInt32(posicion); }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }

            return dato;
        }

       


        public DateTime GetFecha(string sql, int posicion)
        {
            using (var conexion = GetConnection())
            {
                DateTime fecha=new DateTime();
                try
                {
                    conexion.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexion;
                        comando.CommandText = sql;
                        comando.CommandType = CommandType.Text;
                        SqlDataReader reader = comando.ExecuteReader();

                        while (reader.Read())
                        { fecha = reader.GetDateTime(posicion); }
                    }
               
                } catch(Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                return fecha;
            }
        }


        //public bool ObtenerBooleano(string sql, int posicion)
        //{
        //    bool estado=false;
        //    try
        //    {
        //        using(var conexion= GetConnection())
        //        {
        //            conexion.Open();
        //            using(var comando= new SqlCommand())
        //            {
        //                comando.Connection = conexion;
        //                comando.CommandText = sql;
        //                comando.CommandType = CommandType.Text;
        //                SqlDataReader reader = comando.ExecuteReader();

        //                while (reader.Read())
        //                { estado = reader.GetBoolean(posicion); }
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show("Error: " + ex.Message);
        //    }

        //    return estado;
        //}



        public DataTable CargarGridoCmb(string sql)
        {
            DataTable dt = new DataTable();
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
                        SqlDataAdapter da = new SqlDataAdapter(comando);
                        da.Fill(dt);
                       
                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
               
            }
            return dt;
        }

        public bool Esnum(string value)
        {
            return value.All(char.IsNumber);
        }


        public bool isnumericDecimal(string txtbox)
        {
            return decimal.TryParse(txtbox, out decimal valor);
        }

        //public bool isnumeric(string txtbox)
        //{
        //    return int.TryParse(txtbox, out int valor);

        //}

        public bool esNumero(string txtbox)
        {
            return txtbox.All(char.IsDigit);
        }

       

        public List<T> CreaLista<T>(params T[] pars)
        {
            List<T> list = new List<T>();
            foreach (T elem in pars)
            {
                list.Add(elem);
            }
            return list;
        }

        //List<string> noms = CreaLista<string>("Pepe", "Juan", "Luis");  var p = CreaLista( new { X = 1, Y = 2 }, new { X = 3, Y = 4 });
        //Console.WriteLine(p[1].Y); // Pinta "4" 

    }
}
