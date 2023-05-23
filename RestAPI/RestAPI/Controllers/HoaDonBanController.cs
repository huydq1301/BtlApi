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
    public class HoaDonBanController : ApiController
    {
        private DbQuanLyBanQuanAoDataContext DbQuanAo = new DbQuanLyBanQuanAoDataContext(
            "Data Source=DESKTOP-HU50TNN\\SQLEXPRESS;Initial Catalog=API_QuanLyBanQuanAo;Integrated Security=True"
            );
        //httpGet: dùng để lấy thông tin san pham
        //1. Dịch vụ lấy thông tin của toàn bộ san pham
        [HttpGet]
        public List<tblHoaDonBan> GetAllhdn()
        {
            return DbQuanAo.tblHoaDonBans.ToList();

        }
        //2. Dịch vụ lấy thông tin một san pham với mã nào đó
        [HttpGet]
        public tblHoaDonBan Gethdn(string id)
        {
            tblHoaDonBan sp = DbQuanAo.tblHoaDonBans.FirstOrDefault(x => x.MaHDB == id);
            if (sp != null)
                if (sp.IsDeleted == 0)
                    return sp;
            return null;
        }
        [HttpGet]
        public tblHoaDonBan Gethdnkh(string makh)
        {
            tblHoaDonBan sp = DbQuanAo.tblHoaDonBans.FirstOrDefault(x => x.MaKH == makh);
            if (sp != null)
                if (sp.IsDeleted == 0)
                    return sp;
            return null;
        }
        //3. httpPost, dịch vụ thêm mới một san pham
        [HttpPost]
        public bool AddNewhdn(tblHoaDonBan hdn)
        {
            try
            {
                hdn.IsDeleted = 0;
                DbQuanAo.tblHoaDonBans.InsertOnSubmit(hdn);
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
        public bool UpdateNewhdn(tblHoaDonBan postData)
        {
            try
            {
                tblHoaDonBan hdn = DbQuanAo.tblHoaDonBans.FirstOrDefault(x => x.MaHDB == postData.MaHDB);
                if (hdn == null) return false;
                // gan gia tri tu post data vao hdn tim thay trong hdn
                hdn.MaHDB = postData.MaHDB;
                hdn.MaKH = postData.MaKH;
                hdn.NgayBan = postData.NgayBan;
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
                tblHoaDonBan hdn = DbQuanAo.tblHoaDonBans.FirstOrDefault(x => x.MaHDB == id);
                if (hdn == null) return false;
                // gan gia tri tu post data vao hdn tim thay trong hdn
                if (xoa == "1")
                {
                    hdn.IsDeleted = 1;
                }
                else if (xoa == "0")
                {
                    hdn.IsDeleted = 0;
                }
                else if (xoa == "-1")
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
       
    }
}
