using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTDT.Model.ViewModels
{
    public class ViewChungTuModel
    {

        public int Id { get; set; }
        public string Ten { get; set; }
        public string MaChungTu { get; set; }
        public int? MaLoaiChungTu { get; set; }
        public DateTime? NgayBanHanh { get; set; }
        public string NguoiKy { get; set; }
        public string MaDonVi { get; set; }
        public string DonViBanHanh { get; set; }
        public int? TrangThai { get; set; }
        public string MaVach { get; set; }
        public string FileDinhKem { get; set; }
        public string TenLoaiChungTu { get; set; }
      
    }
}
