using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;



namespace sistema_fichas.Helpers
{
    public static class ToolboxHelperExtension 
    {

        public static MvcHtmlString BSbutton(this HtmlHelper helper, string btnClass, string btnContent, string iconName)
        {
            var icon = String.Empty;

            if (btnClass == null)
            {
                btnClass = String.Format("btn-default");
            }
            if (btnContent == null)
            {
                btnContent = String.Empty;
            }
            if (iconName != null) 
            {
                icon = String.Format("<span class='fa fa-{0}'> </span>",iconName);
            }

            return new MvcHtmlString(String.Format("<button class='btn btn-{0}'>{1} {2}</button>", btnClass, icon, btnContent));
        }

        public static MvcHtmlString botonVer(this HtmlHelper helper, int tipo, string dataId)
        {
            var icon = String.Empty;
            icon = String.Format("<i class='col-xs-offset-1 fa-eye fa ver_detalle' data-id='{0}' data-type='{1}'></i>", dataId, tipo);
            return new MvcHtmlString(icon);
        }

        public static MvcHtmlString botonEditar(this HtmlHelper helper, int tipo, string dataId)
        {
            var icon = String.Empty;
            icon = String.Format("<i class='col-xs-offset-1 fa-pencil fa editar_detalle' data-id='{0}' data-type='{1}'> </i>", dataId, tipo);

            return new MvcHtmlString(icon);
        }

        public static MvcHtmlString botonEliminar(this HtmlHelper helper, int tipo, string dataId)
        {
            var icon = String.Empty;
            icon = String.Format("<i class='col-xs-offset-1 fa-trash fa borrar_detalle' data-id='{0}' data-type='{1}'></i>", dataId, tipo);
            return new MvcHtmlString(icon);
        }

        public static MvcHtmlString If(this MvcHtmlString value, bool authorization)
        {
            return authorization ? value : MvcHtmlString.Empty;
        }

    }
}