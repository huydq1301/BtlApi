using RestAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestAPI.Controllers
{
    public class GioHangController : ApiController
    {
        private DbQuanLyBanQuanAoDataContext DbQuanAo = new DbQuanLyBanQuanAoDataContext(
            "Data Source=DESKTOP-HU50TNN\\SQLEXPRESS;Initial Catalog=API_QuanLyBanQuanAo;Integrated Security=True"
            );
        [HttpGet]
        public List<GioHang> GetAllSanPham()
        {
            return DbQuanAo.GioHangs.ToList();
        }
        [HttpGet]
        public GioHang GetSanPham(string masp)
        {
            GioHang sp = DbQuanAo.GioHangs.FirstOrDefault(x => x.MaSP == masp);
            if (sp != null)
                if (sp.IsShopped == 0)
                    return sp;
            return null;
        }
        [HttpPost]
        public bool AddNewSanPham(GioHang sanPham)
        {
            try
            {
                sanPham.IsShopped = 0;
                DbQuanAo.GioHangs.InsertOnSubmit(sanPham);
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        [HttpPut]
        public bool UpDateIsShoppedx(string id, string xoa)
        {
            try
            {
                GioHang sanPham = DbQuanAo.GioHangs.FirstOrDefault(x => x.ID == id);
                if (sanPham == null) return false;
                // gan gia tri tu post data vao sanpham tim thay trong sanpham
                if (xoa == "1")
                {
                    sanPham.IsShopped = 1;
                }
                else if (xoa == "-1")
                {
                    sanPham.IsShopped = -1;

                }
                else if (xoa == "0")
                {
                    sanPham.IsShopped = 0;
                }
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
