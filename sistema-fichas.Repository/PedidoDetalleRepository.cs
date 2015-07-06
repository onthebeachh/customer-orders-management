using sistema_fichas.Business;
using sistema_fichas.Repository.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistema_fichas.Repository
{
    public class PedidoDetalleRepository : GenericRepository<PedidoDetalle>, IPedidoDetalleRepository
    {
        public PedidoDetalleRepository(DbContext _context) : base(_context) { }

        public PedidoDetalle GetById(int Id) {
            
            return FindBy(x => x.ID == Id).FirstOrDefault();
        }

        public IEnumerable<PedidoDetalle> GetAllByPedidoId(int Pedido_ID, int? TipoPedidoDetalle_ID, bool? OnlyActives=true)
        {
            IEnumerable<PedidoDetalle> resultado = null;
            if (TipoPedidoDetalle_ID == null){
                resultado= FindBy(pd => pd.Pedido_ID == Pedido_ID);
            }else {
                resultado = _dbset
                    .Include(p => p.Modalidad)
                    .Include(p => p.Herramienta)
                    .Include(p => p.Catalogo)
                    .Include(p => p.Moneda)
                    .Include(p => p.EstadoDetalle)
                    .Where(pd => pd.Pedido_ID == Pedido_ID && pd.Tipo == TipoPedidoDetalle_ID && pd.EstadoDetalle.Estado != 0).AsEnumerable();
            }

            if (OnlyActives.Value == true)
                resultado.Where(pd => pd.EstadoDetalle.Estado != 0);

            return resultado;
        }
    }
}
