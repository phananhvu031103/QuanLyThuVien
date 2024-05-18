using QuanLyThuVien.Model;
using QuanLyThuVien.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuanLyThuVien.UserControlLibrary
{
    /// <summary>
    /// Interaction logic for LoaiSachWindow.xaml
    /// </summary>
    public partial class LoaiSachWindow : Window
    {
        private LoaiSachViewModel Viewmodel { get; set; }

        public LoaiSachWindow()
        {
            InitializeComponent();
        }

        public LoaiSachWindow(NHANVIEN NhanVien)
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new LoaiSachViewModel(NhanVien);
        }
    }
}
