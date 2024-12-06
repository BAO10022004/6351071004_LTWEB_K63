using Lab1.Models;
using System;
using PagedList;
using PagedList.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Data.Entity.Migrations;

namespace Lab1.Controllers
{
    public class AdminController : Controller
    {
        QLBanXeGanMay3Entities2 db = new QLBanXeGanMay3Entities2();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            var username = fc["username"];
            var password = fc["password"];
            if (username.Trim().Equals(""))
            {
                ViewData["username"] = "Username have to not null";
            }
            else
            if (password.Trim().Equals(""))
            {
                ViewData["password"] = "Password have to not null";
            }
            else
            {

                Admin admin = db.Admins.SingleOrDefault(x => x.UserAdmin.Equals(username) && x.PassAdmin.Equals(password));
                if (admin != null)
                {
                    Session["Admin"] = admin;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewData["Thongbao"] = "User or password not right";
                }

            }
            return View();
        }
        public ActionResult MotorbikeManage(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.XEGANMAYs.ToList().OrderByDescending(n => n.Soluongton).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create()
        {
            ViewBag.MaLX = new SelectList(db.LOAIXEs.ToList().OrderByDescending(n => n.TenLoaiXe), "MaLX", "TenLoaiXe");
            ViewBag.MaNPP = new SelectList(db.NHAPHANPHOIs.ToList().OrderByDescending(n => n.TenNPP), "MaNPP", "TenNPP");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(XEGANMAY xe, HttpPostedFileBase fileUpLoad)
        {
            if (fileUpLoad != null && fileUpLoad.ContentLength > 0)
            {
                // Lấy tên file và đường dẫn
                var fileName = Path.GetFileName(fileUpLoad.FileName);
                var path = Path.Combine(Server.MapPath("~/images"), fileName);
                System.Diagnostics.Debug.WriteLine("Path to save image: " + path);
                // Kiểm tra nếu file đã tồn tại
                if (System.IO.File.Exists(path))
                {
                    
                    ViewBag.Thongbao = "Hình ảnh đã tồn tại.";
                }
                else
                {
                    // Lưu file lên server
                    fileUpLoad.SaveAs(path);
                    xe.Anhbia = fileName; // Gán tên file vào thuộc tính Anhbia của đối tượng xe
                }
            }
            else
            {
                ViewBag.Thongbao = "Vui lòng chọn một file hình ảnh.";
            }

            // Thêm đối tượng xe vào cơ sở dữ liệu
            db.XEGANMAYs.Add(xe);

            // Lưu thay đổi vào cơ sở dữ liệu một cách bất đồng bộ
             db.SaveChangesAsync();

            // Điều hướng đến danh sách xe sau khi tạo thành công
           return RedirectToAction("MotorbikeManage");
        }

        public ActionResult Details(int id)
        {
            var xe = db.XEGANMAYs.FirstOrDefault(x => x.MaXe.Equals(id));
            if (xe == null)
            {
                return null;
            }
            return View(xe);
        }
        public ActionResult Delete(int id)
        {
            var xe = db.XEGANMAYs.FirstOrDefault(x => x.MaXe.Equals(id));
            if (xe == null)
            {
                return null;
            }
            return View(xe);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteBook(int id)
        {
            var xe = db.XEGANMAYs.FirstOrDefault(x => x.MaXe.Equals(id));
            if (xe == null)
            {
                return null;
            }
            db.XEGANMAYs.Remove(xe);
            db.SaveChanges();
            return RedirectToAction("MotorbikeManage");
        }
        public ActionResult Update(int id)
        {
            var xe = db.XEGANMAYs.FirstOrDefault(x => x.MaXe.Equals(id));
            if (xe == null)
            {
                return null;
            }
            ViewBag.MaLX = new SelectList(db.LOAIXEs.ToList().OrderByDescending(n => n.TenLoaiXe), "MaLX", "TenLoaiXe");
            ViewBag.MaNPP = new SelectList(db.NHAPHANPHOIs.ToList().OrderByDescending(n => n.TenNPP), "MaNPP", "TenNPP");
            return View(xe);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(XEGANMAY xe, HttpPostedFileBase fileUpLoad)
        {
            ViewBag.MaLX = new SelectList(db.LOAIXEs.ToList().OrderByDescending(n => n.TenLoaiXe), "MaLX", "TenLoaiXe");
            ViewBag.MaNPP = new SelectList(db.NHAPHANPHOIs.ToList().OrderByDescending(n => n.TenNPP), "MaNPP", "TenNPP");
            if (fileUpLoad != null && fileUpLoad.ContentLength > 0)
            {
                // Lấy tên file và đường dẫn
                var fileName = Path.GetFileName(fileUpLoad.FileName);
                var path = Path.Combine(Server.MapPath("~/images"), fileName);
                System.Diagnostics.Debug.WriteLine("Path to save image: " + path);
                // Kiểm tra nếu file đã tồn tại
                if (System.IO.File.Exists(path))
                {

                    ViewBag.Thongbao = "Hình ảnh đã tồn tại.";
                }
                else
                {
                    // Lưu file lên server
                    fileUpLoad.SaveAs(path);
                    xe.Anhbia = fileName; // Gán tên file vào thuộc tính Anhbia của đối tượng xe
                }
            }
            else
            {
                ViewBag.Thongbao = "Vui lòng chọn một file hình ảnh.";
            }

            db.XEGANMAYs.AddOrUpdate(xe);
               db.SaveChanges();            
                return RedirectToAction("MotorbikeManage");

        }
       
        [HttpGet]
        public ActionResult ThongKe()
        {
            var cars = db.XEGANMAYs
                .GroupBy(c => c.LOAIXE)
                .ToList();

            return View(cars);
        }
    }
}