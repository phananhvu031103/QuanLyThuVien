using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace QuanLyThuVien.ViewModel
{
    public class LoaiNhanVienViewModel : BaseViewModel
    {
        public NHANVIEN NhanVien;
        public LOAINV LoaiNhanVien;

        private ObservableCollection<LOAINV> _List;
        public ObservableCollection<LOAINV> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private LOAINV _SelectedItem;
        public LOAINV SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();

                if (SelectedItem != null)
                {
                    MaLNV = SelectedItem.MaLNV;
                    TenLNV = SelectedItem.TenLNV;
                }
            }
        }

        private int _MaLNV;
        public int MaLNV { get => _MaLNV; set { _MaLNV = value; OnPropertyChanged(); } }

        private string _TenLNV;
        public string TenLNV { get => _TenLNV; set { _TenLNV = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public LoaiNhanVienViewModel() { }
        public LoaiNhanVienViewModel(NHANVIEN _NhanVien)
        {
            NhanVien = _NhanVien;
            LoaiNhanVien = DataProvider.Ins.DB.LOAINVs.Where(x => x.MaLNV == NhanVien.MaLNV).SingleOrDefault();

            List = new ObservableCollection<LOAINV>(DataProvider.Ins.DB.LOAINVs.Where(x => x.Xoa == 0));

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(TenLNV))
                {
                    return false;
                }

                var lnvList = DataProvider.Ins.DB.LOAINVs.Where(x => x.TenLNV == TenLNV && x.Xoa == 0);

                if (lnvList == null || lnvList.Count() != 0)
                {
                    return false;
                }

                if (LoaiNhanVien.TenLNV != "Thủ thư")
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                var loaiNV = new LOAINV() { TenLNV = TenLNV, Xoa = 0 };

                DataProvider.Ins.DB.LOAINVs.Add(loaiNV);
                DataProvider.Ins.DB.SaveChanges();

                List.Add(loaiNV);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(TenLNV))
                {
                    return false;
                }

                var lnvList = DataProvider.Ins.DB.LOAINVs.Where(x => x.TenLNV == TenLNV && x.Xoa == 0);

                if (lnvList == null || lnvList.Count() != 0)
                {
                    return false;
                }

                if (LoaiNhanVien.TenLNV != "Thủ thư")
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                MessageBoxResult rs = MessageBox.Show("Tất cả các nhân viên thuộc loại nhân viên này sẽ cập nhật lại thuộc tính loại nhân viên.\nBạn có chắc chắn muốn cập nhật loại nhân viên này?",
                    "Lưu ý", MessageBoxButton.OKCancel);

                if (rs == MessageBoxResult.OK)
                {
                    var loaiNV = DataProvider.Ins.DB.LOAINVs.Where(x => x.MaLNV == SelectedItem.MaLNV).SingleOrDefault();

                    loaiNV.TenLNV = TenLNV;

                    DataProvider.Ins.DB.SaveChanges();

                    SelectedItem.TenLNV = TenLNV;
                }
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(TenLNV))
                {
                    return false;
                }

                var lnvList = DataProvider.Ins.DB.LOAINVs.Where(x => x.TenLNV == TenLNV && x.Xoa == 0);

                if (lnvList == null || lnvList.Count() == 0)
                {
                    return false;
                }

                if (LoaiNhanVien.TenLNV != "Thủ thư")
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                MessageBoxResult rs = MessageBox.Show("Tất cả các nhân viên thuộc loại nhân viên này sẽ bị xóa.\nBạn có chắc chắn muốn xóa loại nhân viên này?", "Lưu ý", MessageBoxButton.OKCancel);

                if (rs == MessageBoxResult.OK)
                {
                    var loaiNV = DataProvider.Ins.DB.LOAINVs.Where(x => x.MaLNV == SelectedItem.MaLNV).SingleOrDefault();

                    //Xóa tất cả nhân viên thuộc loại nhân viên này
                    var nvs = DataProvider.Ins.DB.NHANVIENs.Where(x => x.MaLNV == loaiNV.MaLNV && x.Xoa == 0).ToList();

                    foreach (NHANVIEN nv in nvs)
                    {
                        nv.Xoa = 1;
                        DataProvider.Ins.DB.SaveChanges();
                    }

                    //Xóa loại nhân viên này
                    loaiNV.Xoa = 1;

                    DataProvider.Ins.DB.SaveChanges();

                    List.Remove(loaiNV);
                }
            });
        }
    }
}
