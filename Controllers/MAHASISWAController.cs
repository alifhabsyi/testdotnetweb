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
    public class MAHASISWAController : Controller
    {
        // GET: MAHASISWA
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData()
        {
            using (DBModels db = new DBModels())
            {
                List<MAHASISWA> MHSLIST = db.MAHASISWAs.ToList<MAHASISWA>();
                return Json(new { data = MHSLIST }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddOrEdit(string id = "")
        {
            if (id == "")
                return View(new MAHASISWA());
            else
            {
                using (DBModels db = new DBModels())
                {
                    return View(db.MAHASISWAs.Where(x => x.NIM == id).FirstOrDefault<MAHASISWA>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(MAHASISWA emp)
        {
            using (DBModels db = new DBModels())
            {
                if (Request.Form["hm"]=="0")
                {
                    if(db.MAHASISWAs.Find(emp.NIM) is null)
                    {
                        try
                        {
                            db.MAHASISWAs.Add(emp);
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
                MAHASISWA emp = db.MAHASISWAs.Where(x => x.NIM == id).FirstOrDefault<MAHASISWA>();
                db.MAHASISWAs.Remove(emp);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}