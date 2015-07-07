using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistema_fichas.WebApi.ViewModels
{
    public class ActividadSimple
    {
        public int Actividad_ID { get; set; }
        public int Actividad_Cantidad { get; set; }
        public string Actividad_Nombre { get; set; }
        public DateTime Actividad_Fecha { get; set; }

        public int Estado_ID { get; set; }
        public string Estado_Nombre { get; set; }

        public int Moneda_ID { get; set; }
        public string Moneda_Alias { get; set; }
         
    }
}