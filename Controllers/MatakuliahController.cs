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
    public class MatakuliahController : Controller
    {
        // GET: MataKuliah
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData()
        {
            using (DBModels db = new DBModels())
            {
                List<MATAKULIAH> MKLIST = db.MATAKULIAHs.ToList<MATAKULIAH>();
                return Json(new { data = MKLIST }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddOrEdit(string id = "")
        {
            if (id == "")
                return View(new MATAKULIAH());
            else
            {
                using (DBModels db = new DBModels())
                {
                    return View(db.MATAKULIAHs.Where(x => x.KODE_MK == id).FirstOrDefault<MATAKULIAH>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(MATAKULIAH emp)
        {
            using (DBModels db = new DBModels())
            {
                if (Request.Form["hm"] == "0")
                {
                    if (db.MATAKULIAHs.Find(emp.KODE_MK) is null)
                    {
                        try
                        {
                            db.MATAKULIAHs.Add(emp);
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
                MATAKULIAH emp = db.MATAKULIAHs.Where(x => x.KODE_MK == id).FirstOrDefault<MATAKULIAH>();
                db.MATAKULIAHs.Remove(emp);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}