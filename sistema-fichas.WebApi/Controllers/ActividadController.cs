using sistema_fichas.Business;
using sistema_fichas.Service.Core;
using sistema_fichas.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sistema_fichas.WebApi.Controllers
{
    public class ActividadController : ApiController
    {
        IPedidoDetalleService _PedidoDetalleService;

        public ActividadController(IPedidoDetalleService PedidoDetalleService) 
        {
            _PedidoDetalleService = PedidoDetalleService;
        }

        public IEnumerable<ActividadDTO> Get()
        {
            return new List<ActividadDTO>();
        }


        public ActividadDetalleDTO Get(int id)
        {
            var pedidoDetalle = _PedidoDetalleService.GetById(id);
            var actividad = new ActividadDetalleDTO();

            actividad.Actividad_ID = unchecked((int)pedidoDetalle.ID);
            actividad.Actividad_Nombre = pedidoDetalle.Catalogo.Nombre;
            actividad.Actividad_Cantidad = pedidoDetalle.Cantidad.Value;
            actividad.Actividad_Fecha = pedidoDetalle.FechaInicio.Value;
            actividad.Actividad_Valor = pedidoDetalle.Valor.Value;

            actividad.Estado_Nombre = pedidoDetalle.EstadoDetalle.Nombre;
            actividad.Moneda_Alias = pedidoDetalle.Moneda.Alias;

            
            actividad.Cliente_ID = unchecked((int)pedidoDetalle.Pedido.Cliente_ID);
            actividad.Cliente_nombreFantasia = pedidoDetalle.Pedido.Cliente.NombreFantasia;

            actividad.Pedido_ID = pedidoDetalle.Pedido_ID;
            actividad.Pedido_Nombre = pedidoDetalle.Pedido_ID.ToString();

            return actividad;
        }


        // POST api/actividad
        public void Post([FromBody] EstadoDTO estado)
        {
            PedidoDetalle actividad = _PedidoDetalleService.GetById(estado.id);
            
            if(estado.estado == TipoEstadoDetalle.Activo.GetHashCode())
                actividad.EstadoDetalle_ID = 2;

            if (estado.estado == TipoEstadoDetalle.Agendado.GetHashCode())
                actividad.EstadoDetalle_ID = 3;

            if (estado.estado == TipoEstadoDetalle.Finalizado.GetHashCode())
                actividad.EstadoDetalle_ID = 5;

            if (estado.estado == TipoEstadoDetalle.Parcial.GetHashCode())
                actividad.EstadoDetalle_ID = 4;
            
            _PedidoDetalleService.Update(actividad);

        }

        // PUT api/actividad/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/actividad/5
        public void Delete(int id)
        {
        }
    }
}
