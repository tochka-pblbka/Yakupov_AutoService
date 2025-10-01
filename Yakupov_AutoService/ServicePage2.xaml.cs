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

namespace Yakupov_AutoService
{
    /// <summary>
    /// Логика взаимодействия для ServicePage2.xaml
    /// </summary>
    public partial class ServicePage2 : Page
    {
        public ServicePage2()
        {
            InitializeComponent();
            var currentServices = ЯкуповаАвтосервисEntities.GetContext().Service.ToList();
            ServiceListView.ItemsSource = currentServices;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEdit());
        }
    }
}
