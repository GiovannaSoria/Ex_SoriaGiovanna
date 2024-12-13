using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BLL;
using Entities;

namespace NWind.MVCPLS.Controllers
{
    public class HomeController : Controller
    {
        private readonly MedicosLogic _logic = new MedicosLogic();

        // Redirige a la lista de médicos al acceder a la raíz
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        // Muestra la lista de médicos
        public ActionResult List()
        {
            try
            {
                var medicos = _logic.RetrieveAll();
                return View("MedicosList", medicos);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al recuperar la lista de médicos: " + ex.Message;
                return View("MedicosList", new List<Medicos>());
            }
        }

        // Muestra los detalles de un médico
        public ActionResult Details(int id)
        {
            try
            {
                var medico = _logic.RetrieveById(id);
                if (medico == null)
                {
                    ViewBag.ErrorMessage = "Médico no encontrado.";
                    return RedirectToAction("List");
                }
                return View("MedicosDetails", medico);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al recuperar los detalles del médico: " + ex.Message;
                return RedirectToAction("List");
            }
        }

        // Muestra el formulario para crear o editar un médico
        public ActionResult CUDForm(int? id = null)
        {
            try
            {
                Medicos model = id.HasValue ? _logic.RetrieveById(id.Value) : new Medicos();
                if (model == null && id.HasValue)
                {
                    ViewBag.ErrorMessage = "Médico no encontrado.";
                    return RedirectToAction("List");
                }
                return View("MedicosCUD", model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al cargar el formulario: " + ex.Message;
                return RedirectToAction("List");
            }
        }

        // Maneja las operaciones de creación, actualización y eliminación de un médico
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CUD(Medicos model, string CreateBtn, string UpdateBtn, string DeleteBtn)
        {
            try
            {
                if (!string.IsNullOrEmpty(CreateBtn))
                {
                    _logic.Create(model);
                    TempData["SuccessMessage"] = "Médico creado exitosamente.";
                }
                else if (!string.IsNullOrEmpty(UpdateBtn))
                {
                    _logic.Update(model);
                    TempData["SuccessMessage"] = "Médico actualizado exitosamente.";
                }
                else if (!string.IsNullOrEmpty(DeleteBtn))
                {
                    if (_logic.Delete(model.MedicoID))
                    {
                        TempData["SuccessMessage"] = "Médico eliminado exitosamente.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "No se puede eliminar el médico.";
                    }
                }
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error durante la operación: " + ex.Message;
                return View("MedicosCUD", model);
            }
        }

        // Elimina un médico por su ID
        public ActionResult Delete(int id)
        {
            try
            {
                var medico = _logic.RetrieveById(id);
                if (medico == null)
                {
                    TempData["ErrorMessage"] = "Médico no encontrado.";
                    return RedirectToAction("List");
                }

                if (_logic.Delete(id))
                {
                    TempData["SuccessMessage"] = "Médico eliminado exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se puede eliminar el médico.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al eliminar el médico: " + ex.Message;
            }
            return RedirectToAction("List");
        }
    }
}
