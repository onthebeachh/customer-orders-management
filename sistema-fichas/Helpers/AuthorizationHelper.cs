using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace sistema_fichas.Helpers
{
    public static class AuthorizationHelper
    {
        public static bool isAdministrador(IPrincipal User) 
        {
            if (User.IsInRole("Administrador"))
            {
                return true;
            }
            
            return false;
        }
        
        public static bool isGerencia(IPrincipal User)
        {
            isAdministrador(User);
            if (User.IsInRole("Gerencia"))
            {
                return true;
            }

            return false;
        }

        public static bool isAdministradorComercial(IPrincipal User)
        {
            isGerencia(User);
            if (User.IsInRole("Administrador Comercial"))
            {
                return true;
            }
            
            return false;
        }

        public static bool isEjecutivoCuenta(IPrincipal User)
        {
            isAdministradorComercial(User);
            if (User.IsInRole("Ejecutivo Comercial"))
            {
                return true;
            }
            return false;
        }

    }
}