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
    public class ElectricidadModel
    {
        MetodosCommon metodos = new MetodosCommon();
        ElectricidadDao electricidadDao = new ElectricidadDao();
        LocalDao localDao = new LocalDao();

        int idElectricidad;
        DateTime fechaInicio;
        DateTime fechaFin;
        int idLoca;
        int idCli;
        decimal consumo;
        bool estado;
        decimal importe;

        public ElectricidadModel(int idElectricidad, DateTime fechaInicio, DateTime fechaFin,  decimal consumo, bool estado, decimal importe)
        {
            this.idElectricidad = idElectricidad;
            this.fechaInicio = fechaInicio;
            this.fechaFin = fechaFin;
            this.consumo = consumo;
            this.estado = estado;
            this.importe = importe;
        }

        public ElectricidadModel(DateTime fechaInicio, DateTime fechaFin, int idLoca, int idCli, decimal consumo, bool estado, decimal importe)
        {
            this.fechaInicio = fechaInicio;
            this.fechaFin = fechaFin;
            this.idLoca = idLoca;
            this.idCli = idCli;
            this.consumo = consumo;
            this.estado = estado;
            this.importe = importe;
        }

        public ElectricidadModel()
        {
        }


        public string NuevaElec()
        {
            try
            {  //comprobamos el acumulado
                if(metodos.ObtenerNumero("select * from Locales where idLocal=" + idLoca + ";", 2)!= 0){

                    if (!metodos.Existe("select * from Electricidad where fechaInicio <= '" + fechaInicio + "'and idLoca=" + idLoca + ";") && !(metodos.ObtenerNumero("select * from Locales where idLocal=" + idLoca + ";", 2) > consumo))
                    {
                        electricidadDao.NuevoElectricidad(fechaInicio, fechaFin, idLoca, idCli, consumo, estado, importe);

                        return "Guardado";
                    }
                    else
                    {
                        return "Repetido o el consumo no es correcto";
                    }

                }
                else
                {
                    return "Primero debes añadir el acumulado del local";
                }
        
            } catch (Exception ex)
            {
                return "No se ha podido Guardar";
            }
        }


        public string EditarElectricidad()
        {
            try
            {
                if (!metodos.Existe("select * from Electricidad where fechaInicio between '" + fechaInicio + "' and '" + fechaFin + "';") && !metodos.Existe("select * from Electricidad where fechaFin between '" + fechaInicio + "' and '" + fechaFin + "';"))
                {
                    electricidadDao.EditarElectricidad(fechaInicio, fechaFin, consumo, estado, idElectricidad,importe,1);
                    return "Modificado";
                }
                else
                {
                    electricidadDao.EditarElectricidad(fechaInicio, fechaFin, consumo, estado, idElectricidad, importe, 0);
                    return "Modificado resto de datos, salvo fechas";
                }

            } catch (Exception ex)
            {
                return "No se ha podido modificar";
            }

        }


        public string EliminarElectricidad(int id)
        {
            try
            {
                if (!metodos.ObtenerBooleano("select * from Electricidad where idElectricidad=" + idElectricidad + ";", 6))
                {
                    electricidadDao.EliminarElectricidad(id);
                    return "Electricidad eliminada";
                }
                else { return "No es posible, Tiene vinculaciones activas"; }
            }
            catch (Exception ex)
            {
                return "no se ha podido eliminar";
            }
        }


        //buscador por lugar y numero de local o por nombre cliente
        public DataTable CargaLocalesClientes(string lugar, int numero, string nombre, int opcion)
        {
            switch (opcion)
            {
                case 1:

                    return metodos.CargarGridoCmb("select idLocal, nombreLugar, numero, idCliente, nombre, activo,idLugar from Locales,Lugares, Clientes, Alquiler where idLocal=idLoc and idLugar=idLug and idCliente=idCl and nombreLugar='" + lugar + "' and numero=" + numero + ";");
   
                case 2:
                    return metodos.CargarGridoCmb("select idLocal, nombreLugar, numero, idCliente, nombre,activo, idLugar from Locales, Lugares, Clientes, Alquiler where idLocal=idLoc and idLugar=idLug and idCliente=idCl and nombre='" + nombre + "';");
                   
            }

            return metodos.CargarGridoCmb("select idLocal, nombreLugar, numero, idCliente, nombre from Locales,Lugares, Clientes, Alquiler where idLocal=idLoc and idLugar=idLug and idCliente=idCl and nombreLugar='" + lugar + "';");
        }



        public DataTable CargaElectricidad(int idCliente)
        {            

            return metodos.CargarGridoCmb("select idElectricidad, nombreLugar, numero, nombre, estado, fechaInicio, fechaFin, consumo, importe from Electricidad,Locales,Clientes,Lugares where idLocal=idLoca  and idCliente=idCli and idLugar=idLug and idCliente=" + idCliente + ";");

        }


        public DataTable CargaListadoElectricidad(string luga, int idcliente, int opcion)
        {
            if (opcion == 1)
            {
                return metodos.CargarGridoCmb("select idElectricidad, nombreLugar, numero, nombre, estado, fechaInicio, fechaFin, DATEDIFF(day, fechaInicio, fechaFin)/30 as plazo, (consumo-acumulado) as Energia_gasto, importe,idLug, consumo from Electricidad,Locales,Clientes,Lugares where idLocal=idLoca  and idCliente=idCli and idLugar=idLug and nombreLugar='" + luga + "'and idCli=" + idcliente + ";");
            }
            else
            {
                return metodos.CargarGridoCmb("select idElectricidad, nombreLugar, numero, nombre, estado, fechaInicio, fechaFin, DATEDIFF(month, fechaInicio, fechaFin) as plazo, (consumo-acumulado) as Energia_gasto, importe,idLug from Electricidad,Locales,Clientes,Lugares where idLocal=idLoca  and idCliente=idCli and idLugar=idLug and importe=0 and nombreLugar='" + luga + "';");
            }
 
        }


        public DataTable CargaComboLugar()
        {
            return metodos.CargarGridoCmb("Select distinct (idLugar), nombreLugar from Locales, Lugares where idLug=idLugar;");
        }



        //OBTENER TODOS LOS DE UNA ZONA CON IMPORTE 0

        public DataTable CargaParaPotencia(string luga, int opcion)
        {
            if (opcion == 1)
            {
                return metodos.CargarGridoCmb("select idElectricidad, idLoca, idCli, fechaInicio, fechaFin, consumo,idLug from Electricidad,Locales,Clientes,Lugares where idLocal=idLoca  and idCliente=idCli and idLugar=idLug and importe= 0 and nombreLugar='" + luga + "';");
            }
            return metodos.CargarGridoCmb("select idElectricidad, idLoca, consumo, idLug from Electricidad,Locales, Clientes,Lugares where idLocal=idLoca  and idCliente=idCli and idLugar=idLug  and nombreLugar='" + luga + "';");

        }




        public bool ComprobarDecimal(string valor)
        {
            if (metodos.isnumericDecimal(valor))
            {
                return true;
            }
            return false;
        }


      

        public bool ComprobarString(string valor)
        {
            if (metodos.Esnum(valor))
            {
                return true;
            }
            return false;
        }


        public bool ComprobarInt(string valor)
        {
            if (metodos.esNumero(valor))
            {
                return true;
            }
            return false;
        }


        #region CALCULO IMPORTE

        public decimal ImporteTotalPot(DateTime fecha1, DateTime fecha2, int idl, int idcl, string lugar, decimal consumo)
        {
            //listado de fechas finales para contar meses si son distintos años fecha inicial y final
            DateTime[] fechasFinales = new DateTime[] { new DateTime(2018, 12, 31), new DateTime(2019, 12, 31), new DateTime(2020, 12, 31), new DateTime(2021, 12, 31), new DateTime(2022, 12, 31), new DateTime(2023, 12, 31), new DateTime(2024, 12, 31), new DateTime(2025, 12, 31), new DateTime(2026, 12, 31), new DateTime(2027, 12, 31), new DateTime(2028, 12, 31) };
           
            decimal valor = 0;
            decimal importeFinal = 0, potencia1 = 0, importeInicial = 0, porcentaje = 1;

            //OBTENER EL % DE POTENCIA FINAL DEL CLIENTE, SEGUN EL TOTAL DE ENERGIA DE CADA LUGAR Y LA ENERGIA DE CADA CLIENTE
            decimal EnergiaTotalLugar = TotalEnergiaActualizado(lugar, fecha1, fecha2);
            porcentaje = (GastoIndividual(idl, idcl, fecha1, fecha2, consumo) / EnergiaTotalLugar) * 100;
           // int numerolocales = NumeroLocales(lugar);
            //LA POTENCIA DEL AÑO DE LA FECHA INICIAL
                potencia1 = (PotenciaIndividual(lugar, fecha1) * porcentaje)/100;
        
           // int diasTotales = metodos.ObtenerDias(fecha1, fecha2, idl, idcl,2);
           decimal diasTotales= (fecha2 - fecha1).Days;
            
            decimal meses = 0; int i=0;
            decimal mesesFinales = fecha2.Month;
            //RECORRER FECHAS FINALES COMPARANDO CON LA INICIAL + 1 AÑO PARA OBTENER LOS MESES
            foreach (var item in fechasFinales)
            {
                if (fecha2 > item && fecha1.Year == item.Year)
                {
                    decimal diasInicio = (item - fecha1).Days; 
                     meses = (diasTotales - diasInicio) / 30;  //meses restantes años intermedios y meses año fecha final
                    meses = decimal.Round(meses, 2);
                    if (meses != 0)
                    {  importeInicial = potencia1 * (diasInicio / 30); }
                    else { importeInicial = 0; }

                }
             }
            //PARA AÑOS INTERMEDIOS ENTRE EL AÑO DE FECHA INICIAL Y EL AÑO DE FECHA FINAL
            decimal mess = decimal.Round(meses, 0);
            while (mess > 12)
            {
                i++;
                fecha1 = fecha1.AddYears(i);
                decimal potencia2 = (PotenciaIndividual(lugar, fecha1) * porcentaje)/100;
                valor += potencia2 * 12;
                mess -= 12;
                i--;
            }

            decimal potencia3 = (PotenciaIndividual(lugar, fecha2) * porcentaje)/100;
            importeFinal = potencia3 * mesesFinales;
 
            return importeInicial + valor + importeFinal;
        }



        public decimal ImporteTotalEnerg(DateTime fecha1, DateTime fecha2, int idloc, int idcli,decimal consumo)
        {           
            decimal meses = ((fecha2 - fecha1).Days) / 30;
            decimal energiaIndividual = GastoIndividual(idloc, idcli, fecha1, fecha2, consumo);
            decimal promedioValorEnergia = metodos.ObtenerNumero("select avg(importeEnergia) from PotEnerg where anno between '" + fecha1.Year + "'and '" + fecha2.Year + "';", 0);
            return (promedioValorEnergia*energiaIndividual)*meses;
        }


        public decimal PotenciaIndividual(string lugar, DateTime fecha1)
        {
            decimal potenciaYear = metodos.ObtenerNumero("select Potencia from PotEnerg, Lugares where idLu=idLugar and nombreLugar='" + lugar + "'and anno = '" + fecha1.Year + "';", 0);
        
            return potenciaYear;
        }

        //public int NumeroLocales(string lugar)
        //{
        //    return metodos.ObtenerInt("select count(numero) as inquilinos from Locales, Lugares where idLug=idLugar and nombreLugar='" + lugar + "';", 0);
        //}

       

        //METODOS PARA TOTAL DE POTENCIA

        //obtenemos la suma total de energia de un periodo

        public decimal TotalEnergiaYears(string lugar, DateTime fecha1, DateTime fecha2)
        { 
            int mesesFinales, i=0; decimal valor1=0, valor2=0, valor3=0;
            int meses = ((fecha2 - fecha1).Days)/30;
            int mesesIniciales = 13-fecha1.Month;
            valor1 = metodos.ObtenerNumero("select Energia from PotEnerg, Lugares where idLu=idLugar and nombreLugar= '" + lugar + "'and anno='" + fecha1.Year + "';", 0)*mesesIniciales;

            if (fecha1.Year == fecha2.Year) { mesesFinales = 0; } else {  mesesFinales = fecha2.Month;}
            
            int mesesIntermedios = meses - mesesIniciales;

            while (mesesIntermedios > 12)
            {
                i++;
                fecha1 = fecha1.AddYears(i);
                decimal energia =metodos.ObtenerNumero("select Energia  from PotEnerg, Lugares where idLu=idLugar and nombreLugar= '" + lugar + "'and anno='"+fecha1.Year + "';", 0); 
                valor2 += energia * 12;
                mesesIntermedios -= 12;
                i--;
            }

           valor3 = metodos.ObtenerNumero("select Energia from PotEnerg, Lugares where idLu=idLugar and nombreLugar= '" + lugar + "'and anno between '" + fecha1.Year + "'and '" + fecha2.Year + "';", 0)*mesesFinales;
     
            return valor1+valor2+valor3;
        }

       
        //gasto individual de cada uno en un periodo
        public decimal GastoIndividual(int idLocal, int idCliente, DateTime fecha1, DateTime fecha2, decimal consumo)
        {//comparamos si lo que tenemos es igual al inicial, el primer consumo, para que no quede a cero pq se resta al acumulado
            
            if (metodos.ObtenerNumero("select * from Locales where idLocal=" + idLocal + ";", 2) == consumo)
            {
                return 0.1M;
            }
            else
            {
                return metodos.ObtenerNumero("select consumo-acumulado as gastoIndividual from Locales, Electricidad where idLocal=idLoca and idLoca= " + idLocal + "and idCli=" + idCliente + "and fechaFin= '" + fecha2 + "'and fechaInicio= '" + fecha1 + "';", 0);
            }

        }


        //si solo es un usuario tomamos la suma de energia de un periodo, fijada previamente por años
        // si son mas, sumamos las energias individuales de un periodo

        public decimal TotalEnergiaActualizado(string lugar, DateTime fecha1, DateTime fecha2)
        {
            if (metodos.ObtenerInt("select count(idCli) from Electricidad,Locales,Lugares,Clientes where idLocal=idLoca and idLugar=idLug and idCli=idCliente and activo=1 and nombreLugar='" + lugar + "'and fechaInicio='" + fecha1 + "'and fechaFin= '" + fecha2 + "';",0)<2) {

               return TotalEnergiaYears(lugar, fecha1, fecha2);
            }

           return  metodos.ObtenerNumero("select sum(consumo-acumulado) as energia from Locales, Electricidad,Lugares where idLocal=idLoca and idLug=idLugar and nombreLugar='" + lugar + "'and fechaInicio='" + fecha1 + "'and fechaFin= '" + fecha2 + "';", 0);
           
        }


        #endregion

        //POR INDIVIDUO

        public string EditarImporteIndividual(int id, decimal imp)
        {
            try
            {
                electricidadDao.EditarIndividuoElectricidad(id, imp);
                return "Importe individual Modificado";

            }catch(Exception ex)
            {
                return "no se ha podido modificar el importe individual";
            }
        }


       //POR ZONA

        public void EditarAcumuladoZona(string lugares)
        {
            var tabla = CargaParaPotencia(lugares,2);
            for (int i = 0; i < tabla.Rows.Count; i++)
            {
                int idlocal = int.Parse(tabla.Rows[i]["idLoca"].ToString());
                int idlug = int.Parse(tabla.Rows[i]["idLug"].ToString());
                decimal cons = decimal.Parse(tabla.Rows[i]["consumo"].ToString());
                localDao.EditarLocalAcumladoZona(idlug, cons, idlocal);
            }
            
        }

        public string EditarImporteZona(string lugares)
        {
            try
            {    
                var tabla = CargaParaPotencia(lugares,1);
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                   DateTime fecha1= DateTime.Parse( tabla.Rows[i]["fechaInicio"].ToString());
                    DateTime fecha2 = DateTime.Parse(tabla.Rows[i]["fechaFin"].ToString());
                    int idcliente = int.Parse(tabla.Rows[i]["idCli"].ToString());
                    int idlocal = int.Parse(tabla.Rows[i]["idLoca"].ToString());
                    int idlug= int.Parse(tabla.Rows[i]["idLug"].ToString());
                    decimal cons = decimal.Parse(tabla.Rows[i]["consumo"].ToString());
                    decimal importeEnerg = ImporteTotalEnerg(fecha1, fecha2,idlocal,idcliente,cons);
                    decimal importePot = ImporteTotalPot(fecha1, fecha2, idlocal, idcliente, lugares, cons);
                    decimal importe = importeEnerg + importePot;
                    electricidadDao.EditarTotalElectricidad( importe, idlocal);
                  
                }
                
                return "Actualizado importe";

            }
            catch(Exception ex)
            {
                return "No se ha podido Actualizar el importe";
            }
           
        }

    }
}
