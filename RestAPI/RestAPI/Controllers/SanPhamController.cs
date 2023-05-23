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
    public class SanPhamController : ApiController
    {
        private DbQuanLyBanQuanAoDataContext DbQuanAo = new DbQuanLyBanQuanAoDataContext(
            "Data Source=DESKTOP-HU50TNN\\SQLEXPRESS;Initial Catalog=API_QuanLyBanQuanAo;Integrated Security=True"
            );
        //httpGet: dùng để lấy thông tin san pham
        //1. Dịch vụ lấy thông tin của toàn bộ san pham
        [HttpGet]
        public List<tblSanPham> GetAllSanPham()
        {
            return DbQuanAo.tblSanPhams.ToList();
        }
       
        [HttpGet]
        public List<tblSanPham> getAll(string name)
        {
            if (name == null || name == "")
            {
                GetAllSanPham();
            }
            List<tblSanPham> tSanPham = new List<tblSanPham>();
            tSanPham = DbQuanAo.tblSanPhams.Where(x => x.TenSP.Contains(name)).ToList();
            List<tblSanPham> listSPCurrent = new List<tblSanPham>();
            foreach (var sp in tSanPham)
            {
                if (sp.IsDeleted == 0)
                    listSPCurrent.Add(sp);
            }
            return listSPCurrent;
        }
        //2. Dịch vụ lấy thông tin một san pham với mã nào đó
        [HttpGet]
        public tblSanPham GetSanPham(string id)
        {
            tblSanPham sp = DbQuanAo.tblSanPhams.FirstOrDefault(x => x.MaSP == id);
            if (sp != null)
                if (sp.IsDeleted == 0)
                    return sp;
            return null;
        }
        //3. httpPost, dịch vụ thêm mới một san pham
        [HttpPost]
        public bool AddNewSanPham(tblSanPham sanPham)
        {
            try
            {
                sanPham.IsDeleted = 0;
                DbQuanAo.tblSanPhams.InsertOnSubmit(sanPham);
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //4. httpPut để chỉnh sửa thông tin một sanPham
        [HttpPut]
        public bool UpdateNewSanPham(tblSanPham postData)
        {
            try
            {
                tblSanPham sanPham = DbQuanAo.tblSanPhams.FirstOrDefault(x => x.MaSP == postData.MaSP);
                if (sanPham == null) return false;
                // gan gia tri tu post data vao sanpham tim thay trong sanpham
                sanPham.MaSP = postData.MaSP;
                sanPham.TenSP = postData.TenSP;
                sanPham.MaNCC = postData.MaNCC;
                sanPham.MaSize = postData.MaSize;
                sanPham.MaMauSac = postData.MaMauSac;
                sanPham.MaChatLieu = postData.MaChatLieu;
                sanPham.SoLuong = postData.SoLuong;
                sanPham.MaMauSac = postData.MaMauSac;
                sanPham.DonGiaBan = postData.DonGiaBan;
                sanPham.DonGiaNhap = postData.DonGiaNhap;
                sanPham.Anh = postData.Anh;
                sanPham.GhiChu = postData.GhiChu;
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        [HttpPut]
        public bool UpDateIsDeleted(string id, string xoa)
        {
            try
            {
                tblSanPham sanPham = DbQuanAo.tblSanPhams.FirstOrDefault(x => x.MaSP == id);
                if (sanPham == null) return false;
                // gan gia tri tu post data vao sanpham tim thay trong sanpham
                if (xoa == "1")
                {
                    sanPham.IsDeleted = 1;
                }
                else if(xoa == "-1")
                {
                    sanPham.IsDeleted = -1;

                }
                else if(xoa == "0")
                {
                    sanPham.IsDeleted = 0;
                }
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
        public bool DeleteSanPham(string id)
        {
            try
            {
                tblSanPham sanPham = DbQuanAo.tblSanPhams.FirstOrDefault(x => x.MaSP == id);
                if (sanPham == null) { return false; }
                tblChiTietHDN ctHDN = DbQuanAo.tblChiTietHDNs.FirstOrDefault(x => x.MaSP == id);
                if (ctHDN != null) { return false; }
                DbQuanAo.tblSanPhams.DeleteOnSubmit(sanPham);
                // submit changed
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
