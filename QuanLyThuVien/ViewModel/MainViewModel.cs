using QuanLyThuVien.UserControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using QuanLyThuVien.Model;

namespace QuanLyThuVien.ViewModel
{
    public class MainViewModel:BaseViewModel
    {
        public bool IsLoaded = false;
        public NHANVIEN NhanVien;
        public int previousUC = 0;
        public ICommand LoadedWindowCommand { get; set; }


        private Visibility _ButtonOpenMenu;
        public Visibility ButtonOpenMenu { get => _ButtonOpenMenu; set { _ButtonOpenMenu = value; OnPropertyChanged(); } }
        public ICommand OpenMenuCommand { get; set; }

        private Visibility _ButtonCloseMenu;
        public Visibility ButtonCloseMenu { get => _ButtonCloseMenu; set { _ButtonCloseMenu = value; OnPropertyChanged(); } }
        public ICommand CloseMenuCommand { get; set; }

        public ICommand OpenUCCommand { get; set; }

        public ICommand OpenProfileCommand { get; set; }


        private UserControl _contentControl;
        public UserControl contentControl { get => _contentControl; set { _contentControl = value; OnPropertyChanged(); } }

        public bool Isloaded = false;
        public MainViewModel()
        {
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                IsLoaded = true;

                if (p == null)
                {
                    return;
                }

                p.Hide();

                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();

                if (loginWindow.DataContext == null)
                {
                    return;
                }

                var loginVM = loginWindow.DataContext as LoginViewModel;

                if (loginVM.IsLogin)
                {
                    NhanVien = loginVM.NhanVien;
                    contentControl = new DocGiaUC();
                    ButtonCloseMenu = Visibility.Collapsed;
                    ButtonOpenMenu = Visibility.Visible;

                    p.Show();
                }
                else
                {
                    p.Close();
                }

            });

            OpenMenuCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                ButtonCloseMenu = Visibility.Visible;
                ButtonOpenMenu = Visibility.Collapsed;
            });

            CloseMenuCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                ButtonOpenMenu = Visibility.Visible;
                ButtonCloseMenu = Visibility.Collapsed;
            });

           

            OpenProfileCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
                contentControl = new ProfileUC(NhanVien);
            });

            OpenUCCommand = new RelayCommand<ListView>((p) => { return true; }, (p) => {
                switch (((ListViewItem)((ListView)p).SelectedItem).Name)
                {
                    case "DocGia":
                        {
                            contentControl = new DocGiaUC();
                            previousUC = 0;

                            break;
                        }
                    case "PhieuMuon":
                        {
                            contentControl = new PhieuMuonUC();
                            previousUC = 1;

                            break;
                        }
                    case "Sach":
                        {
                            contentControl = new SachUC();
                            previousUC = 2;

                            break;
                        }
                    case "NhanVien":
                        {
                            contentControl = new NhanVienUC();
                            previousUC = 3;

                            break;
                        }
                    case "QuyDinh":
                        {
                            contentControl = new QuyDinhUC();
                            previousUC = 4;

                            break;
                        }                  
                    case "ThongKe":
                        {
                            contentControl = new BaoCaoThongKeUC();
                            previousUC = 5;

                            break;
                        }
                    case "DangXuat":
                        {
                            MessageBoxResult rs = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Lưu ý", MessageBoxButton.OKCancel);

                            if (rs == MessageBoxResult.OK)
                            {
                                FrameworkElement window = GetWindowParent(p);
                                var w = window as Window;

                                if (w != null)
                                {
                                    w.Hide();

                                    LoginWindow loginWindow = new LoginWindow();
                                    loginWindow.ShowDialog();

                                    if (loginWindow.DataContext == null)
                                    {
                                        return;
                                    }

                                    var loginVM = loginWindow.DataContext as LoginViewModel;

                                    if (loginVM.IsLogin)
                                    {
                                        NhanVien = loginVM.NhanVien;
                                        contentControl = new DocGiaUC();
                                        ButtonCloseMenu = Visibility.Collapsed;
                                        ButtonOpenMenu = Visibility.Visible;
                                        p.SelectedItem = p.Items[0];

                                        w.Show();
                                    }
                                    else
                                    {
                                        w.Close();
                                    }
                                }
                            }
                            else
                            {
                                p.SelectedItem = p.Items[previousUC];
                            }

                            break;
                        }
                    default:
                        break;
                }
            });
            FrameworkElement GetWindowParent(ListView p)
            {
                FrameworkElement parent = p;

                while (parent.Parent != null)
                {
                    parent = parent.Parent as FrameworkElement;
                }

                return parent;
            }
        }
    }
}