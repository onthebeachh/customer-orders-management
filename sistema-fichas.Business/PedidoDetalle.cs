using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistema_fichas.Business
{
    public enum TipoPedidoDetalle
    {
        Servicio = 1,
        Producto = 2,
        Herramienta = 3,
        Actividad = 4,
        Adjunto = 5
    }

    public class PedidoDetalle : AuditableEntity<long>
    {

        [DisplayName("Cantidad")]
        public int? Cantidad { get; set; }

        [DisplayName("Valor")]
        public double? Valor { get; set; }

        [DisplayName("Tipo de Cobro")]
        public int TipoCobro { get; set; }

        [DisplayName("Comentario")]
        public String Comentario { get; set; }

        [DisplayName("Fecha de Inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = false, NullDisplayText = "Debe ingresar una fecha")]
        public DateTime? FechaInicio { get; set; }

        [DisplayName("Fecha de Termino")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = false, NullDisplayText = "Debe ingresar una fecha")]
        public DateTime? FechaTermino { get; set; }

        [DisplayName("Tipo")]
        public int? Tipo { get; set; }

        [DisplayName("Pedido")]
        public int Pedido_ID { get; set; }

        [DisplayName("Estado")]
        public int EstadoDetalle_ID { get; set; }

        [DisplayName("Catálogo")]
        public int? Catalogo_ID { get; set; }

        [DisplayName("Herramienta")]
        public int? Herramienta_ID { get; set; }

        [DisplayName("Modalidad")]
        public int? Modalidad_ID { get; set; }

        [DisplayName("MonedaCobro")]
        public int Moneda_ID { get; set; }

        [DisplayName("Catálogo")]
        [ForeignKey("Catalogo_ID")]
        public virtual Catalogo Catalogo { get; set; }

        [DisplayName("Modalidad")]
        [ForeignKey("Modalidad_ID")]
        public virtual Modalidad Modalidad { get; set; }

        [DisplayName("Herramienta")]
        [ForeignKey("Herramienta_ID")]
        public virtual Herramienta Herramienta { get; set; }

        [DisplayName("Pedido")]
        [ForeignKey("Pedido_ID")]
        public virtual Pedido Pedido { get; set; }

        [DisplayName("Estado")]
        [ForeignKey("EstadoDetalle_ID")]
        public virtual EstadoDetalle EstadoDetalle { get; set; }

        [ForeignKey("Moneda_ID")]
        public virtual Moneda Moneda { get; set; }

    }
}
