//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyThuVien.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CHITIETPHIEUMUON
    {
        public int MaPM { get; set; }
        public int MaSach { get; set; }
        public string TinhTrang { get; set; }
        public System.DateTime NgayTra { get; set; }
        public int Xoa { get; set; }
    
        public virtual PHIEUMUON PHIEUMUON { get; set; }
        public virtual SACH SACH { get; set; }
    }
}
