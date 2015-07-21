using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using sistema_fichas.Business;

namespace sistema_fichas.Helpers
{
    public static class PedidoHelper
    {

        public static IEnumerable<PropertyInfo> getPropertyNames()
        {
            var resultado = typeof(sistema_fichas.Business.Pedido).GetProperties().Where(p => !p.GetGetMethod().IsVirtual && !p.Name.Contains("ID"));
            return resultado.ToList();
        }

        public static SelectList getPropertyNamesAsSelectList()
        {


            //var select_customFilters =
            IEnumerable<SelectListItem> select_customFilters = (new[] { new SelectListItem { Text = "Nombre Fantasia", Value = "Cliente.NombreFantasia" }, new SelectListItem { Text = "Ejecutivo", Value = "UserProfile.UserName" }});
            IEnumerable<SelectListItem> select_properties = 
                getPropertyNames().Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Name
                });
            var select_mixed = select_customFilters.Concat(select_properties);
            return new SelectList(select_mixed,"Value","Text");
        }        
    
        public static bool esValido(int pedido)
        {
            return (pedido == TipoEstadoPedido.Inactivo.GetHashCode()) ? true : false;
        }
    }
}