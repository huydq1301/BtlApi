using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;

namespace RestAPI.Controllers
{
    public class ChiTietHoaDonController : ApiController
    {
        private DbQuanLyBanQuanAoDataContext DbQuanAo = new DbQuanLyBanQuanAoDataContext(
            "Data Source=DESKTOP-HU50TNN\\SQLEXPRESS;Initial Catalog=API_QuanLyBanQuanAo;Integrated Security=True"
            );
        //httpGet: dùng để lấy thông tin san pham
        //1. Dịch vụ lấy thông tin của toàn bộ san pham
        [HttpGet]
        public List<tblChiTietHDN> GetAllChiTietHDN(string id)
        {
            List<tblChiTietHDN> listSP = DbQuanAo.tblChiTietHDNs.Where(x => x.MaHDN == id).ToList();
            List<tblChiTietHDN> listSPCurrent = new List<tblChiTietHDN>();
            foreach (var sp in listSP)
            {
                if (sp.IsDeleted == 0)
                    listSPCurrent.Add(sp);
            }
            return listSPCurrent;
        }

        [HttpGet]
        public tblChiTietHDN GetID(string MaHDN)
        {
            tblChiTietHDN tChitiet = DbQuanAo.tblChiTietHDNs.FirstOrDefault(x => x.MaHDN == MaHDN);
            if (tChitiet != null)

                if (tChitiet.IsDeleted == 0)
                return tChitiet;
            return null;
        }
        [HttpGet]
        public tblChiTietHDN getAll(string MaSP)
        {
            tblChiTietHDN tChitiet = DbQuanAo.tblChiTietHDNs.FirstOrDefault(x => x.MaSP == MaSP);
            if (tChitiet != null)

                if (tChitiet.IsDeleted == 0)
                return tChitiet;
            return null;
        }
        //2. Dịch vụ lấy thông tin một san pham với mã nào đó
        [HttpGet]
        public tblChiTietHDN GetChiTietHDN(string hdn, string sp)
        {
            tblChiTietHDN tChitiet = DbQuanAo.tblChiTietHDNs.FirstOrDefault(x => x.MaHDN == hdn && x.MaSP == sp);
            if (tChitiet != null)

                if (tChitiet.IsDeleted == 0)
                return tChitiet;
            return null;
        }
        //3. httpPost, dịch vụ thêm mới một san pham
        [HttpPost]
        public bool AddNewChiTietHDN(tblChiTietHDN ChiTietHDN)
        {
            try
            {
                ChiTietHDN.IsDeleted = 0;
                DbQuanAo.tblChiTietHDNs.InsertOnSubmit(ChiTietHDN);
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //4. httpPut để chỉnh sửa thông tin một ChiTietHDN
        [HttpPut]
        public bool UpdateNewChiTietHDN(tblChiTietHDN postData)
        {
            try
            {
                tblChiTietHDN ChiTietHDN = DbQuanAo.tblChiTietHDNs.FirstOrDefault(x => x.MaHDN == postData.MaHDN && x.MaSP == postData.MaSP);
                if (ChiTietHDN == null) return false;
                // gan gia tri tu post data vao ChiTietHDN tim thay trong ChiTietHDN
                ChiTietHDN.MaHDN = postData.MaHDN;
                ChiTietHDN.MaSP = postData.MaSP;
                ChiTietHDN.SLNhap = postData.SLNhap;
                ChiTietHDN.ThanhTien = postData.ThanhTien;
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        [HttpPut]
        public bool UpDateIsDeleted(string id , string masp)
        {
            try
            {
                tblChiTietHDN sanPham = DbQuanAo.tblChiTietHDNs.FirstOrDefault(x => x.MaHDN == id && x.MaSP == masp);
                if (sanPham == null) return false;
                // gan gia tri tu post data vao sanpham tim thay trong sanpham
                sanPham.IsDeleted = 1;
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //5.httpDelete để xóa một san pham
        [HttpDelete]
        public bool DeleteCT(string hdn, string masp)
        {
            tblChiTietHDN HoaDon = DbQuanAo.tblChiTietHDNs.FirstOrDefault(x => x.MaHDN == hdn & x.MaSP == masp);
            if (HoaDon == null) { return false; }
            DbQuanAo.tblChiTietHDNs.DeleteOnSubmit(HoaDon);
            // submit changed
            DbQuanAo.SubmitChanges();
            return true;
        }
    }
}
