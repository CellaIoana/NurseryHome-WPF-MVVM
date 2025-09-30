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
    /// <summary>
    /// Interaction logic for EducatorDashboard.xaml
    /// </summary>
    public partial class EducatorDashboard : Window
    {
        public EducatorDashboard(int educatorId)
        {
            InitializeComponent();
            DataContext = new EducatorViewModel(educatorId);
        }

    }
}
