using Lab1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;



namespace Lab1.Controllers
{
    public class HomeController : Controller
    {
        QLBanXeGanMay3Entities2 dbContent = new QLBanXeGanMay3Entities2();
        // GET: Home
        public ActionResult Index(int CategoryId = -1)
        {
            var hangSanXuatList = dbContent.HANGSANXUATs.ToList();  // Dữ liệu cho Sidebar
            ViewBag.HangSanXuat = hangSanXuatList;
            if (CategoryId <0 )
            {
                return View(LayXeReNhat(dbContent.XEGANMAYs.ToArray().Count()));
            }
            else
            {
                var result = from xe in dbContent.XEGANMAYs
                             join sx in dbContent.SANXUATXEs on xe.MaXe equals sx.MaXe
                             where sx.MaHSX == CategoryId
                             select xe;

                return View(result.ToList());
            }
            
        }
        private List<XEGANMAY> LayXeReNhat(int count)
        {
            return dbContent.XEGANMAYs.OrderBy( x=> x.Giaban).ToList();
        }
        public ActionResult Details(int? XeId)
        {
            if (XeId == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            List< XEGANMAY> xe = new List<XEGANMAY>();
            foreach (XEGANMAY x in dbContent.XEGANMAYs) {
                if(x.MaXe == XeId)
                {
                    xe.Add(x); break;
                }
            }

           
            return View(xe);
        }
    }
}