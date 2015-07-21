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
    public class ClienteController : ApiController
    {
        IClienteService _ClienteService;
        IPedidoService _PedidoService;

        public ClienteController(IClienteService ClienteService, IPedidoService PedidoService)
        {
            _ClienteService = ClienteService;
            _PedidoService = PedidoService;
        }


        public IList<ClienteDTO> Get()
        {
            IList<ClienteDTO> clientesConPedidos = new List<ClienteDTO>();
            IList<Cliente> clientes = _ClienteService.GetAllWithPedidos(5).ToList();

            foreach (Cliente cl in clientes)
            {
                ClienteDTO cliente = new ClienteDTO();
                cliente.ID = cl.ID;
                cliente.NombreFantasia = cl.NombreFantasia;
                cliente.RazonSocial = cl.RazonSocial;
                cliente.RUT = cl.Rut;
                clientesConPedidos.Add(cliente);
            }


            return clientesConPedidos.ToList();
        }

        // TODOS LOS PEDIDOS DE UN CLIENTE SEGUN ID CLIENTE
        public IList<PedidoDTO> Get(int id)
        {
            IEnumerable<Pedido> pedidos = _PedidoService.GetAllByClienteId(id, true).ToList();
            IList<PedidoDTO> pedidosDTO = new List<PedidoDTO>();
            
            foreach(Pedido p in pedidos)
            {
                PedidoDTO pedido = new PedidoDTO();
                pedido.ID = p.ID;
                pedido.Nombre = p.ID.ToString();
                pedidosDTO.Add(pedido);
            }
            return pedidosDTO;
        }

        // POST api/cliente
        public void Post([FromBody]string value)
        {
        }

        // PUT api/cliente/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/cliente/5
        public void Delete(int id)
        {
        }
    }
}
