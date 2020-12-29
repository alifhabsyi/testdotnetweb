using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Akademik.Models;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Akademik.Controllers
{
    public class DosenController : Controller
    {
        // GET: Dosen
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData()
        {
            using (DBModels db = new DBModels())
            {
                List<DOSEN> DSNLIST = db.DOSENs.ToList<DOSEN>();
                return Json(new { data = DSNLIST }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddOrEdit(string id = "")
        {
            if (id == "")
                return View(new DOSEN());
            else
            {
                using (DBModels db = new DBModels())
                {
                    return View(db.DOSENs.Where(x => x.NIP == id).FirstOrDefault<DOSEN>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(DOSEN emp)
        {
            using (DBModels db = new DBModels())
            {
                if (Request.Form["hm"] == "0")
                {
                    if (db.DOSENs.Find(emp.NIP) is null)
                    {
                        try
                        {
                            db.DOSENs.Add(emp);
                            db.SaveChanges();
                            return Json(new { success = true, message = "Data Berhasil Disimpan!" }, JsonRequestBehavior.AllowGet);

                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {
                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                    return Json(new { success = true, message = "Data Gagal Disimpan!" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }

                    }
                    return Json(new { success = true, message = "Data Sudah Ada!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(emp).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Data Berhasil Diupdate!" }, JsonRequestBehavior.AllowGet);
                }

            }


        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            using (DBModels db = new DBModels())
            {
                DOSEN emp = db.DOSENs.Where(x => x.NIP == id).FirstOrDefault<DOSEN>();
                db.DOSENs.Remove(emp);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}