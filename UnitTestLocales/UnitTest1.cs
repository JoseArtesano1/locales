using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Domain;

namespace UnitTestLocales
{
    [TestClass]
    public class UnitTest1
    {
        ElectricidadModel electricidad = new ElectricidadModel();

        [TestMethod]
        public void TestMethod()
        {            
            decimal importe = 0m;

            decimal compensacion = electricidad.TotalEnergyCompensar(lugar: "CALLE VITORIA 175",fecha1: DateTime.Parse("01/06/2019"), fecha2: DateTime.Parse("31/12/2021"), tiempo: 2.66m);
            Assert.AreEqual(importe, compensacion);


        }


        [TestMethod]
        public void TestMethod1()
        {
            decimal importe = 2m;  
            
            decimal Total = electricidad.TotalEnergiaActualizado(lugar: "CALLE VITORIA 175", fecha1: DateTime.Parse("01/06/2019"), fecha2: DateTime.Parse("31/12/2021"));
            Assert.AreEqual(importe, Total);

        }


        [TestMethod]
        public void TestMethod2()
        {
            decimal importe = 0.1m;

            decimal gasto = electricidad.GastoIndividual(idLocal: 1, idCliente:1, fecha1: DateTime.Parse("01/06/2019"), fecha2: DateTime.Parse("31/12/2021"), consumo:0.50m);
            Assert.AreEqual(importe, gasto);

        }


        [TestMethod]
        public void TestMethod4()
        {          
            decimal importe = 188.48500m;

           decimal importePotencia= electricidad.ImporteTotalPot(fecha1: DateTime.Parse ("01/06/2019"), fecha2:DateTime.Parse ("31/12/2021"), idl:1, idcl:1, lugar:"CALLE VITORIA 175", consumo:0.50m);
            Assert.AreEqual(importe, importePotencia);
           

        }



    }
}
