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
using NurseryHome.ViewModels;

namespace NurseryHome.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            MessageBox.Show("LoginView loaded!");

            var vm = new LoginViewModel();
            DataContext = vm;

            // Actualizăm parola când se scrie în PasswordBox
            PasswordBox.PasswordChanged += (s, e) =>
            {
                vm.Password = PasswordBox.Password;
            };
        }
    }
}
