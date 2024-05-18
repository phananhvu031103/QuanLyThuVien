using QuanLyThuVien.Model;
using QuanLyThuVien.UserControlLibrary;
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
    public class QuyDinhViewModel : BaseViewModel
    {
        public NHANVIEN NhanVien;
        public LOAINV LoaiNhanVien;

        public ICommand LoaiSachCommand { get; set; }

        public ICommand LoaiNhanVienCommand { get; set; }
        public ICommand LoaiDocGiaCommand { get; set; }

        private ObservableCollection<QUYDINH> _List;
        public ObservableCollection<QUYDINH> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private QUYDINH _SelectedItem;
        public QUYDINH SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();

                if (SelectedItem != null)
                {
                    MaQD = SelectedItem.MaQD;
                    TenQD = SelectedItem.TenQD;
                    GiaTri = SelectedItem.GiaTri;
                }
            }
        }

        private int _MaQD;
        public int MaQD { get => _MaQD; set { _MaQD = value; OnPropertyChanged(); } }

        private string _TenQD;
        public string TenQD { get => _TenQD; set { _TenQD = value; OnPropertyChanged(); } }

        private string _GiaTri;
        public string GiaTri { get => _GiaTri; set { _GiaTri = value; OnPropertyChanged(); } }

        public ICommand EditCommand { get; set; }

        public QuyDinhViewModel(NHANVIEN _NhanVien)
        {
            NhanVien = _NhanVien;
            LoaiNhanVien = DataProvider.Ins.DB.LOAINVs.Where(x => x.MaLNV == NhanVien.MaLNV).SingleOrDefault();
            LoaiSachCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoaiSachWindow wd = new LoaiSachWindow(NhanVien); wd.ShowDialog(); });

            LoaiNhanVienCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoaiNhanVienWindow wd = new LoaiNhanVienWindow(NhanVien); wd.ShowDialog(); });
            LoaiDocGiaCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoaiDocGiaWindow wd = new LoaiDocGiaWindow(NhanVien); wd.ShowDialog(); });

            List = new ObservableCollection<QUYDINH>(DataProvider.Ins.DB.QUYDINHs);

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(GiaTri))
                {
                    return false;
                }

                var qdList = DataProvider.Ins.DB.QUYDINHs.Where(x => x.TenQD == TenQD && x.GiaTri == GiaTri);

                if (qdList == null || qdList.Count() != 0)
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
                try
                {
                    int gt = int.Parse(GiaTri);

                    if (TenQD == "Số sách mượn tối đa" && gt < 1)
                    {
                        MessageBox.Show("Số sách mượn tối đa không được nhỏ hơn 1 cuốn!");
                        return;
                    }

                    if (TenQD == "Số ngày mượn tối đa" && gt < 1)
                    {
                        MessageBox.Show("Số ngày mượn tối đa không được nhỏ hơn 1 ngày!");
                        return;
                    }

                    if (TenQD == "Số tuổi tối thiểu")
                    {
                        var tuoiToiDa = DataProvider.Ins.DB.QUYDINHs.Where(x => x.TenQD == "Số tuổi tối đa").SingleOrDefault();

                        if (gt > int.Parse(tuoiToiDa.GiaTri))
                        {
                            MessageBox.Show("Số tuổi tối thiểu không được nhỏ hơn số tuổi tối đa!");
                            return;
                        }

                        if (gt < 6)
                        {
                            MessageBox.Show("Số tuổi tối thiểu không được nhỏ hơn 6 tuổi!");
                            return;
                        }
                    }

                    if (TenQD == "Số tuổi tối đa")
                    {
                        var tuoiToiThieu = DataProvider.Ins.DB.QUYDINHs.Where(x => x.TenQD == "Số tuổi tối thiểu").SingleOrDefault();

                        if (gt < int.Parse(tuoiToiThieu.GiaTri))
                        {
                            MessageBox.Show("Số tuổi tối đa không được nhỏ hơn số tuổi tối thiểu!");
                            return;
                        }
                    }

                    if (TenQD == "Thời hạn năm sản xuất tối đa" && gt < 1)
                    {
                        MessageBox.Show("Thời hạn năm sản xuất tối đa không được nhỏ hơn 1 năm!");
                        return;
                    }

                    if (TenQD == "Thời hạn thẻ độc giả" && gt < 1)
                    {
                        MessageBox.Show("Thời hạn thẻ độc giả không được nhỏ hơn 1 tháng!");
                        return;
                    }

                    MessageBoxResult rs = MessageBox.Show("Bạn có chắc chắn muốn thay đổi quy định này?", "Thông báo", MessageBoxButton.OKCancel);

                    if (rs == MessageBoxResult.OK)
                    {
                        var qd = DataProvider.Ins.DB.QUYDINHs.Where(x => x.MaQD == SelectedItem.MaQD).SingleOrDefault();

                        qd.GiaTri = gt.ToString();

                        DataProvider.Ins.DB.SaveChanges();

                        SelectedItem.GiaTri = GiaTri;
                    }
                }
                catch
                {
                    MessageBox.Show("Giá trị nhập không hợp lệ!");
                }
            });
        }
    }
}
