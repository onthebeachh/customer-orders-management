using sistema_fichas.Business;
using sistema_fichas.Service.Core;
using sistema_fichas.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sistema_fichas.WebApi.Controllers
{
    public class PedidoController : ApiController
    {
        IPedidoService _PedidoService;

        public PedidoController(IPedidoService PedidoService)
        {
            _PedidoService = PedidoService;
        }

        public IList<PedidoConDetalles> Get()
        {
            var pedidos = _PedidoService.GetAllByCriteria(null, null, true).ToList().Where(x => x.EstadoPedido.Estado == 5);
            List<PedidoConDetalles> pedidosConDetalles = new List<PedidoConDetalles>();
            pedidosConDetalles.Clear();


            foreach (Pedido p in pedidos)
            {
                PedidoConDetalles pedidoConDetalle = new PedidoConDetalles();
                pedidoConDetalle.Pedido_ID = p.ID;
                pedidoConDetalle.Pedido_FechaInicio = p.FechaInicio.Value;
                pedidoConDetalle.Cliente_ID = p.Cliente_ID;
                pedidoConDetalle.Cliente_NombreFantasia = p.Cliente.NombreFantasia;
                pedidoConDetalle.Cliente_RazonSocial = p.Cliente.RazonSocial;
                pedidoConDetalle.Cliente_RUT = p.Cliente.Rut;

                pedidoConDetalle.Estado_ID = p.EstadoPedido_ID.Value;
                pedidoConDetalle.Estado_Codigo = p.EstadoPedido.Estado;
                pedidoConDetalle.Estado_Nombre = p.EstadoPedido.Nombre;

                pedidoConDetalle.Ejecutivo_ID = p.UserProfile_ID;
                pedidoConDetalle.Ejecutivo_Nombre = p.UserProfile.UserName;

                IList<PedidoDetalle> actividades = null;
                actividades = p.PedidosDetalle.Where(x => x.Tipo == TipoPedidoDetalle.Actividad.GetHashCode()).ToList();

                if (actividades.Count > 0)
                {
                    pedidoConDetalle.Pedido_actividades = new List<ActividadSimple>();
                    pedidoConDetalle.Pedido_actividades = actividades.Select(x => new ActividadSimple
                    {

                        Actividad_ID = Convert.ToInt32(x.ID),
                        Actividad_Cantidad = x.Cantidad.Value,
                        Actividad_Nombre = x.Catalogo.Nombre,
                        Actividad_Fecha = x.FechaInicio.Value,
                        Estado_ID = x.EstadoDetalle_ID,
                        Estado_Nombre = x.EstadoDetalle.Nombre,
                        Moneda_ID = x.Moneda_ID,
                        Moneda_Alias = x.Moneda.Alias


                    }).ToList();

                }
                else
                {
                    pedidoConDetalle.Pedido_actividades = null;
                }
                pedidosConDetalles.Add(pedidoConDetalle);
            }

            return pedidosConDetalles.ToList();
        }

        // GET api/pedido/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/pedido
        public void Post([FromBody]string value)
        {
        }

        // PUT api/pedido/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/pedido/5
        public void Delete(int id)
        {
        }
    }
}
