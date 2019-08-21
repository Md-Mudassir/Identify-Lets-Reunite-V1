using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdentifyProject.Models;
using System.Data.Entity;
using System.IO;

namespace IdentifyProject.Controllers
{
    public class SightedController : Controller
    {
        // GET: Sighted
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewAll()
        {
            return View(GetAllSights());
        }

        IEnumerable<SightedDB> GetAllSights()
        {
            using (DBModel db = new DBModel())
            {
                return db.SightedDBs.ToList<SightedDB>();
            }

        }

        public ActionResult AddOrEdit(int id = 0)
        {
            SightedDB Sit = new SightedDB();
            if (id != 0)
            {
                using (DBModel db = new DBModel())
                {
                    Sit = db.SightedDBs.Where(x => x.SID == id).FirstOrDefault<SightedDB>();
                }
            }
            return View(Sit);
        }

        [HttpPost]
        public ActionResult AddOrEdit(SightedDB Sit)
        {
            try
            {
                if (Sit.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(Sit.ImageUpload.FileName);
                    string extension = Path.GetExtension(Sit.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    Sit.ImagePath = "~/AppFiles/Images/" + fileName;
                    Sit.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                }
                using (DBModel db = new DBModel())
                {
                    if (Sit.SID == 0)
                    {
                        db.SightedDBs.Add(Sit);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(Sit).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllSights()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
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
                    SightedDB Sit = db.SightedDBs.Where(x => x.SID == id).FirstOrDefault<SightedDB>();
                    db.SightedDBs.Remove(Sit);
                    db.SaveChanges();
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllSights()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}