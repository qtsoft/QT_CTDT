//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CTDT.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class KetQuaKiemTra
    {
        public int Id { get; set; }
        public string MaChungTu { get; set; }
        public string NoiDungKT { get; set; }
        public Nullable<System.DateTime> NgayKT { get; set; }
        public string DiaDiemKT { get; set; }
        public string DonViKT { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<bool> KetQuaKT { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string NguoiKiemTra { get; set; }
    }
}
