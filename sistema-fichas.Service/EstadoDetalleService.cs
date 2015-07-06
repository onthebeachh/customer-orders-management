using sistema_fichas.Business;
using sistema_fichas.Repository.Core;
using sistema_fichas.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistema_fichas.Service
{
    public class EstadoDetalleService : EntityService<EstadoDetalle>, IEstadoDetalleService
    {
        IUnitOfWork _unitOfWork;
        IEstadoDetalleRepository _estadoDetalleRepository;
        
        public EstadoDetalleService(IUnitOfWork unitOfWork, IEstadoDetalleRepository estadoDetalleRepository) 
            : base(unitOfWork, estadoDetalleRepository) {
                _unitOfWork = unitOfWork;
                _estadoDetalleRepository = estadoDetalleRepository;
        }

        public EstadoDetalle GetById(int Id)
        {
            return _estadoDetalleRepository.GetById(Id);
        }

        public int GetIdEstadoInicial()
        {
            return _estadoDetalleRepository.getIdEstado("Activo");
        }

        public int GetIdEstadoInactivo() 
        {
            return _estadoDetalleRepository.getIdEstado("Inactivo");
        }
    }
}
