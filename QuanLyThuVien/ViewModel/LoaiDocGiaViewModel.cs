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
    public class LoaiDocGiaViewModel : BaseViewModel
    {
        public NHANVIEN NhanVien;
        public LOAINV LoaiNhanVien;

        private ObservableCollection<LOAIDG> _List;
        public ObservableCollection<LOAIDG> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private LOAIDG _SelectedItem;
        public LOAIDG SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();

                if (SelectedItem != null)
                {
                    MaLDG = SelectedItem.MaLDG;
                    TenLDG = SelectedItem.TenLDG;
                }
            }
        }

        private int _MaLDG;
        public int MaLDG { get => _MaLDG; set { _MaLDG = value; OnPropertyChanged(); } }

        private string _TenLDG;
        public string TenLDG { get => _TenLDG; set { _TenLDG = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public LoaiDocGiaViewModel(NHANVIEN _NhanVien)
        {
            NhanVien = _NhanVien;
            LoaiNhanVien = DataProvider.Ins.DB.LOAINVs.Where(x => x.MaLNV == NhanVien.MaLNV).SingleOrDefault();

            List = new ObservableCollection<LOAIDG>(DataProvider.Ins.DB.LOAIDGs.Where(x => x.Xoa == 0));

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(TenLDG))
                {
                    return false;
                }

                var lsList = DataProvider.Ins.DB.LOAIDGs.Where(x => x.TenLDG == TenLDG && x.Xoa == 0);

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
                var loaiSach = new LOAIDG() { TenLDG = TenLDG, Xoa = 0 };

                DataProvider.Ins.DB.LOAIDGs.Add(loaiSach);
                DataProvider.Ins.DB.SaveChanges();

                List.Add(loaiSach);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(TenLDG))
                {
                    return false;
                }

                var lsList = DataProvider.Ins.DB.LOAIDGs.Where(x => x.TenLDG == TenLDG && x.Xoa == 0);

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
                MessageBoxResult rs = MessageBox.Show("Tất cả các độc giả thuộc loại độc giả này sẽ cập nhật lại thuộc tính loại độc giả.\nBạn có chắc chắn muốn cập nhật loại độc giả này?",
                    "Lưu ý", MessageBoxButton.OKCancel);

                if (rs == MessageBoxResult.OK)
                {
                    var loaDG = DataProvider.Ins.DB.LOAIDGs.Where(x => x.MaLDG == SelectedItem.MaLDG).SingleOrDefault();

                    loaDG.TenLDG = TenLDG;

                    DataProvider.Ins.DB.SaveChanges();

                    SelectedItem.TenLDG = TenLDG;
                }
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(TenLDG))
                {
                    return false;
                }

                var ldgList = DataProvider.Ins.DB.LOAIDGs.Where(x => x.TenLDG == TenLDG && x.Xoa == 0);

                if (ldgList == null || ldgList.Count() == 0)
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
                MessageBoxResult rs = MessageBox.Show("Tất cả các độc giả thuộc loại độc giả này sẽ bị xóa.\nBạn có chắc chắn muốn xóa thẻ độc giả này?", "Lưu ý", MessageBoxButton.OKCancel);

                if (rs == MessageBoxResult.OK)
                {
                    var loaiDG = DataProvider.Ins.DB.LOAIDGs.Where(x => x.MaLDG == SelectedItem.MaLDG).SingleOrDefault();
                    var theDGs = DataProvider.Ins.DB.DOCGIAs.Where(x => x.MaLDG == loaiDG.MaLDG && x.Xoa == 0).ToList();

                    //Xóa các độc giả thuộc loại độc giả này
                    foreach (DOCGIA theDG in theDGs)
                    {
                        //Xóa các phiếu mượn thuộc thẻ độc giả này
                        var pms = DataProvider.Ins.DB.PHIEUMUONs.Where(x => x.MaDG == theDG.MaDG && x.Xoa == 0).ToList();

                        foreach (PHIEUMUON pm in pms)
                        {
                            //Xóa các chi tiết phiếu mượn thuộc phiếu mượn này
                            var ctpms = DataProvider.Ins.DB.CHITIETPHIEUMUONs.Where(x => x.MaPM == pm.MaPM && x.Xoa == 0).ToList();

                            foreach (CHITIETPHIEUMUON ctpm in ctpms)
                            {
                                ctpm.Xoa = 1;
                                DataProvider.Ins.DB.SaveChanges();

                                //Xóa các sách mà độc giả này đang mượn
                                if (pm.NgayHenTra == null)
                                {
                                    var sach = DataProvider.Ins.DB.SACHes.Where(x => x.MaSach == ctpm.MaSach).SingleOrDefault();
                                    sach.Xoa = 1;
                                    DataProvider.Ins.DB.SaveChanges();
                                }
                            }

                            pm.Xoa = 1;
                            DataProvider.Ins.DB.SaveChanges();
                        }

                        theDG.Xoa = 1;
                        DataProvider.Ins.DB.SaveChanges();
                    }

                    //Xóa loại độc giả này
                    loaiDG.Xoa = 1;

                    DataProvider.Ins.DB.SaveChanges();

                    List.Remove(loaiDG);
                }
            });
        }
    }
}
