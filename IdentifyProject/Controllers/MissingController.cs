using System;
using IdentifyProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;


namespace IdentifyProject.Controllers
{
    public class MissingController : Controller
    {
        // GET: Missing
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewAll()
        {
            return View(GetAllMissings());
        }

        IEnumerable<MissingDB> GetAllMissings()
        {
            using (DBModel db = new DBModel())
            {
                return db.MissingDBs.ToList<MissingDB>();
            }

        }

        public ActionResult AddOrEdit(int id = 0)
        {
            MissingDB Mis = new MissingDB();
            if (id != 0)
            {
                using (DBModel db = new DBModel())
                {
                    Mis = db.MissingDBs.Where(x => x.MID == id).FirstOrDefault<MissingDB>();
                }
            }
            return View(Mis);
        }

        [HttpPost]
        public ActionResult AddOrEdit(MissingDB Mis)
        {
            try
            {
                if (Mis.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(Mis.ImageUpload.FileName);
                    string extension = Path.GetExtension(Mis.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    Mis.ImagePath = "~/AppFiles/Images/" + fileName;
                    Mis.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                }
                using (DBModel db = new DBModel())
                {
                    if (Mis.MID == 0)
                    {
                        db.MissingDBs.Add(Mis);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(Mis).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllMissings()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (DBModel db = new DBModel())
                {
                    MissingDB Mis = db.MissingDBs.Where(x => x.MID == id).FirstOrDefault<MissingDB>();
                    db.MissingDBs.Remove(Mis);
                    db.SaveChanges();
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllMissings()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}