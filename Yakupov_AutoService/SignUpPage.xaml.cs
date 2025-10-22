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
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();

        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                this._currentService = SelectedService;

            DataContext = _currentService;
            var _currentClient = ЯкуповаАвтосервисEntities.GetContext().Client.ToList();

            ComboClient.ItemsSource = _currentClient;
        }
        private ClientService _currentClientService = new ClientService();

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");
            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");
            if (TBStart.Text == "")
                errors.AppendLine("Укажите время начала услуги");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            //_currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            Client selectedClient = ComboClient.SelectedItem as Client;
            _currentClientService.ClientID = selectedClient.ID;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);

            if (_currentClientService.ID == 0)
                ЯкуповаАвтосервисEntities.GetContext().ClientService.Add(_currentClientService);

            try
            {
                ЯкуповаАвтосервисEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;
            string[] start = s.Split(':');
            if (start.Length != 2)
            {
                TBend.Text = "Неверный формат времени";
                return;
            }
            if(s.Length !=5 || s[2] != ':')
            {
                TBend.Text = "Неверный формат времени надо (чч:мм)";
                return;
            }
            if(!int.TryParse(start[0], out int startHour) || !int.TryParse(start[1], out int startMin))
            {
                TBend.Text = "Неверный формат времени";
                return;
            }
            if(startHour<0 || startHour >23 || startMin < 0 || startMin > 59)
            {
                TBend.Text = "Неверный формат времени";
                return;
            }
            int totalMin = startHour * 60 + startMin + _currentService.Duration;
            int EndHour = totalMin / 60;
            int EndMin = totalMin % 60;
            EndHour = EndHour % 24;
            s = EndHour.ToString("D2") + ":" + EndMin.ToString("D2");
            TBend.Text = s;
        }

    }
}
