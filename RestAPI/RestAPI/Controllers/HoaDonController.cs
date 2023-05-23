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
    public class HoaDonController : ApiController
    {
        private DbQuanLyBanQuanAoDataContext DbQuanAo = new DbQuanLyBanQuanAoDataContext(
            "Data Source=DESKTOP-HU50TNN\\SQLEXPRESS;Initial Catalog=API_QuanLyBanQuanAo;Integrated Security=True"
            );
        //httpGet: dùng để lấy thông tin san pham
        //1. Dịch vụ lấy thông tin của toàn bộ san pham
        [HttpGet]
        public List<tblHoaDonNhap> GetAllhdn()
        {
            return DbQuanAo.tblHoaDonNhaps.ToList();
        }
        //2. Dịch vụ lấy thông tin một san pham với mã nào đó
        [HttpGet]
        public tblHoaDonNhap Gethdn(string id)
        {
            tblHoaDonNhap sp = DbQuanAo.tblHoaDonNhaps.FirstOrDefault(x => x.MaHDN == id);
            if (sp != null)
                if (sp.IsDeleted == 0)
                    return sp;
            return null;
        }
        //3. httpPost, dịch vụ thêm mới một san pham
        [HttpPost]
        public bool AddNewhdn(tblHoaDonNhap hdn)
        {
            try
            {
                hdn.IsDeleted = 0;
                DbQuanAo.tblHoaDonNhaps.InsertOnSubmit(hdn);
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //4. httpPut để chỉnh sửa thông tin một hdn
        [HttpPut]
        public bool UpdateNewhdn(tblHoaDonNhap postData)
        {
            try
            {
                tblHoaDonNhap hdn = DbQuanAo.tblHoaDonNhaps.FirstOrDefault(x => x.MaHDN == postData.MaHDN);
                if (hdn == null) return false;
                // gan gia tri tu post data vao hdn tim thay trong hdn
                hdn.MaHDN = postData.MaHDN;
                hdn.MaNCC = postData.MaNCC;
                hdn.NgayNhap = postData.NgayNhap;
                hdn.MaNV = postData.MaNV;
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
                tblHoaDonNhap hdn = DbQuanAo.tblHoaDonNhaps.FirstOrDefault(x => x.MaHDN == id);
                if (hdn == null) return false;
                // gan gia tri tu post data vao hdn tim thay trong hdn
                if(xoa=="1")
                {
                    hdn.IsDeleted = 1;
                }else if(xoa=="0")
                {
                    hdn.IsDeleted = 0;
                }else if(xoa=="-1")
                {
                    hdn.IsDeleted = -1;

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
        public bool Deletehdn(string id)
        {
            try
            {
                tblHoaDonNhap hdn = DbQuanAo.tblHoaDonNhaps.FirstOrDefault(x => x.MaHDN == id);
                if (hdn == null) { return false; }
                tblChiTietHDN ctHDN = DbQuanAo.tblChiTietHDNs.FirstOrDefault(x => x.MaHDN == id);
                if (ctHDN != null) { return false; }
                DbQuanAo.tblHoaDonNhaps.DeleteOnSubmit(hdn);
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
