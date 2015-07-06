using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using sistema_fichas.Service.Core;
using sistema_fichas.Controllers;
using sistema_fichas.Business;
using Moq;
using System.Collections.Generic;

namespace sistema_fichas.Test
{
    [TestClass]
    public class PedidoControllerTest
    {
        private Mock<IPedidoService> _pedidoServiceMock;
        PedidoController objController;
        List<Pedido> listPedido;

       /* [TestInitialize]
        public void Initialize()
        {

            _pedidoServiceMock = new Mock<IPedidoService>();
            objController = new PedidoController(_pedidoServiceMock.Object);
            listCountry = new List<Country>() {
           new Country() { Id = 1, Name = "US" },
           new Country() { Id = 2, Name = "India" },
           new Country() { Id = 3, Name = "Russia" }
          };
        }
        */

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
