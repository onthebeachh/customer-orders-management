using sistema_fichas.Business;
using sistema_fichas.Repository.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace sistema_fichas.Repository
{
    public class PedidoRepository : GenericRepository<Pedido>, IPedidoRepository
    {
        public PedidoRepository(DbContext _context) : base(_context) { }

        public Pedido GetById(int Id) {
            return _dbset
                .Include(p => p.Cliente)
                .Include(p => p.EstadoPedido)
                .Include(p => p.PedidosDetalle)
                .Include(p => p.UserProfile)
                .Include(p => p.Adjuntos)
                .Where(x => x.ID == Id)
                .FirstOrDefault();
        }

        public IEnumerable<Pedido> GetAllByClienteId(int Id, bool? OnlyActive = true)
        {
            return FindBy(x => x.Cliente_ID==Id);
        }

        public IEnumerable<Pedido> GetAll(bool? OnlyActives)
        {
            return (OnlyActives.Value) ? FindBy(x => x.EstadoPedido.Estado != null && x.EstadoPedido.Estado != 0) : GetAll();
        }

        public IEnumerable<Pedido> GetAllByCriteria(string attributeName, string attributeValue)
        {
            return FindBy(x => x.Cliente.NombreFantasia.Contains(attributeValue) || x.UserProfile.UserName.Contains(attributeValue) || x.Cliente.Rut.Contains(attributeValue));
        }
    }
}
