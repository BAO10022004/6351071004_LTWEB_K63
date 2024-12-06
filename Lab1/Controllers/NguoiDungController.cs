using Lab1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab1.Controllers
{
    public class NguoiDungController : Controller
    {
        QLBanXeGanMay3Entities2 db = new QLBanXeGanMay3Entities2();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DangKi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKi(FormCollection formCollection, KHACHHANG khanhHang)
        {

            KHACHHANG kh = new KHACHHANG(formCollection["name"] , formCollection["username"], formCollection["password"],  formCollection["email"], formCollection["address"], formCollection["numberphone"],DateTime.Parse( formCollection["ngaysinh"]));
            if(kh.HoTen == "")
            {
                ViewData["Loi1"] = "name not empty";
            }else
            if (kh.Taikhoan == "")
            {
                ViewData["Loi2"] = "username not empty";
            }
            else
            if (kh.Matkhau == "")
            {
                ViewData["Loi3"] = "password not empty";
            }
            else
            if (formCollection["confirm"] == "")
            {
                ViewData["Loi4"] = "confirm password not empty";
            }
            else
            if (kh.Email == "")
            {
                ViewData["Loi5"] = "email password not empty";
            }
            else
            if (kh.DiachiKH == "")
            {
                ViewData["Loi6"] = "Address password not empty";
            }
            else
            if (kh.DienthoaiKH == "")
            {
                ViewData["Loi7"] = "numberphone password not empty";
            }
            else
            
            {
                khanhHang = kh;
                db.KHACHHANGs.Add(khanhHang);
                db.SaveChanges();
                return RedirectToAction("DangNhap");
            }

                return View();
        }

        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection formCollection)
        {
            var username = formCollection["username"];
            var password = formCollection["password"];
            if (username == "")
            {
                ViewData["Loi2"] = "username not empty";
            }
            else
            if (password == "")
            {
                ViewData["Loi3"] = "password not empty";
            }
            else
            {
                KHACHHANG kHACHHANG = db.KHACHHANGs.SingleOrDefault(kh => kh.Taikhoan.Equals(username)&& kh.Matkhau.Equals(password) );
                if(kHACHHANG != null )
                {
                    ViewData["messege"] = "SUCCESS";
                    Session["Taikhoang"] = kHACHHANG;

                }
                else
                {
                    ViewData["messege"] = "NOT SUCCESS";
                }
            }
            return View();
        }
    }
}