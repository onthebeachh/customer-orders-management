using sistema_fichas.Business;
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

        public IEnumerable<Pedido> Get()
        {
            _db.Configuration.LazyLoadingEnabled = false;
            _db.Configuration.ProxyCreationEnabled = false;


            return _db.Pedidos.ToList();
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