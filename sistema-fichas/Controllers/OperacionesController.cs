using sistema_fichas.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sistema_fichas.Controllers
{
    public class OperacionesController : Controller
    {
        IPedidoService _PedidoService;

        public OperacionesController(IPedidoService PedidoService)
        {
            _PedidoService = PedidoService; 
        }

        public ActionResult Index(string busqueda, string tipo_filtro)
        {
            return View(_PedidoService.GetAllByCriteria(busqueda, tipo_filtro, true).ToList());
        }

        //
        // GET: /Operaciones/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Operaciones/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Operaciones/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Operaciones/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Operaciones/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Operaciones/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Operaciones/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
