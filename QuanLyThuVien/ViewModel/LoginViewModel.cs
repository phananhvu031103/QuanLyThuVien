using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using QuanLyThuVien.Model;
using System.Windows.Controls;
using System.Security.Cryptography;

namespace QuanLyThuVien.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public bool IsLogin { get; set; }

        public NHANVIEN NhanVien { get; set; }

        private string _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }
        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }


        private Visibility _UserNameVisibility;
        public Visibility UserNameVisibility { get => _UserNameVisibility; set { _UserNameVisibility = value; OnPropertyChanged(); } }
        public ICommand UserNameCommand { get; set; }

        private Visibility _HintUserNameVisibility;
        public Visibility HintUserNameVisibility { get => _HintUserNameVisibility; set { _HintUserNameVisibility = value; OnPropertyChanged(); } }
        public ICommand HintUserNameCommand { get; set; }


        private Visibility _PasswordVisibility;
        public Visibility PasswordVisibility { get => _PasswordVisibility; set { _PasswordVisibility = value; OnPropertyChanged(); } }
        public ICommand PasswordCommand { get; set; }

        private Visibility _HintPasswordVisibility;
        public Visibility HintPasswordVisibility { get => _HintPasswordVisibility; set { _HintPasswordVisibility = value; OnPropertyChanged(); } }
        public ICommand HintPasswordCommand { get; set; }

        public LoginViewModel()
        {
            IsLogin = false;
            Password = "";
            UserName = "";

            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });

            UserNameCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) => {
                if (string.IsNullOrEmpty(UserName))
                {
                    UserNameVisibility = Visibility.Collapsed;
                    HintUserNameVisibility = Visibility.Visible;
                }
            });

            HintUserNameCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) => {
                HintUserNameVisibility = Visibility.Collapsed;
                UserNameVisibility = Visibility.Visible;

                p.Focus();
            });


            PasswordCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) => {
                if (string.IsNullOrEmpty(Password))
                {
                    PasswordVisibility = Visibility.Collapsed;
                    HintPasswordVisibility = Visibility.Visible;
                }
            });

            HintPasswordCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => {
                HintPasswordVisibility = Visibility.Collapsed;
                PasswordVisibility = Visibility.Visible;

                p.Focus();
            });
        }
        void Login(Window p)
        {
            if (p == null)
            {
                return;
            }

            string passEncode = MD5Hash(Base64Encode(Password));
            var account = DataProvider.Ins.DB.NHANVIENs.Where(x => x.TenDangNhap == UserName && x.MatKhau == passEncode).ToArray();

            if (account.Count() > 0)
            {
                IsLogin = true;

                NhanVien = new NHANVIEN()
                {
                    TenNV = account[0].TenNV,
                    NgaySinh = account[0].NgaySinh,
                    DiaChi = account[0].DiaChi,
                    Email = account[0].Email,
                    MaLNV = account[0].MaLNV,
                    TenDangNhap = account[0].TenDangNhap,
                    MatKhau = account[0].MatKhau,
                };

                p.Close();
            }
            else
            {
                IsLogin = false;

                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
            }
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

    }
}
