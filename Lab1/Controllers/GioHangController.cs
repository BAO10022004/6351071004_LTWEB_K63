using Lab1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab1.Controllers
{
    public class GioHangController : Controller
    {
        QLBanXeGanMay3Entities2 db = new QLBanXeGanMay3Entities2();
        // GET: GioHang
        public ActionResult Index()
        {
            List<Giohang> list = layGioHang();
            if (list.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["TongSoLuong"] = tongSoLuong();
            ViewData["TongTien"] = tongTien();
            return View(list);
        }


        public ActionResult GioHang()
        {
            List<Giohang> list = layGioHang();
            if (list.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["TongSoLuong"] = tongSoLuong();
            ViewData["TongTien"] = tongTien();
            return View(list);
        }
        public ActionResult themGioHang(int iMaXe, string strUrl)
        {
            List<Giohang> list = layGioHang();
            Giohang sanPham = list.Find(x => x.iMaXeMay == iMaXe);
            if (sanPham == null)
            {
                sanPham = new Giohang(iMaXe);
                list.Add(sanPham);
                return Redirect(strUrl);
            }
            sanPham.iSoLuong++;
            return Redirect(strUrl);
        }
        public int tongSoLuong()
        {
            int sum = 0;
            List<Giohang> list = Session["GioHang"] as List<Giohang>;
            if (list != null)
            {
                sum = list.Sum(x => x.iSoLuong);
            }
            return sum;
        }
        public double tongTien()
        {
            double sum = 0;
            List<Giohang> list = Session["GioHang"] as List<Giohang>;
            if (list != null)
            {
                sum = list.Sum(x => x.iThanhTien);
            }
            return sum;
        }
        public List<Giohang> layGioHang()
        {
            List<Giohang> list = Session["GioHang"] as List<Giohang>;
            if (list == null)
            {
                list = new List<Giohang>();
                Session["GioHang"] = list;
            }
            return list;

        }
        public ActionResult GiohangPartial()
        {
            ViewData["TongSoLuong"] = tongSoLuong();
            ViewData["TongTien"] = tongTien();
            return PartialView();
        }
        public ActionResult XoaGioHang(int iMaXe)
        {
            List<Giohang> list = layGioHang();
            Giohang sp = list.SingleOrDefault(x => x.iMaXeMay == iMaXe);
            if (sp != null)
            {
                list.RemoveAll(x => x.iMaXeMay == sp.iMaXeMay);
                return RedirectToAction("GioHang");
            }
            if (list.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang(int iMaXe, FormCollection formCollection)
        {
            List<Giohang> list = layGioHang();
            Giohang sp = list.SingleOrDefault(x => x.iMaXeMay == iMaXe);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(formCollection["SoLuong"].ToString());

            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCa()
        {
            List<Giohang> list = layGioHang();
            list.Clear();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult DatHang()
        {
            if (Session["Taikhoang"] == null || Session["Taikhoang"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["GioHang"] == null )
            {
                return RedirectToAction("Index", "Home");
            }
            List<Giohang> list = layGioHang();
            ViewData["TongSoLuong"] = tongSoLuong();
            ViewData["TongTien"] = tongTien();
            return View(list);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection formCollection)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoang"];
            List<Giohang> list = layGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            ddh.Ngaygiao = DateTime.Parse(String.Format("{0:MM/dd/yyy}", formCollection["Ngaygiao"]));
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            db.DONDATHANGs.Add(ddh);
            
            foreach (Giohang g in list)
            {
                CHITIETDONTHANG cHITIETDONTHANG = new CHITIETDONTHANG();
                cHITIETDONTHANG.MaXe = g.iMaXeMay;
                cHITIETDONTHANG.MaDonHang = ddh.MaDonHang;
                cHITIETDONTHANG.Soluong = g.iSoLuong;
                cHITIETDONTHANG.Dongia = (decimal)g.dDonGia;
                db.CHITIETDONTHANGs.Add(cHITIETDONTHANG);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("Xacnhandonhang", "GioHang");

        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}

