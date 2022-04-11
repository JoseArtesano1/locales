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
   public class PotenciaEnergiaModel
    {
        MetodosCommon metodos = new MetodosCommon();
        LocalDao localDao = new LocalDao();


        int idPotEnergy;
        int anno;
        decimal Energia;
        decimal Potencia;
        int idLu;
        decimal importeEnergia;

        public PotenciaEnergiaModel(int idPotEnergy, int anno, decimal Energia, decimal Potencia, int idLu, decimal importeEnergia)
        {
            this.idPotEnergy = idPotEnergy;
            this.anno = anno;
            this.Energia = Energia;
            this.Potencia = Potencia;
            this.idLu = idLu;
            this.importeEnergia = importeEnergia;
        }

       

        public PotenciaEnergiaModel()
        {
        }

        public PotenciaEnergiaModel(int anno, decimal Energia, decimal Potencia, int idLu, decimal importeEnergia)
        {
            this.anno = anno;
            this.Energia = Energia;
            this.Potencia = Potencia;
            this.idLu = idLu;
            this.importeEnergia = importeEnergia;
        }

        public string NuevoPotEng()
        {
            try
            {
                if(!metodos.Existe("select * from PotEnerg where anno=" + anno + "and idLu=" + idLu + ";"))
                {
                    localDao.NuevoPotEng(anno, Energia, Potencia, idLu,importeEnergia);
                    return "Guardado";
                }
                else
                {
                    return "Ya existe para Este año";
                }

            }catch(Exception ex)
            {
                return "No es posible";
            }
        }


        public string EditarPotEnergia()
        {
            try
            {
                if(!metodos.Existe("select*from PotEnerg where anno=" + anno + "and idLu=" + idLu + ";"))
                {
                    localDao.EditarPotEng(idPotEnergy, Potencia, Energia, idLu, anno, importeEnergia,true);
                    return "Modificado";
                }
                else
                {
                      localDao.EditarPotEng(idPotEnergy, Potencia, Energia, idLu, anno, importeEnergia, false);
                      return "Modificado, menos el año";
                }
                  
                 
            }
            catch(Exception ex)
            {
                return "No se ha modificado";
            }
        }


        


        public string EliminarPotEnergia(int year, int idLugar)
        {
            try                 //controlar que el año no este en electricidad activo
            {               
                if (!metodos.Existe("select * from Electricidad, Locales where idLoca= idLocal and " + year + " between year (fechaInicio) and year (fechaFin) and estado=0 and idlug="+idLugar+";")) { 
                    localDao.EliminarPotenciaEnergia(idLugar, year);
                    return "Eliminado correctamente";
                }
                else
                {
                    return "Tiene dependencias con Tiempo no cobrado";
                }

            }catch(Exception ex)
            {
                return "no se ha eliminado";
            }
        }

       

        public DataTable CargaPotenciaEnergia(int id)
        {
            return metodos.CargarGridoCmb("select idPotEnergy as id, anno as año, Energia as Energia_Total, Potencia, importeEnergia as Energia$ from PotEnerg where idLu=" + id);
        }

        public List<int> CargaYears()
        {
            return metodos.CreaLista<int>(2019, 2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027, 2028, 2029, 2030);
        }

       

    }
}
