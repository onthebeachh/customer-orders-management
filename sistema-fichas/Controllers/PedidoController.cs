using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinqKit;
using System.Net;
using WebMatrix.WebData;
using sistema_fichas.Filters;
using sistema_fichas.Business;
using sistema_fichas.ViewModels;
using sistema_fichas.Service.Core;
using sistema_fichas.Helpers;



namespace sistema_fichas.Controllers
{
    [InitializeSimpleMembership]
    public class PedidoController : SistemaFichasController
    {
        
        IPedidoService _PedidoService;
        IAdjuntoService _AdjuntoService;
        IPedidoDetalleService _PedidoDetalleService;
        ICatalogoService _CatalogoService;
        IModalidadService _ModalidadService;
        IMonedaService _MonedaService;
        IHerramientaService _HerramientaService;
        IEstadoDetalleService _EstadoDetalleService;
        IEstadoPedidoService _EstadoPedidoService;
        IClienteService _ClienteService;
        IUsuarioService _UsuarioService;

        public PedidoController(IPedidoService PedidoService, IAdjuntoService AdjuntoService, IPedidoDetalleService PedidoDetalleService, ICatalogoService CatalogoService, IModalidadService ModalidadService, IMonedaService MonedaService, IHerramientaService HerramientaService, IEstadoDetalleService EstadoDetalleService, IEstadoPedidoService EstadoPedidoService, IClienteService ClienteService, IUsuarioService UsuarioService)
        {
            this._PedidoService = PedidoService;
            this._AdjuntoService = AdjuntoService;
            this._PedidoDetalleService = PedidoDetalleService;
            this._CatalogoService = CatalogoService;
            this._ModalidadService  = ModalidadService;
            this._MonedaService = MonedaService;
            this._HerramientaService = HerramientaService;
            this._EstadoDetalleService = EstadoDetalleService;
            this._EstadoPedidoService = EstadoPedidoService;
            this._ClienteService = ClienteService;
            this._UsuarioService = UsuarioService;
        }

        public ActionResult Index(string busqueda, string tipo_filtro)
        {
            return View(_PedidoService.GetAllByCriteria(busqueda, tipo_filtro, true).ToList());
        }

        public ActionResult Create(int ClienteID)
        {
            try
            {
                PedidoViewModel pedidoVM = new PedidoViewModel();
                pedidoVM.Pedido = new Pedido();
                pedidoVM.Pedido.Cliente_ID = ClienteID;
                pedidoVM.Pedido.UserProfile_ID = (WebSecurity.CurrentUserId);
                pedidoVM.Pedido.EstadoPedido_ID = _EstadoPedidoService.GetIdEstadoInicial();
                pedidoVM.Users = _UsuarioService.GetAll();

                return PartialView("_CrearPedido", pedidoVM);
            }
            catch (Exception e) {
                return Json(new { status = false, msg = String.Format(mensaje_error, "Crear un Pedido") });
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pedido pedido)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(pedido.UserProfile_ID == null)
                        pedido.UserProfile_ID = (WebSecurity.CurrentUserId);

                    if (pedido.EstadoPedido_ID == null)
                        pedido.EstadoPedido_ID = _EstadoPedidoService.GetIdEstadoInicial();

                    _PedidoService.Create(pedido);
                    pedido = _PedidoService.GetById(pedido.ID);
                    return Json(new { status = true, msg = "Pedido creado <b>exitosamente</b>", contenido = RenderPartialViewToString("_PedidoDetalle", pedido), url = Url.Action("Details", "Pedido", new { id = pedido.ID }) });
                   
                }
                else
                    throw new Exception("Detalle: El pedido posee valores incorrectos."); 
            }
            catch (Exception e)
            {
                PedidoViewModel pedidoVM = new PedidoViewModel();
                pedidoVM.Clientes = _ClienteService.GetAll();
                return View(pedidoVM);
            }
        }

        public ActionResult Details(int id)
        {

            PedidoViewModel PedidoVM = new PedidoViewModel();
            try {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Pedido pedido = _PedidoService.GetById(id);
                if (pedido == null)
                {
                    return HttpNotFound();
                }

                PedidoVM.Pedido = pedido;
                PedidoVM.PedidosDetalles = PedidoVM.Pedido.PedidosDetalle.Where(s => s.EstadoDetalle.Estado != TipoEstadoDetalle.Inactivo.GetHashCode()) as IEnumerable<PedidoDetalle>;
                return View(PedidoVM);
            
            }catch(Exception e){
                return View(PedidoVM);
            }
            
        }

        private Boolean ValidarEstado(int Estado_ID) {

            int pedido = _PedidoService.GetById(Estado_ID).EstadoPedido.Estado;
           
            
            return false;
        }

        public ActionResult AgregarServicio(int PedidoID)
        {
            PedidoViewModel PedidoVM = new PedidoViewModel();
            DateTime fecha = DateTime.Now;
            DateTime fecha_fin = fecha.AddMonths(6);
            try
            {
                PedidoVM.Productos = _CatalogoService.GetAllByType(TipoPedidoDetalle.Servicio.GetHashCode());
                PedidoVM.Monedas = _MonedaService.GetAll();

                if (Request.IsAjaxRequest())
                {
                    PedidoVM.PedidoDetalle.Pedido_ID = PedidoID;
                    PedidoVM.PedidoDetalle.FechaInicio = fecha;
                    PedidoVM.PedidoDetalle.FechaTermino = fecha_fin;

                    PedidoVM.PedidoDetalle.Tipo = TipoPedidoDetalle.Servicio.GetHashCode();
                    return PartialView("_AgregarServicio", PedidoVM);
                }
                return View();
            }
            catch (Exception e)
            {
                return PartialView("_AgregarServicio", PedidoVM);
            }

            
        }

        public ActionResult AgregarAdjunto(int PedidoID)
        {

            Adjunto Adjunto = new Adjunto();

            if (Request.IsAjaxRequest())
            {
                Adjunto.Pedido_ID = PedidoID;
                Adjunto.Version = 1;
                return PartialView("_AgregarAdjunto", Adjunto);
            }
            return View(Adjunto);

        }

        public ActionResult AgregarHerramienta(int PedidoID) {

            PedidoViewModel ProductoVM = new PedidoViewModel();
            ProductoVM.Herramientas = _HerramientaService.GetAll();
            ProductoVM.Modalidades = _ModalidadService.GetAll();
            ProductoVM.Monedas = _MonedaService.GetAll();
            try
            {
                if (Request.IsAjaxRequest())
                {
                    ProductoVM.PedidoDetalle.Pedido_ID = PedidoID;
                    ProductoVM.PedidoDetalle.Tipo = TipoPedidoDetalle.Herramienta.GetHashCode();
                    return PartialView("_AgregarHerramienta", ProductoVM);
                }
                return View(ProductoVM);

            }
            catch (Exception e)
            {
                return View(e.Message);
            }
        
        }

        public ActionResult AgregarActividad(int PedidoID)
        {
            PedidoViewModel ProductoVM = new PedidoViewModel();
            ProductoVM.Productos = _CatalogoService.GetAllByType(TipoPedidoDetalle.Actividad.GetHashCode());
            ProductoVM.Monedas = _MonedaService.GetAll();
            try
            {
                if (Request.IsAjaxRequest())
                {
                    ProductoVM.PedidoDetalle.Pedido_ID = PedidoID;
                    ProductoVM.PedidoDetalle.Tipo = TipoPedidoDetalle.Actividad.GetHashCode();
                    return PartialView("_AgregarActividad", ProductoVM);
                }
                return View(ProductoVM);
            }
            catch (Exception e)
            {
                return View(e.Message);
            }
        }
        
        public ActionResult AgregarProducto(int PedidoID)
        {
            PedidoViewModel ProductoVM = new PedidoViewModel();
            ProductoVM.Productos = _CatalogoService.GetAllByType(TipoPedidoDetalle.Producto.GetHashCode());
            ProductoVM.Modalidades = _ModalidadService.GetAll();
            ProductoVM.Monedas = _MonedaService.GetAll();
            try
            {
                if (Request.IsAjaxRequest())
                {
                    ProductoVM.PedidoDetalle.Pedido_ID = PedidoID;
                    ProductoVM.PedidoDetalle.Tipo = TipoPedidoDetalle.Producto.GetHashCode();
                    return PartialView("_AgregarProducto", ProductoVM);
                }
                return View(ProductoVM);
            }
            catch (Exception e)
            {
                return View(e.Message);
            }
        }

        public ActionResult EditPedidoDetalle(int Detalle_ID, int TipoDetalle)
        {
            
            PedidoViewModel PedidoVM = new PedidoViewModel();
            PedidoVM.PedidoDetalle = _PedidoDetalleService.GetById(Detalle_ID); // db.PedidosDetalle.Find(Detalle_ID);
            PedidoVM.Modalidades = _ModalidadService.GetAll(); // db.Modalidades.Where(m => m.Estado == 1);
            PedidoVM.TipoDetalle = TipoDetalle.ToString();
            PedidoVM.Monedas = _MonedaService.GetAll(); // db.Monedas.Where(m => m.Estado == 1);
            var tipoPedidoDetalle = "";
            try
            {
                
                switch (TipoDetalle)
                {
                    case 1:
                        PedidoVM.Productos = _CatalogoService.GetAllByType(TipoPedidoDetalle.Servicio.GetHashCode());
                        tipoPedidoDetalle = "_AgregarServicio";
                        break;
                    case 2:
                        PedidoVM.Productos = _CatalogoService.GetAllByType(TipoPedidoDetalle.Producto.GetHashCode());
                        tipoPedidoDetalle = "_AgregarProducto";
                        break;
                    case 3:
                        PedidoVM.Herramientas = _HerramientaService.GetAll();
                        tipoPedidoDetalle = "_AgregarHerramienta";
                        break;
                    case 4:
                        PedidoVM.Productos = _CatalogoService.GetAllByType(TipoPedidoDetalle.Actividad.GetHashCode());
                        tipoPedidoDetalle = "_AgregarActividad";
                        break;
                    case 5:
                        tipoPedidoDetalle = "_AgregarAdjunto";
                        break;
                    default:
                        break;

                }
                return PartialView(tipoPedidoDetalle, PedidoVM);

            }
            catch (Exception e)
            {
                return PartialView("_Error", e.Message);
            }
        }

        public ActionResult VerPedidoDetalle(int PedidoDetalleID, int TipoPedidoDetalle) {

            PedidoDetalle pedido = new PedidoDetalle();
            
            try{
                pedido = _PedidoDetalleService.GetById(PedidoDetalleID);
                return PartialView("_VerPedidoDetalle", pedido);

            }
            catch (Exception e)
            {
                return PartialView("_Error", e.Message);
            }
            return PartialView();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditPedidoDetalle(PedidoDetalle PedidoDetalle)
        {
            var tipoPedidoDetalle = "";
            var TipoDetalle = ""; 
            IEnumerable<object> Pedidos = null;
            int PedidoDetalle_ID = PedidoDetalle.Pedido_ID;
            try
            {
                if (ModelState.IsValid)
                {
                    _PedidoDetalleService.Update(PedidoDetalle);
                    switch (PedidoDetalle.Tipo)
                {
                    case 1:
                        tipoPedidoDetalle = "_ServiciosList";
                        TipoDetalle = TipoPedidoDetalle.Servicio.ToString().ToLower()+"s";
                        Pedidos = _PedidoDetalleService.GetPedidosDetalleServicio(PedidoDetalle_ID);
                        break;
                    case 2:
                        tipoPedidoDetalle = "_ProductosList";
                        TipoDetalle = TipoPedidoDetalle.Producto.ToString().ToLower() + "s";
                        Pedidos = _PedidoDetalleService.GetPedidosDetalleProducto(PedidoDetalle_ID);
                        break;
                    case 3:
                        tipoPedidoDetalle = "_HerramientaList";
                        TipoDetalle = TipoPedidoDetalle.Herramienta.ToString().ToLower() + "s";
                        Pedidos = _PedidoDetalleService.GetPedidosDetalleHerramienta(PedidoDetalle_ID);
                        break;
                    case 4:
                        tipoPedidoDetalle = "_ActividadesList";
                        TipoDetalle = TipoPedidoDetalle.Actividad.ToString().ToLower() + "es";
                        Pedidos = _PedidoDetalleService.GetPedidosDetalleActividad(PedidoDetalle_ID);
                        break;
                    case 5:
                        tipoPedidoDetalle = "_AdjuntoList";
                        Pedidos = _AdjuntoService.GetAll();/*db.Adjuntos
                        .Where(s => s.PedidoID == PedidoDetalle.PedidoID)
                        .ToList();*/
                        break;
                    
                }

                    return Json(new { status = status_success, msg = String.Format("Detalle actualizado <b>exitosamente</b>"), tipo = TipoDetalle,  contenido = RenderPartialViewToString(tipoPedidoDetalle, Pedidos) });
                }
                else
                {
                    throw new Exception("Detalle: El Pedido detalle posee valores incorrectos"); 
                }

            }
            catch (Exception e)
            {
                return Json(new { status = status_error, msg = String.Format(mensaje_error, "Actualizar el detalle") + System.Environment.NewLine + e.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarHerramienta(PedidoDetalle PedidoDetalle) {
            var msg_error = String.Format(mensaje_error,"agregar una herramienta");
            try {
                if(ModelState.IsValid){

                    int EstadoDetalle_ID = _EstadoDetalleService.GetIdEstadoInicial();
                    int TipoPedidoDetalle_Id = TipoPedidoDetalle.Herramienta.GetHashCode();


                    PedidoDetalle.Tipo = TipoPedidoDetalle_Id;
                    PedidoDetalle.EstadoDetalle_ID = EstadoDetalle_ID;
                    _PedidoDetalleService.Create(PedidoDetalle);

                    var Pedidos = _PedidoDetalleService.GetPedidosDetalleHerramienta(PedidoDetalle.Pedido_ID);
                    return Json(new { msg = "Detalle creado <b>exitosamente</b>", status = status_success, contenido = RenderPartialViewToString("_HerramientaList", Pedidos) });
                }

                return Json(new {status = status_error, msg = msg_error});
            }
            catch (Exception e) {
                return Json(new { status = status_error, msg = msg_error + System.Environment.NewLine + e.Message });
            }
        
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarServicio(PedidoDetalle PedidoDetalle)
        {

            try {
                if (ModelState.IsValid)
                {
                    int EstadoDetalle_ID = _EstadoDetalleService.GetIdEstadoInicial();
                    int TipoPedidoDetalle_Id = TipoPedidoDetalle.Servicio.GetHashCode();


                    PedidoDetalle.Tipo = TipoPedidoDetalle_Id;
                    PedidoDetalle.EstadoDetalle_ID = EstadoDetalle_ID;

                    _PedidoDetalleService.Create(PedidoDetalle);
                    var Pedidos = _PedidoDetalleService.GetPedidosDetalleServicio(PedidoDetalle.Pedido_ID);

                    return Json(new { msg = "Detalle creado <b>exitosamente</b>", status = status_success, contenido = RenderPartialViewToString("_ServiciosList", Pedidos) });
                }
                else {
                    return Json(new { msg = "Ocurrio un error, por favor intente de nuevo", status = status_error });
                }

            }catch(Exception e){
                return Json(new { msg = e.Message, status = false});
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarActividad(PedidoDetalle PedidoDetalle)
        {

            try
            {
                //if(!_PedidoService.IsEditable(PedidoDetalle.Pedido_ID))
                //    throw new Exception("Detalle: Este pedido ya no puede ser actualizado"); 

                if (ModelState.IsValid)
                {
                    int EstadoDetalle_ID = _EstadoDetalleService.GetIdEstadoInicial();
                    int TipoPedidoDetalle_Id = TipoPedidoDetalle.Actividad.GetHashCode();


                    PedidoDetalle.Tipo = TipoPedidoDetalle_Id;
                    PedidoDetalle.EstadoDetalle_ID = EstadoDetalle_ID;
                    _PedidoDetalleService.Create(PedidoDetalle);

                    var Pedidos = _PedidoDetalleService.GetPedidosDetalleActividad(PedidoDetalle.Pedido_ID);
                    return Json(new { msg = "Detalle creado <b>exitosamente</b>", status = status_success, contenido = RenderPartialViewToString("_ActividadesList", Pedidos) });
                }
                else
                {
                    return Json(new { msg = "Ocurrio un error, por favor intente de nuevo", status = status_error });
                }

            }
            catch (Exception e)
            {
                return Json(new { msg = "Ocurrio un error, por favor intente de nuevo" + System.Environment.NewLine + e.Message, status = false });
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarProducto(PedidoDetalle PedidoDetalle)
        {

            try {
                if (ModelState.IsValid)
                {
                    int EstadoDetalle_ID = _EstadoDetalleService.GetIdEstadoInicial();
                    int TipoPedidoDetalle_Id = TipoPedidoDetalle.Producto.GetHashCode();


                    PedidoDetalle.Tipo = TipoPedidoDetalle_Id;
                    PedidoDetalle.EstadoDetalle_ID = EstadoDetalle_ID;
                    _PedidoDetalleService.Create(PedidoDetalle);

                    var Pedidos = _PedidoDetalleService.GetPedidosDetalleProducto(PedidoDetalle.Pedido_ID);
                    return Json(new { msg = "Detalle creado <b>exitosamente</b>", status = status_success, contenido = RenderPartialViewToString("_ProductosList", Pedidos) });
                }
                else
                {
                    return Json(new { msg = "Ocurrio un error, por favor intente de nuevo", status = status_error });
                }
                
            }
            catch (Exception e) {
                return Json(new { msg = "Ocurrio un error, por favor intente de nuevo" + System.Environment.NewLine + e.Message, status = false });
            }
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarAdjunto(Adjunto Adjunto)
        {
            try
            {
                string msg;
                Adjunto.FechaHora = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        msg = "Adjunto creado <b>exitósamente</b>";
                        _AdjuntoService.Create(Adjunto);
                        var Adjuntos = _AdjuntoService.GetAll();
                        return Json(new { msg = "Detalle creado <b>exitosamente</b>", status = status_success, contenido = RenderPartialViewToString("_AdjuntoList", Adjuntos) });
                    }
                    else{
                        msg = "No se pudo guardar la información exitósamente, intente de nuevo.";
                        return Json(new { msg = msg , status = status_error });
                    }
            }
            catch(Exception e)
            {
                return Json(new { status = false, msg = e.Message });
            }
        }

        public ActionResult Edit(int PedidoID)
        {
            if (PedidoID == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Pedido pedido = _PedidoService.GetById(PedidoID);
            if (pedido == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest()) {
                return PartialView("_Editar",pedido);
            }

            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Pedido pedido)
        {
            String msg_error = "Ocurrio un problema al intentar actualizar el pedido, por favor intente de nuevo.";
            String msg_success = "El Pedido ha sido actualizado <strong>exitosamente</strong>";
            try
            {
                if (ModelState.IsValid) {
                    _PedidoService.Update(pedido);
                    pedido = _PedidoService.GetById(pedido.ID);
                    return Json(new { status = status_success, msg = msg_success, contenido = RenderPartialViewToString("_PedidoDetalle",pedido) });
                } else {
                    throw new Exception("Detalle: El pedido contiene valores incorrectos."); 
                }
            }
            catch(Exception e)
            {
                return Json(new { status = status_error, msg = msg_error + System.Environment.NewLine + e.Message });
            }
        }

        [HttpPost]
        public ActionResult CambiarEstado(int PedidoID, int Estado)
        {
            String msg_error = "Ocurrio un problema al intentar activar el pedido, por favor intente de nuevo.";
            String msg_success = "El Pedido ha sido actualizado exitosamente";
            int EstadoID = 1;

            try
            {
                if (PedidoID == null)
                    throw new Exception("Detalle: Debe especificar una ID de pedido."); 

                Pedido Pedido = _PedidoService.GetById(PedidoID);

                if (Pedido == null)
                    throw new Exception("Detalle: El pedido no se encuentra en la BD."); 

                switch (Estado) {
                    case 0:
                        EstadoID = _EstadoPedidoService.GetIdEstadoInactivo(); 
                        break;
                    case 1:
                        EstadoID = _EstadoPedidoService.GetIdEstadoInicial();
                        break;
                    case 2:
                        EstadoID = _EstadoPedidoService.GetIdEstadoRevisionComercial();
                        break;
                    case 3:
                        EstadoID = _EstadoPedidoService.GetIdEstadoAprobadoComercial();
                        break;
                    case 4:
                        EstadoID = _EstadoPedidoService.GetIdEstadoRechazadoComercial();
                        break;
                }
                if (EstadoID.Equals(0))
                    throw new Exception("Detalle: El estado seleccionado no se encuentra en la BD."); 
                
                Pedido.EstadoPedido_ID = EstadoID;
                _PedidoService.Update(Pedido);
                return Json(new { status = status_success, msg = msg_success, url = Url.Action("Details", "Pedido", new { ID = PedidoID }), contenido = RenderPartialViewToString("~/Views/Pedido/_PedidoDetalle.cshtml", Pedido), contenidoBotones = RenderPartialViewToString("~/Views/Pedido/_BotonesPedidoDetalle.cshtml", Pedido) });
            }
            catch (Exception e)
            {
                return Json(new { status = status_error, msg = msg_error + System.Environment.NewLine + e.Message });
            };

        }

        [HttpPost]
        public ActionResult EliminarHerramienta(int PedidoDetalleID)
        {
            String msg_error = "Ocurrio un problema al intentar borrar la Herramienta, por favor intente de nuevo.";
            String msg_success = "La herramienta ha sido borrada exitosamente";

            try
            {
                if (PedidoDetalleID == null)
                    throw new Exception("Detalle: Debe especificar una ID de herramienta"); 

                PedidoDetalle PedidoDetalle = _PedidoDetalleService.GetById(PedidoDetalleID);
                if (PedidoDetalle == null)
                    throw new Exception("Detalle: la Herramienta no se encuentra en la BD."); 

                int EstadoDetalle_ID = _EstadoDetalleService.GetIdEstadoInactivo();

                if (EstadoDetalle_ID != null)
                {
                    PedidoDetalle.EstadoDetalle_ID = EstadoDetalle_ID;
                    _PedidoDetalleService.Update(PedidoDetalle);
                    return Json(new { status = status_success, msg = msg_success });

                }
                else
                    throw new Exception("Detalle: Debe especificar una ID de herramienta"); 

            }
            catch (Exception e)
            {
                return Json(new { status = status_error, msg = msg_error + System.Environment.NewLine + e.Message });
            }

        }

        [HttpPost]
        public ActionResult EliminarAdjunto(int AdjuntoID)
        {
            String msg_error = "Ocurrio un problema al intentar borrar el archivo, por favor intente de nuevo.";
            String msg_success = "El Archivo adjunto ha sido borrado exitosamente";

            try {
                if (AdjuntoID == null)
                {
                    return Json(new { status = status_error, msg = msg_error });
                }
                Adjunto adjunto = _AdjuntoService.GetById(AdjuntoID); 
                if (adjunto == null)
                {
                    return Json(new { status = status_error, msg = msg_error });
                }

                adjunto.Estado = 0;
                _AdjuntoService.Update(adjunto);
                return Json(new { status = status_success, msg = msg_success});

            } catch(Exception e){
                return Json(new { status = status_error, msg = msg_error + System.Environment.NewLine + e.Message });
            };
            
        }

        [HttpPost]
        public ActionResult EliminarDetallePedido(int PedidoDetalleID)
        {

            String msg_error = "Ocurrio un problema al intentar borrar el servicio, por favor intente de nuevo.";
            String msg_success = "El Detalle ha sido borrado exitosamente";

            try
            {
                if (PedidoDetalleID == null)
                {
                    throw new Exception("Detalle: Debe especificar una ID del Pedido Detalle"); 
                }
                PedidoDetalle PedidoDetalle = _PedidoDetalleService.GetById(PedidoDetalleID);
                int EstadoDetalle_ID = _EstadoDetalleService.GetIdEstadoInactivo();


                if (PedidoDetalle == null)
                    throw new Exception("Detalle: El Detalle no existe.");

                if (PedidoDetalle == null)
                    throw new Exception("Detalle: No Existe un estado inactivo en la BD.");
                
                
                PedidoDetalle.EstadoDetalle_ID = EstadoDetalle_ID;
                _PedidoDetalleService.Update(PedidoDetalle);
                
                return Json(new { status = status_success, msg = msg_success });
                
            }
            catch (Exception e)
            {
                return Json(new { status = status_error, msg = msg_error + System.Environment.NewLine + e.Message });
            }

        }

    }
}
