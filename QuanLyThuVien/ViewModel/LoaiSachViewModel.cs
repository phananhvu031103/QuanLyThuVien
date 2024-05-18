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
    public class LoaiSachViewModel : BaseViewModel
    {
        public NHANVIEN NhanVien;
        public LOAINV LoaiNhanVien;

        private ObservableCollection<LOAISACH> _List;
        public ObservableCollection<LOAISACH> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private LOAISACH _SelectedItem;
        public LOAISACH SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();

                if (SelectedItem != null)
                {
                    MaLS = SelectedItem.MaLS;
                    TenLS = SelectedItem.TenLS;
                }
            }
        }

        private int _MaLS;
        public int MaLS { get => _MaLS; set { _MaLS = value; OnPropertyChanged(); } }

        private string _TenLS;
        public string TenLS { get => _TenLS; set { _TenLS = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public LoaiSachViewModel(NHANVIEN _NhanVien)
        {
            NhanVien = _NhanVien;
            LoaiNhanVien = DataProvider.Ins.DB.LOAINVs.Where(x => x.MaLNV == NhanVien.MaLNV).SingleOrDefault();

            List = new ObservableCollection<LOAISACH>(DataProvider.Ins.DB.LOAISACHes.Where(x => x.Xoa == 0));

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(TenLS))
                {
                    return false;
                }

                var lsList = DataProvider.Ins.DB.LOAISACHes.Where(x => x.TenLS == TenLS && x.Xoa == 0);

                if (lsList == null || lsList.Count() != 0)
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
                var loaiSach = new LOAISACH() { TenLS = TenLS, Xoa = 0 };

                DataProvider.Ins.DB.LOAISACHes.Add(loaiSach);
                DataProvider.Ins.DB.SaveChanges();

                List.Add(loaiSach);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(TenLS))
                {
                    return false;
                }

                var lsList = DataProvider.Ins.DB.LOAISACHes.Where(x => x.TenLS == TenLS && x.Xoa == 0);

                if (lsList == null || lsList.Count() != 0)
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
                MessageBoxResult rs = MessageBox.Show("Tất cả các sách thuộc loại sách này sẽ cập nhật lại thuộc tính loại sách.\nBạn có chắc chắn muốn cập nhật loại sách này?", "Lưu ý", MessageBoxButton.OKCancel);

                if (rs == MessageBoxResult.OK)
                {
                    var loaiSach = DataProvider.Ins.DB.LOAISACHes.Where(x => x.MaLS == SelectedItem.MaLS).SingleOrDefault();

                    loaiSach.TenLS = TenLS;

                    DataProvider.Ins.DB.SaveChanges();

                    SelectedItem.TenLS = TenLS;
                }
            });

            /*DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(TenLS))
                {
                    return false;
                }

                var lsList = DataProvider.Ins.DB.LOAISACHes.Where(x => x.TenLS == TenLS && x.Xoa == 0);

                if (lsList == null || lsList.Count() == 0)
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
                MessageBoxResult rs = MessageBox.Show("Tất cả các sách thuộc loại sách này sẽ bị xóa.\nBạn có chắc chắn muốn xóa loại sách này?", "Lưu ý", MessageBoxButton.OKCancel);

                if (rs == MessageBoxResult.OK)
                {
                    var loaiSach = DataProvider.Ins.DB.LOAISACHes.Where(x => x.MaLS == SelectedItem.MaLS).SingleOrDefault();
                    var sachs = DataProvider.Ins.DB.SACHes.Where(x => x.MaLS == loaiSach.MaLS && x.Xoa == 0).ToList();

                    //Xóa các sách thuộc loại sách này
                    foreach (SACH sach in sachs)
                    {
                        //Nếu sách này đang được mượn thì xóa chi tiết phiếu mượn
                        if (sach.TinhTrang == "Đang mượn")
                        {
                            var ctpm = DataProvider.Ins.DB.CHITIETPHIEUMUONs.Where(x => x.MaSach == sach.MaSach && x.Xoa == 0).SingleOrDefault();
                            ctpm.Xoa = 1;
                            DataProvider.Ins.DB.SaveChanges();

                            var countctpm = DataProvider.Ins.DB.CHITIETPHIEUMUONs.Where(x => x.MaPM == ctpm.MaPM && x.Xoa == 0);

                            //Nếu phiếu mượn chỉ gồm chi tiết phiếu mượn này thì xóa phiếu mượn luôn
                            if (countctpm == null || countctpm.Count() == 0)
                            {
                                var pm = DataProvider.Ins.DB.PHIEUMUONs.Where(x => x.MaPM == ctpm.MaPM).SingleOrDefault();
                                pm.Xoa = 1;

                                DataProvider.Ins.DB.SaveChanges();
                            }
                        }

                        sach.Xoa = 1;

                        DataProvider.Ins.DB.SaveChanges();
                    }

                    //Xóa loại sách này
                    loaiSach.Xoa = 1;

                    DataProvider.Ins.DB.SaveChanges();

                    List.Remove(loaiSach);
                }
            });*/
        }
    }
}
