using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Akademik.Models;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Reflection;


namespace Akademik.Controllers
{
    public class PerkuliahanController : Controller
    {
        // GET: PERKULIAHAN
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData()
        {
            using (DBModels db = new DBModels())
            {
                List<PERKULIAHAN> MHSLIST = db.PERKULIAHANs.ToList<PERKULIAHAN>();
                //var id = "87";
                //List<PERKULIAHAN> MHSLIST = db.PERKULIAHANs.Where(x => x.NIM == "");
                return Json(new { data = MHSLIST }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddOrEdit(string id = "")
        {
            if (id == "")
            {
                //PERKULIAHAN datamhs = new PERKULIAHAN();
                
                //using (DBModels db = new DBModels())
                //datamhs.namamahasiswa = db.MAHASISWAs.ToList<MAHASISWA>();
                //return View(datamhs);
                PERKULIAHAN datamk = new PERKULIAHAN();

                using (DBModels db = new DBModels())
                    datamk.listmk = db.MATAKULIAHs.ToList<MATAKULIAH>();
                using (DBModels db = new DBModels())
                    datamk.namamahasiswa = db.MAHASISWAs.ToList<MAHASISWA>();
                using (DBModels db = new DBModels())
                    datamk.listdosen = db.DOSENs.ToList<DOSEN>();
              

                return View(datamk);


            }
            else
            {
                using (DBModels db = new DBModels())
                {
                    
                    return View(db.PERKULIAHANs.Where(x => x.NIM == id).FirstOrDefault<PERKULIAHAN>());
                }
            }
        }
        

        [HttpPost]
        public ActionResult AddOrEdit(PERKULIAHAN emp)
        {
            using (DBModels db = new DBModels())
            {


                //return Json(new { success = true, message = Request.Form["hm"] }, JsonRequestBehavior.AllowGet);
                if (Request.Form["hm"] == "0")
                    
                {
                    var nimm = Request.Form["NIM"];
                    var kode_mk = Request.Form["KODE_MK"];

                    var drafts = db.PERKULIAHANs.Where(p => p.NIM == nimm && p.KODE_MK == kode_mk).ToList();

                    if (drafts.Count > 0)
                    {
                        return Json(new { success = true, message = "Data dengan NIM : "+nimm+" dan Kode MK : "+kode_mk + "Sudah ada" }, JsonRequestBehavior.AllowGet);
                    }
                    //}
                    //if (db.PERKULIAHANs.Find(emp.NIM) is null)
                    //{
                    else { 
                    try
                        {
                            db.PERKULIAHANs.Add(emp);
                            try
                            {
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
                                        return Json(new { success = true, message = "Error!" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }

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
                PERKULIAHAN emp = db.PERKULIAHANs.Where(x => x.NIM == id).FirstOrDefault<PERKULIAHAN>();
                db.PERKULIAHANs.Remove(emp);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}