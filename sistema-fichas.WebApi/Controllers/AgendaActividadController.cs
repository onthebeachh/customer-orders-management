using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using sistema_fichas.Business;
using sistema_fichas.Service.Core;
using sistema_fichas.WebApi.ViewModels;

namespace sistema_fichas.WebApi.Controllers
{
    public class AgendaActividadController : ApiController
    {

        IPedidoDetalleService _PedidoDetalleService;

        public AgendaActividadController(IPedidoDetalleService PedidoDetalleService)
        {
            _PedidoDetalleService = PedidoDetalleService;
        }


        public IEnumerable<AgendaDTO> Get()
        {
            IList<AgendaDTO> actividades = new List<AgendaDTO>();
            return actividades;
        }

        // GET api/actividadagenda/5
        public AgendaDTO Get(int id)
        {
            PedidoDetalle pedidoDetalle = _PedidoDetalleService.GetById(id);

            AgendaDTO agendaDTO = new AgendaDTO();
            agendaDTO.Pedido_ID = pedidoDetalle.Pedido_ID;
            agendaDTO.Pedido_Nombre = pedidoDetalle.Pedido_ID.ToString();
            agendaDTO.Cliente_ID = unchecked((int)pedidoDetalle.Pedido.Cliente_ID);
            agendaDTO.Cliente_Nombre = pedidoDetalle.Pedido.Cliente.NombreFantasia;

            var actividadesNoEliminadas = _PedidoDetalleService.GetPedidosDetalleActividad(pedidoDetalle.Pedido_ID);

            IList<ActividadDTO> actividades = new List<ActividadDTO>();

            foreach (PedidoDetalle p in actividadesNoEliminadas) 
            {
                ActividadDTO a = new ActividadDTO();
                a.ID = p.ID;
                a.Nombre = p.Catalogo.Nombre;
                actividades.Add(a);
            
            }
            agendaDTO.actividades = actividades;

            return agendaDTO;
        }

        // POST api/actividadagenda
        public void Post([FromBody]string value)
        {
        }

        // PUT api/actividadagenda/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/actividadagenda/5
        public void Delete(int id)
        {
        }
    }
}
