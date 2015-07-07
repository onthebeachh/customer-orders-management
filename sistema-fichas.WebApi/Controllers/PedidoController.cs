using sistema_fichas.Business;
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

        FichasContext _db = new FichasContext();

        public IList<PedidoConDetalles> Get()
        {
            _db.Configuration.LazyLoadingEnabled = false;
            _db.Configuration.ProxyCreationEnabled = false;

            IEnumerable<Pedido> pedidos = _db.Pedidos
                .Where(x => x.EstadoPedido.Estado == 5)
                .Include(x => x.PedidosDetalle)
                .Include(x => x.Cliente)
                .Include(x => x.UserProfile)
                .Include(x => x.EstadoPedido)
                .ToList();

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
                        //Actividad_Nombre = x.Catalogo.Nombre,
                        Actividad_Fecha = x.FechaInicio.Value,
                        Estado_ID = x.EstadoDetalle_ID,
                        //Estado_Nombre = x.EstadoDetalle.Nombre,
                        Moneda_ID = x.Moneda_ID,
                        //Moneda_Alias = x.Moneda.Alias


                    }).ToList();

                }
                else {
                    pedidoConDetalle.Pedido_actividades = null;
                }
                pedidosConDetalles.Add(pedidoConDetalle);
            }

            return pedidosConDetalles.ToList();
        }

        public Pedido Get(int id)
        {
            Pedido pedido = _db.Pedidos.Find(id);
            return pedido;
        }

        public void Post(Pedido pedido)
        {
            if (ModelState.IsValid) 
            {
                _db.Pedidos.Add(pedido);
                _db.SaveChanges();
            }

        }

        public void Put(int id, int estadoPedido)
        {

            
            if (estadoPedido != null) 
            {
                Pedido pedido = _db.Pedidos.Find(id);
                pedido.EstadoPedido_ID = estadoPedido;
                _db.Entry(pedido).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        // DELETE api/values/5
        public void Delete(int id)
        {

        }
    }
}