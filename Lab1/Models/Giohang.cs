using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab1.Models
{
    public class Giohang
    {
        public int iMaXeMay { get; set; }
        public String sTenXe { get; set; }
        public String sAnhBia { get; set; }


        public Double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public Double iThanhTien
        {
            get{ return iSoLuong * dDonGia; }
        }
        public Giohang(int _iMaXeMay)
        {
            QLBanXeGanMay3Entities2 db = new QLBanXeGanMay3Entities2();
            var xEGANMAY = db.XEGANMAYs.Single( x => x.MaXe.Equals(_iMaXeMay));
            iMaXeMay = _iMaXeMay;
            sTenXe = xEGANMAY.TenXe;
            sAnhBia = xEGANMAY.Anhbia;
            dDonGia =double.Parse(xEGANMAY.Giaban.ToString());
            iSoLuong = 1;
        }
    }
}