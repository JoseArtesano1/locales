using DataAccess;
using DataAccess.Data;
using DataAccess.Documents;
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
        Pdfs pdf = new Pdfs();

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

                    if (!metodos.Existe("select * from Electricidad where fechaInicio >= '" + fechaInicio + "'and idLoca=" + idLoca + ";") && !(metodos.ObtenerNumero("select * from Locales where idLocal=" + idLoca + ";", 2) > consumo))
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
                if(metodos.Existe("select * from Electricidad where importe=0 and idElectricidad=" +idElectricidad + ";"))
                {
                    if (!metodos.Existe("select * from Electricidad where idElectricidad=" + idElectricidad + " and fechaInicio='" + fechaInicio + "' and fechaFin= '" + fechaFin + "';"))
                    {
                        electricidadDao.EditarElectricidad(fechaInicio, fechaFin, consumo, estado, idElectricidad, importe, 0);
                        return "Modificado";
                    }
                    else
                    {
                        electricidadDao.EditarElectricidad(fechaInicio, fechaFin, consumo, estado, idElectricidad, importe, 1);
                        return "Modificado resto de datos, salvo fechas";
                    }
                }
                else
                {
                    electricidadDao.EditarElectricidad(fechaInicio, fechaFin, consumo, estado, idElectricidad, importe, 2);
                    return "Solo se puede modificar el estado";
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
                if (!metodos.Existe("select * from Electricidad where estado=0 and idElectricidad=" + id + ";"))
                {
                    electricidadDao.EliminarElectricidad(id);
                    return "Electricidad eliminada";
                }
                else { return "No es posible, Tiene vinculaciones activas"; }
            }
            catch (Exception ex)
            {
                return "no se ha podido eliminar" + ex;
            }
        }


        //buscador por lugar y numero de local o por nombre cliente
        public DataTable CargaLocalesClientes(string lugar, int numero, string nombre, int opcion, int idluga)
        {
            switch (opcion)
            {
                case 0:
                    return metodos.CargarGridoCmb("select idLocal,  numero, idCliente, nombre, activo,idLugar, dni, direccion, importe from Locales, Lugares, Clientes, Alquiler where idLocal=idLoc and idLugar=idLug and idCliente=idCl and activo=1 and tipo=0 and modelo=0 and nombreLugar='" + lugar + "';");

                case 1:

                    return metodos.CargarGridoCmb("select idLocal, nombreLugar, numero, idCliente, nombre, activo,idLugar from Locales,Lugares, Clientes, Alquiler where idLocal=idLoc and idLugar=idLug and idCliente=idCl and activo=1 and nombreLugar='" + lugar + "' and numero=" + numero + ";");
   
                case 2:
                    return metodos.CargarGridoCmb("select idLocal, nombreLugar, numero, idCliente, nombre,activo, idLugar from Locales, Lugares, Clientes, Alquiler where idLocal=idLoc and idLugar=idLug and idCliente=idCl and activo=1 and nombre='" + nombre + "';");
                   
            }

            return metodos.CargarGridoCmb("select idLocal, nombreLugar, numero, idCliente, nombre from Locales,Lugares, Clientes, Alquiler where idLocal=idLoc and idLugar=idLug and idCliente=idCl and activo=1 and idLugar=" + idluga + ";");
        }



        public DataTable CargaElectricidad(int idCliente)
        {            

            return metodos.CargarGridoCmb("select idElectricidad as id, nombreLugar as Lugar, numero as Nº, nombre, estado, fechaInicio as Inicio, fechaFin as Fin, consumo from Electricidad,Locales,Clientes,Lugares where idLocal=idLoca  and idCliente=idCli and idLugar=idLug and activo=1 and estado=0 and idCliente=" + idCliente + ";");

        }


        public DataTable CargaListadoElectricidad(string luga, int idcliente, int idlug, int opcion)
        {
            switch (opcion)
            {
                case 1:
                    return metodos.CargarGridoCmb("select idElectricidad as id, nombreLugar as Lugar, numero, nombre, estado, fechaInicio as Inicio, fechaFin as Fin, DATEDIFF(day, fechaInicio,fechaFin)/30 as plazo, (consumo-acumulado) as Gasto, importe as Importe, consumo from Electricidad,Locales,Clientes,Lugares where idLocal=idLoca  and idCliente=idCli and idLugar=idLug and activo=1 and importe=0 and nombreLugar='" + luga + "'and idCli=" + idcliente + ";");

                case 2:
                    return metodos.CargarGridoCmb("select idElectricidad as id, nombreLugar as Lugar, numero, nombre, estado, fechaInicio as Inicio, fechaFin as Fin, DATEDIFF(day, fechaInicio,fechaFin )/30 as plazo, (consumo-acumulado) as Gasto, importe as Importe from Electricidad,Locales,Clientes,Lugares where idLocal=idLoca  and idCliente=idCli and idLugar=idLug and activo=1 and importe=0 and nombreLugar='" + luga + "';");
            }

            return metodos.CargarGridoCmb("select numero as Trastero, nombre as Cliente,  fechaInicio as Inicio, fechaFin as Fin, DATEDIFF(day, fechaInicio,fechaFin) / 30 as Plazo, (consumo - acumulado) as Gasto, importe as Importe, estado from Electricidad, Locales, Clientes where idLocal = idLoca  and idCliente = idCli and estado=0  and idLug = " + idlug + "; ");

        }


        public DataTable CargaComboLugar()
        {
            return metodos.CargarGridoCmb("Select distinct (idLugar), nombreLugar from Locales, Lugares where idLug=idLugar;");
        }



        //OBTENER TODOS LOS DE UNA ZONA CON IMPORTE 0
        public DataTable CargaParaPotencia(string luga, int idlugar, int opcion)
        {
            switch(opcion)
            {
                case 1:
                return metodos.CargarGridoCmb("select idElectricidad, idLoca, idCli, fechaInicio, fechaFin, consumo,idLug from Electricidad,Locales,Clientes,Lugares where idLocal=idLoca  and idCliente=idCli and idLugar=idLug and activo=1 and importe= 0 and nombreLugar='" + luga + "';");
                case 2:
                    return metodos.CargarGridoCmb("select idElectricidad, idLoca, consumo, idLug from Electricidad,Locales, Clientes,Lugares where idLocal=idLoca  and idCliente=idCli and idLugar=idLug and consumo >acumulado and activo=1 and importe >0 and estado=1 and nombreLugar='" + luga + "';");
            }
            return metodos.CargarGridoCmb("select idElectricidad, idLoca, idLug, estado from Electricidad, Locales where idLocal=idLocal and importe >0  and estado=0 and idLug=" + idlugar + ";");

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

        public decimal TotalEnergyCompensar(string lugar, DateTime fecha1, DateTime fecha2, decimal tiempo)
        {
            if (metodos.ObtenerInt("select count(idElectricidad)  from electricidad, Lugares, locales where idLocal=idLoca and idLugar= idLug and estado=0 and nombreLugar='" + lugar + "'and fechaFin='" + fecha2 + "' and fechaInicio='" + fecha1 + "';", 0) > 2)
            {
                return electricidadDao.listaCompensacion(lugar, fecha1, fecha2, 0, tiempo);
            }

            return 0;

        }


        public decimal ImporteTotalPot(DateTime fecha1, DateTime fecha2, int idl, int idcl, string lugar, decimal consumo)
         {
            //listado de fechas finales para contar meses si son distintos años fecha inicial y final
            DateTime[] fechasFinales = new DateTime[] { new DateTime(2018, 12, 31), new DateTime(2019, 12, 31), new DateTime(2020, 12, 31), new DateTime(2021, 12, 31), new DateTime(2022, 12, 31), new DateTime(2023, 12, 31), new DateTime(2024, 12, 31), new DateTime(2025, 12, 31), new DateTime(2026, 12, 31), new DateTime(2027, 12, 31), new DateTime(2028, 12, 31), new DateTime(2029, 12, 31) };
           
            decimal importeIntermedio = 0;
            decimal importeFinal = 0, potencia = 0, importeInicial = 0, porcentaje = 1;
            decimal diasTotales = (fecha2 - fecha1).Days, mesesFinales;
            decimal meses = 0; int i = 0;

            //OBTENER EL % DE POTENCIA FINAL DEL CLIENTE, SEGUN EL TOTAL DE ENERGIA DE CADA LUGAR Y LA ENERGIA DE CADA CLIENTE
            decimal compensar = TotalEnergyCompensar(lugar, fecha1, fecha2, (diasTotales / 30) / 12);
            decimal EnergiaTotalLugar = TotalEnergiaActualizado(lugar, fecha1, fecha2) +compensar;
            porcentaje = (GastoIndividual(idl, idcl, fecha1, fecha2, consumo) / EnergiaTotalLugar) * 100;
           
            //LA POTENCIA DEL AÑO DE LA FECHA INICIAL
                potencia = (PotenciaIndividual(lugar, fecha1) * Math.Round(porcentaje, 2)) / 100;

            //==========================================PERIODOS================================================
            
            //PERIODO INFERIOR AL AÑO
            if (fecha1.Year == fecha2.Year)
            {
                mesesFinales = ((fecha2 - fecha1).Days) / 30.00m;
            }
            else
            {
                //PERIODO INICIAL
                //RECORRER FECHAS FINALES COMPARANDO CON LA INICIAL + 1 AÑO PARA OBTENER LOS MESES
                foreach (var item in fechasFinales)
                {
                    if (fecha2 > item && fecha1.Year == item.Year)
                    {
                        decimal diasInicio = (item - fecha1).Days;
                        meses = (diasTotales - diasInicio) / 30;  //meses restantes años intermedios y meses año fecha final
                        meses = decimal.Round(meses, 2);
                        if (meses != 0)
                        { importeInicial = potencia * (diasInicio / 30); }
                        else { importeInicial = 0; }
                        break;
                    }
                }

                //PERIODO INTERMEDIO
                //PARA AÑOS INTERMEDIOS ENTRE EL AÑO DE FECHA INICIAL Y EL AÑO DE FECHA FINAL
                decimal mess = decimal.Round(meses, 0);
                while (mess > 12)
                {
                    i++;
                    fecha1 = fecha1.AddYears(i);
                    importeIntermedio += (PotenciaIndividual(lugar, fecha1)) * 12;
                    mess -= 12;
                    i--;
                }

                //PERIODO FINAL
                mesesFinales = mess;
            }
            
            importeFinal = PotenciaIndividual(lugar, fecha2) * mesesFinales;
 
            return importeInicial + importeIntermedio + importeFinal;
        }



        public decimal ImporteTotalEnerg(DateTime fecha1, DateTime fecha2, int idloc, int idcli,decimal consumo,string lugar)
        {  
            decimal energiaIndividual = GastoIndividual(idloc, idcli, fecha1, fecha2, consumo);
            decimal promedioValorEnergia = metodos.ObtenerNumero("select avg(importeEnergia) from PotEnerg, Lugares where idLu=idLugar and nombreLugar='" + lugar + "' and anno between '" + fecha1.Year + "'and '" + fecha2.Year + "';", 0);
            return (promedioValorEnergia*energiaIndividual);
        }


        public decimal PotenciaIndividual(string lugar, DateTime fecha1)
        {
            decimal potenciaYear = metodos.ObtenerNumero("select Potencia from PotEnerg, Lugares where idLu=idLugar and nombreLugar='" + lugar + "'and anno = '" + fecha1.Year + "';", 0);
        
            return potenciaYear;
        }

       

        //METODOS PARA TOTAL DE POTENCIA

        //obtenemos la suma total de energia de un periodo

        public decimal TotalEnergiaYears(string lugar, DateTime fecha1, DateTime fecha2)
        { 
            int mesesFinales, i=0; decimal valor1=0, valor2=0, valor3= 0, energia = 0;
            int meses = ((fecha2 - fecha1).Days)/30;
            int mesesIniciales; //= 13-fecha1.Month;

            energia = metodos.ObtenerNumero("select Energia  from PotEnerg, Lugares where idLu=idLugar and nombreLugar= '" + lugar + "'and anno='" + fecha1.Year + "';", 0);

            //==============================================PERIODOS==========================================

            if (fecha1.Year == fecha2.Year)
            { mesesFinales = 0;
                if (meses == 0)
                { mesesIniciales = 1; } 
                else { mesesIniciales = meses; }
                valor1 = energia * mesesIniciales;
            }
            else 
            {
                //PERIODO INICIAL
                mesesIniciales = 13 - fecha1.Month;
                valor1 = energia * mesesIniciales;

                //PERIODO INTERMEDIO
                int mesesIntermedios = meses - mesesIniciales;

                while (mesesIntermedios > 12)
                {
                    i++;
                    fecha1 = fecha1.AddYears(i);
                  decimal energia2=  metodos.ObtenerNumero("select Energia  from PotEnerg, Lugares where idLu=idLugar and nombreLugar= '" + lugar + "'and anno='" + fecha1.Year + "';", 0);
                    valor2 += energia2 * 12;
                    mesesIntermedios -= 12;
                    i--;
                }

                //PERIODO FINAL
                mesesFinales = fecha2.Month;
                valor3 = metodos.ObtenerNumero("select Energia  from PotEnerg, Lugares where idLu=idLugar and nombreLugar= '" + lugar + "'and anno='" + fecha2.Year + "';", 0) * mesesFinales;
            }
     
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

        public string EditarAcumuladoZona(string lugares)
        {  try
            {
                var tabla = CargaParaPotencia(lugares, 0, 2);
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    int idlocal = int.Parse(tabla.Rows[i]["idLoca"].ToString());
                    int idlug = int.Parse(tabla.Rows[i]["idLug"].ToString());
                    decimal cons = decimal.Parse(tabla.Rows[i]["consumo"].ToString());
                    localDao.EditarLocalAcumladoZona(idlug, cons, idlocal);
                }
                return "Acumulado actualizado";
            }
            catch (Exception ex)
            {
                return "No se ha podido Actualizar el acumulado" + ex;
            }
        }

        public string EditarImporteZona(string lugares)
        {
            try
            {    
                var tabla = CargaParaPotencia(lugares,0,1);
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                   DateTime fecha1= DateTime.Parse( tabla.Rows[i]["fechaInicio"].ToString());
                    DateTime fecha2 = DateTime.Parse(tabla.Rows[i]["fechaFin"].ToString());
                    int idcliente = int.Parse(tabla.Rows[i]["idCli"].ToString());
                    int idlocal = int.Parse(tabla.Rows[i]["idLoca"].ToString());
                    int idlug= int.Parse(tabla.Rows[i]["idLug"].ToString());
                    decimal cons = decimal.Parse(tabla.Rows[i]["consumo"].ToString());
                    decimal importeEnerg = ImporteTotalEnerg(fecha1, fecha2,idlocal,idcliente,cons,lugares);
                    decimal importePot = ImporteTotalPot(fecha1, fecha2, idlocal, idcliente, lugares, cons);
                    decimal importe = importeEnerg + importePot;
                    electricidadDao.EditarTotalElectricidad( importe, idlocal);
                  
                }
                
                return "Actualizado importe";

            }
            catch(Exception ex)
            {
                return "No se ha podido Actualizar el importe" + ex;
            }
           
        }



        public string GenerarInforme( int idl)
        {
            try
            {              
                pdf.GenerarPdf( idl);
                return "Informe generado";
            }
            catch (Exception ex)
            {
                return "No se ha podido Generar el informe" + ex;
            }
        }


        public bool botonesAcumulados(int idlocal, int idcliente, int idLug, string zona, int opcion)
        {
            switch (opcion)
            {
                case 1:
                    return metodos.Existe("select * from Electricidad,Locales where idLoca=idLocal and consumo >acumulado and importe >0 and estado=1 and idLoca=" + idlocal + " and idCli=" + idcliente + ";");
                case 2:
                    return metodos.Existe("select * from Electricidad, Locales, Lugares where idLoca=idLocal and idLug=idLugar and consumo >acumulado and importe >0 and estado=1 and nombreLugar='" + zona + "';");

                case 3:
                    return metodos.Existe("select * from Electricidad, Locales where idLoca=idLocal  and importe >0 and estado=0 and idLug=" + idLug + ";");
            }
            return metodos.Existe("select * from Electricidad,Locales where idLoca=idLocal  and importe =0 and estado=0 and idLoca=" + idlocal + " and idCli=" + idcliente + ";");

        }


        public int NumeroEstados(string zona, int idlug, int opcion)
        {
            if (opcion == 1)
            {
                return metodos.ObtenerInt("select count(idElectricidad) from Electricidad,Locales, Lugares where idLoca=idLocal and idLug=idLugar and importe >0 and estado=1 and nombreLugar='" + zona + "';", 0);
            }
            else
            {
                return metodos.ObtenerInt("select count(idElectricidad) from Electricidad,Locales where idLoca=idLocal and importe >0 and estado=0 and idLug=" + idlug + ";", 0);
            }

        }

        public string EditarEstadoZona(int idLugar)
        {
            try
            {
                var tabla = CargaParaPotencia("", idLugar, 3);
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    int idlocal = int.Parse(tabla.Rows[i]["idLoca"].ToString());
                    electricidadDao.EditarEstadoZona(idlocal);
                }
                return "Actualizado Estado";
            }
            catch (Exception ex)
            {
                return "Estado sin Actualizar";
            }
        }
    }
}
