using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using QuanLyThuVien.UserControlLibrary;
using System.Windows.Controls;
namespace QuanLyThuVien.ViewModel
{
    public class NhanVienViewModel : BaseViewModel
    {
        public NHANVIEN NhanVien;
        public LOAINV LoaiNhanVien;

        private ObservableCollection<NHANVIEN> _List;
        public ObservableCollection<NHANVIEN> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<LOAINV> _LOAINVList;
        public ObservableCollection<LOAINV> LOAINVList { get => _LOAINVList; set { _LOAINVList = value; OnPropertyChanged(); } }

        private NHANVIEN _SelectedItem;
        public NHANVIEN SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();

                if (SelectedItem != null)
                {
                    MaNV = SelectedItem.MaNV;
                    TenNV = SelectedItem.TenNV;
                    NgaySinh = SelectedItem.NgaySinh;
                    DiaChi = SelectedItem.DiaChi;
                    Email = SelectedItem.Email;
                    TenDangNhap = SelectedItem.TenDangNhap;
                    SelectedLOAINV = SelectedItem.LOAINV;
                }
            }
        }

        private LOAINV _SelectedLOAINV;
        public LOAINV SelectedLOAINV { get => _SelectedLOAINV; set { _SelectedLOAINV = value; OnPropertyChanged(); } }

        private int _MaNV;
        public int MaNV { get => _MaNV; set { _MaNV = value; OnPropertyChanged(); } }

        private string _TenNV;
        public string TenNV { get => _TenNV; set { _TenNV = value; OnPropertyChanged(); } }

        private DateTime _NgaySinh;
        public DateTime NgaySinh { get => _NgaySinh; set { _NgaySinh = value; OnPropertyChanged(); } }

        private string _DiaChi;
        public string DiaChi { get => _DiaChi; set { _DiaChi = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private string _TenDangNhap;
        public string TenDangNhap { get => _TenDangNhap; set { _TenDangNhap = value; OnPropertyChanged(); } }

        private string _TenLNV;
        public string TenLNV { get => _TenLNV; set { _TenLNV = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ResetPasswordCommand { get; set; }

        public NhanVienViewModel() { }
        public NhanVienViewModel(NHANVIEN _NhanVien)
        {
            NhanVien = _NhanVien;
            LoaiNhanVien = DataProvider.Ins.DB.LOAINVs.Where(x => x.MaLNV == NhanVien.MaLNV).SingleOrDefault();

            List = new ObservableCollection<NHANVIEN>(DataProvider.Ins.DB.NHANVIENs.Where(x => x.Xoa == 0));
            LOAINVList = new ObservableCollection<LOAINV>(DataProvider.Ins.DB.LOAINVs.Where(x => x.Xoa == 0));

            /*AddCommand = new RelayCommand<Object>((p) =>
            {
                if (LoaiNhanVien.TenLNV != "Thủ thư")
                {
                    return false;
                }

                if (string.IsNullOrEmpty(TenNV) || string.IsNullOrEmpty(NgaySinh.ToString())
                   || string.IsNullOrEmpty(DiaChi) || string.IsNullOrEmpty(Email)
                   || string.IsNullOrEmpty(TenDangNhap) || SelectedLOAINV == null)
                {
                    return false;
                }

                var nvList = DataProvider.Ins.DB.NHANVIENs.Where(x =>
                x.TenNV == TenNV && x.NgaySinh == NgaySinh && x.DiaChi == DiaChi && x.Email == Email && x.TenDangNhap == TenDangNhap && x.MaLNV == SelectedItem.MaLNV && x.Xoa == 0);

                if (nvList == null || nvList.Count() != 0)
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                var tdnList = DataProvider.Ins.DB.NHANVIENs.Where(x => x.TenDangNhap == TenDangNhap && x.Xoa == 0);

                if (tdnList == null || tdnList.Count() != 0)
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại!");
                    return;
                }

                var emailList = DataProvider.Ins.DB.NHANVIENs.Where(x => x.Email == Email && x.Xoa == 0);

                if (emailList == null || emailList.Count() != 0)
                {
                    MessageBox.Show("Email đã tồn tại!");
                    return;
                }

                if (CalculateAge(NgaySinh) == -1 || CalculateAge(NgaySinh) < 18)
                {
                    MessageBox.Show("Nhân viên không đủ 18 tuổi!");
                    return;
                }

                string passDefault = "123456";
                string passEncode = MD5Hash(Base64Encode(passDefault));
                var nv = new NHANVIEN()
                {
                    TenNV = TenNV,
                    NgaySinh = NgaySinh,
                    DiaChi = DiaChi,
                    Email = Email,
                    MaLNV = SelectedLOAINV.MaLNV,
                    TenDangNhap = TenDangNhap,
                    MatKhau = passEncode,
                    Xoa = 0
                };

                DataProvider.Ins.DB.NHANVIENs.Add(nv);
                DataProvider.Ins.DB.SaveChanges();

                List.Add(nv);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (LoaiNhanVien.TenLNV != "Thủ thư")
                {
                    return false;
                }

                if (SelectedItem == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(TenNV) || string.IsNullOrEmpty(NgaySinh.ToString())
                   || string.IsNullOrEmpty(DiaChi) || string.IsNullOrEmpty(Email)
                   || string.IsNullOrEmpty(TenDangNhap) || SelectedLOAINV == null)
                {
                    return false;
                }

                var nvList = DataProvider.Ins.DB.NHANVIENs.Where(x =>
                x.TenNV == TenNV && x.NgaySinh == NgaySinh && x.DiaChi == DiaChi && x.Email == Email && x.TenDangNhap == TenDangNhap && x.MaLNV == SelectedLOAINV.MaLNV && x.Xoa == 0);

                if (nvList == null || nvList.Count() != 0)
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                var tdnList = DataProvider.Ins.DB.NHANVIENs.Where(x => x.TenDangNhap == TenDangNhap && x.Xoa == 0).SingleOrDefault();

                if (tdnList != null && tdnList.MaNV != MaNV)
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại!");
                    return;
                }

                var emailList = DataProvider.Ins.DB.NHANVIENs.Where(x => x.Email == Email && x.Xoa == 0).SingleOrDefault();

                if (emailList != null && emailList.MaNV != MaNV)
                {
                    MessageBox.Show("Email đã tồn tại!");
                    return;
                }

                if (CalculateAge(NgaySinh) == -1 || CalculateAge(NgaySinh) < 18)
                {
                    MessageBox.Show("Nhân viên không đủ 18 tuổi!");
                    return;
                }

                MessageBoxResult rs = MessageBox.Show("Bạn có chắc chắn muốn cập nhật thông tin nhân viên này?",
                    "Thông báo", MessageBoxButton.OKCancel);

                if (rs == MessageBoxResult.OK)
                {
                    var nv = DataProvider.Ins.DB.NHANVIENs.Where(x => x.MaNV == SelectedItem.MaNV).SingleOrDefault();

                    nv.TenNV = TenNV;
                    nv.NgaySinh = NgaySinh;
                    nv.DiaChi = DiaChi;
                    nv.Email = Email;
                    nv.MaLNV = SelectedLOAINV.MaLNV;
                    nv.TenDangNhap = TenDangNhap;

                    DataProvider.Ins.DB.SaveChanges();

                    SelectedItem.TenNV = TenNV;
                    SelectedItem.NgaySinh = NgaySinh;
                    SelectedItem.DiaChi = DiaChi;
                    SelectedItem.Email = Email;
                    SelectedItem.MaLNV = SelectedLOAINV.MaLNV;
                    SelectedItem.TenDangNhap = TenDangNhap;
                }
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (LoaiNhanVien.TenLNV != "Thủ thư")
                {
                    return false;
                }

                if (SelectedItem == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(TenNV) || string.IsNullOrEmpty(NgaySinh.ToString())
                   || string.IsNullOrEmpty(DiaChi) || string.IsNullOrEmpty(Email)
                   || string.IsNullOrEmpty(TenDangNhap) || SelectedLOAINV == null)
                {
                    return false;
                }

                var nvList = DataProvider.Ins.DB.NHANVIENs.Where(x =>
                x.TenNV == TenNV && x.NgaySinh == NgaySinh && x.DiaChi == DiaChi && x.Email == Email && x.TenDangNhap == TenDangNhap && x.MaLNV == SelectedItem.MaLNV && x.Xoa == 0);

                if (nvList == null || nvList.Count() == 0)
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                MessageBoxResult rs = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Thông báo", MessageBoxButton.OKCancel);

                if (rs == MessageBoxResult.OK)
                {
                    var nv = DataProvider.Ins.DB.NHANVIENs.Where(x => x.MaNV == MaNV).SingleOrDefault();

                    //Xóa nhân viên này
                    nv.Xoa = 1;

                    DataProvider.Ins.DB.SaveChanges();

                    List.Remove(nv);
                }
            });

            ResetPasswordCommand = new RelayCommand<object>((p) =>
            {
                if (LoaiNhanVien.TenLNV != "Thủ thư")
                {
                    return false;
                }

                if (SelectedItem == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(TenNV) || string.IsNullOrEmpty(NgaySinh.ToString())
                   || string.IsNullOrEmpty(DiaChi) || string.IsNullOrEmpty(Email)
                   || string.IsNullOrEmpty(TenDangNhap) || SelectedLOAINV == null)
                {
                    return false;
                }

                var nvList = DataProvider.Ins.DB.NHANVIENs.Where(x =>
                x.TenNV == TenNV && x.NgaySinh == NgaySinh && x.DiaChi == DiaChi && x.Email == Email && x.TenDangNhap == TenDangNhap && x.MaLNV == SelectedItem.MaLNV && x.Xoa == 0);

                if (nvList == null || nvList.Count() == 0)
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                MessageBoxResult rs = MessageBox.Show("Bạn có chắc chắn muốn reset mật khẩu của nhân viên này?", "Thông báo", MessageBoxButton.OKCancel);

                if (rs == MessageBoxResult.OK)
                {
                    var nv = DataProvider.Ins.DB.NHANVIENs.Where(x => x.MaNV == MaNV).SingleOrDefault();
                    string passDefault = "123456";
                    string passEncode = MD5Hash(Base64Encode(passDefault));

                    //Reset password nhân viên này
                    nv.MatKhau = passEncode;

                    DataProvider.Ins.DB.SaveChanges();
                }
            });*/

        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public int CalculateAge(DateTime d)
        {
            DateTime now = DateTime.Now;

            if (DateTime.Compare(now, d) == -1)
            {
                return -1;
            }

            int year = now.Year - d.Year;

            if (d.Month < now.Month)
            {
                year--;
            }

            if (d.Month == now.Month && d.Day < now.Day)
            {
                year--;
            }


            return year;
        }
    }
}
