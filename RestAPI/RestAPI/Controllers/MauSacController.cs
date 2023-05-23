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
    public class MauSacController : ApiController
    {
        private DbQuanLyBanQuanAoDataContext DbQuanAo = new DbQuanLyBanQuanAoDataContext(
            "Data Source=DESKTOP-HU50TNN\\SQLEXPRESS;Initial Catalog=API_QuanLyBanQuanAo;Integrated Security=True"
            );
        //httpGet: dùng để lấy thông tin san pham
        //1. Dịch vụ lấy thông tin của toàn bộ san pham
        [HttpGet]
        public List<tblMauSac> GetAllMauSac()
        {
            return DbQuanAo.tblMauSacs.ToList();
        }
        //2. Dịch vụ lấy thông tin một san pham với mã nào đó
        [HttpGet]
        public tblMauSac GetMauSac(string id)
        {
            return DbQuanAo.tblMauSacs.FirstOrDefault(x => x.MaMauSac == id);
        }
        //3. httpPost, dịch vụ thêm mới một san pham
        [HttpPost]
        public bool AddNewMauSac(tblMauSac mauSac)
        {
            try
            {
                DbQuanAo.tblMauSacs.InsertOnSubmit(mauSac);
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
        public bool UpdateNewmauSac(tblMauSac postData)
        {
            try
            {
                tblMauSac mauSac = DbQuanAo.tblMauSacs.FirstOrDefault(x => x.MaMauSac == postData.MaMauSac);
                if (mauSac == null) return false;
                // gan gia tri tu post data vao sanpham tim thay trong sanpham
                mauSac.MaMauSac = postData.MaMauSac;
                mauSac.TenMau = postData.TenMau;
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
        public bool DeleteMauSac(string id)
        {
            try
            {
                tblMauSac mauSac = DbQuanAo.tblMauSacs.FirstOrDefault(x => x.MaMauSac == id);
                if (mauSac == null) { return false; }
                tblSanPham sanPham = DbQuanAo.tblSanPhams.FirstOrDefault(x => x.MaMauSac == id);
                if (sanPham != null) { return false; }
                DbQuanAo.tblMauSacs.DeleteOnSubmit(mauSac);
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
