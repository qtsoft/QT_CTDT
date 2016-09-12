using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTDT.Model.ViewModels
{
    public class ChungTuModel
    {
        public int Id { get; set; }
        public string MaChungTu { get; set; }
        public string Ten { get; set; }
        public Nullable<int> MaLoaiChungTu { get; set; }
        public Nullable<System.DateTime> NgayBanHanh { get; set; }
        public string NguoiKy { get; set; }
        public string MaDonVi { get; set; }
        public string DonViBanHanh { get; set; }
        public Nullable<int> TrangThai { get; set; }
        public string MaVach { get; set; }
        public string FileDinhKem { get; set; }
    }
}
