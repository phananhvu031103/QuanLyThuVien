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
    
    public partial class SACH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SACH()
        {
            this.CHITIETPHIEUMUONs = new HashSet<CHITIETPHIEUMUON>();
        }
    
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public string TacGia { get; set; }
        public int NamXB { get; set; }
        public string NhaXB { get; set; }
        public System.DateTime NgayNhap { get; set; }
        public string TinhTrang { get; set; }
        public int SoLuong { get; set; }
        public int MaLS { get; set; }
        public int Xoa { get; set; }
    
        public virtual LOAISACH LOAISACH { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETPHIEUMUON> CHITIETPHIEUMUONs { get; set; }
    }
}
