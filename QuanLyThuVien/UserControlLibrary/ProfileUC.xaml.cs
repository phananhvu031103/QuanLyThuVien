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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyThuVien.UserControlLibrary
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class ProfileUC : UserControl
    {
        private ProfileViewModel Viewmodel { get; set; }

        public ProfileUC()
        {
            InitializeComponent();
        }

        public ProfileUC(NHANVIEN NhanVien)
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new ProfileViewModel(NhanVien);
        }
    }
}
