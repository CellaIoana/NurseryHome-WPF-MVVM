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
    /// Interaction logic for ParentDashboard.xaml
    /// </summary>
    public partial class ParentDashboard : Window
    {
        public ParentDashboard(int parentId)
        {
            InitializeComponent();
            DataContext = new ParentViewModel(parentId);
        }
    }
}
