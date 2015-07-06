using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistema_fichas.Helpers
{
    public static class SessionHelper
    {

        public static bool isAdmin() 
        {
            bool resultado = true;
            return resultado;
        }

        public static bool isEjecutivoCuenta()
        {
            bool resultado = true;
            return resultado;
        }

        public static bool isAdministradorComercial()
        {
            bool resultado = true;
            return resultado;
        }
    }
}