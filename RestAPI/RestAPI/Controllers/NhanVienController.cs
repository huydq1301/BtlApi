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
using System.Xml.Linq;

namespace RestAPI.Controllers
{
    public class NhanVienController : ApiController
    {
        private DbQuanLyBanQuanAoDataContext DbQuanAo = new DbQuanLyBanQuanAoDataContext(
            "Data Source=DESKTOP-HU50TNN\\SQLEXPRESS;Initial Catalog=API_QuanLyBanQuanAo;Integrated Security=True"
            );
        //httpGet: dùng để lấy thông tin san pham
        //1. Dịch vụ lấy thông tin của toàn bộ san pham
        [HttpGet]
        public List<tblNV> GetAllNV()
        {
           return  DbQuanAo.tblNVs.ToList();
            
        }
        [HttpGet]
        public List<tblNV> getAll(string name)
        {
            if (name == null || name == "")
            {
                GetAllNV();
            }
            List<tblNV> tNV = new List<tblNV>();
            tNV = DbQuanAo.tblNVs.Where(x => x.TenNV.Contains(name)).ToList();
            List<tblNV> listNVCurrent = new List<tblNV>();
            foreach (var NV in tNV)
            {
                if (NV.IsDeleted == 0)
                    listNVCurrent.Add(NV);
            }
            return listNVCurrent;
        }
        //2. Dịch vụ lấy thông tin một san pham với mã nào đó
        [HttpGet]
        public tblNV GetNV(string id)
        {
            tblNV nv = DbQuanAo.tblNVs.FirstOrDefault(x => x.MaNV == id);
            if (nv != null)

                if (nv.IsDeleted == 0)
                return nv;
            return null;
        }
        //3. httpPost, dịch vụ thêm mới một san pham
        [HttpPost]
        public bool AddNewNV(tblNV NV)
        {
            try
            {
                NV.IsDeleted = 0;
                DbQuanAo.tblNVs.InsertOnSubmit(NV);
                DbQuanAo.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //4. httpPut để chỉnh sửa thông tin một NV
        [HttpPut]
        public bool UpdateNewNV(tblNV postData)
        {
            try
            {
                tblNV NV = DbQuanAo.tblNVs.FirstOrDefault(x => x.MaNV == postData.MaNV);
                if (NV == null) return false;
                // gan gia tri tu post data vao NV tim thay trong NV
                NV.MaNV = postData.MaNV;
                NV.TenNV = postData.TenNV;
                NV.DiaChi = postData.DiaChi;
                NV.SoDienThoai = postData.SoDienThoai;
                NV.GioiTinh = postData.GioiTinh;
                NV.ChucVu = postData.ChucVu;
                NV.NgaySinh = postData.NgaySinh;
                NV.Anh = postData.Anh;
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
                tblNV nv = DbQuanAo.tblNVs.FirstOrDefault(x => x.MaNV == id);
                if (nv == null) return false;
                // gan gia tri tu post data vao nv tim thay trong nv
                if (xoa == "1")
                {
                    nv.IsDeleted = 1;
                }
                else if (xoa == "-1")
                {
                    nv.IsDeleted = -1;

                }
                else if (xoa == "0")
                {
                    nv.IsDeleted = 0;
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
        public bool DeleteNV(string id)
        {
            try
            {
                tblNV NV = DbQuanAo.tblNVs.FirstOrDefault(x => x.MaNV == id);
                if (NV == null) { return false; }
                DbQuanAo.tblNVs.DeleteOnSubmit(NV);
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
