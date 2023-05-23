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
    public class NhaCungCapController : ApiController
    {
        private DbQuanLyBanQuanAoDataContext DbQuanAo = new DbQuanLyBanQuanAoDataContext(
            "Data Source=DESKTOP-HU50TNN\\SQLEXPRESS;Initial Catalog=API_QuanLyBanQuanAo;Integrated Security=True"
            );
        //httpGet: dùng để lấy thông tin san pham
        //1. Dịch vụ lấy thông tin của toàn bộ san pham
        [HttpGet]
        public List<tblNhaCungCap> GetAllNhaCungCap()
        {
            return DbQuanAo.tblNhaCungCaps.ToList();
        }
        //2. Dịch vụ lấy thông tin một san pham với mã nào đó
        [HttpGet]
        public tblNhaCungCap GetNhaCungCap(string id)
        {
            return DbQuanAo.tblNhaCungCaps.FirstOrDefault(x => x.MaNCC == id);
        }
        //3. httpPost, dịch vụ thêm mới một san pham
        [HttpPost]
        public bool AddNewNhaCungCap(tblNhaCungCap nhaCC)
        {
            try
            {
                DbQuanAo.tblNhaCungCaps.InsertOnSubmit(nhaCC);
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
        public bool UpdateNewNhaCC(tblNhaCungCap postData)
        {
            try
            {
                tblNhaCungCap nhaCC = DbQuanAo.tblNhaCungCaps.FirstOrDefault(x => x.MaNCC == postData.MaNCC);
                if (nhaCC == null) return false;
                // gan gia tri tu post data vao sanpham tim thay trong sanpham
                nhaCC.MaNCC = postData.MaNCC;
                nhaCC.TenNCC = postData.TenNCC;
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
        public bool DeleteNhaCungCap(string id)
        {
            try
            {
                tblNhaCungCap nhaCC = DbQuanAo.tblNhaCungCaps.FirstOrDefault(x => x.MaNCC == id);
                if (nhaCC == null) { return false; }
                tblHoaDonNhap hoaDonNhap = DbQuanAo.tblHoaDonNhaps.FirstOrDefault(x => x.MaNCC == id);
                if (hoaDonNhap != null){ return false; }
                tblSanPham sanPham = DbQuanAo.tblSanPhams.FirstOrDefault(x => x.MaNCC == id);
                if (sanPham != null) { return false; }
                DbQuanAo.tblNhaCungCaps.DeleteOnSubmit(nhaCC);
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
