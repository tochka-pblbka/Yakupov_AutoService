﻿using System;
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
            ComboType.SelectedIndex = 0;
            UpdateServices();
        }
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;

        List<Service> CurrentPageList = new List<Service>();
        List<Service> TableList;

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }
        private void ComboType_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            UpdateServices();
        }
        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }
        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }
        private void UpdateServices()
        {
            var currentServices = ЯкуповаАвтосервисEntities.GetContext().Service.ToList();
            if (ComboType.SelectedIndex == 0)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.DiscountInt) >= 0 && Convert.ToInt32(p.DiscountInt) <= 100)).ToList();
            }
            if (ComboType.SelectedIndex == 1)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.DiscountInt) >= 0 && Convert.ToInt32(p.DiscountInt) < 5)).ToList();

            }

            if (ComboType.SelectedIndex == 2)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.DiscountInt) >= 5 && Convert.ToInt32(p.DiscountInt) < 15)).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.DiscountInt) >= 15 && Convert.ToInt32(p.DiscountInt) < 30)).ToList();
            }
            if (ComboType.SelectedIndex == 4)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.DiscountInt) >= 30 && Convert.ToInt32(p.DiscountInt) < 70)).ToList();
            }
            if (ComboType.SelectedIndex == 5)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.DiscountInt) >= 70 && Convert.ToInt32(p.DiscountInt) < 100)).ToList();
            }
            currentServices = currentServices.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            ServiceListView.ItemsSource = currentServices.ToList();

            if (RButtonDown.IsChecked.Value)
            {
                currentServices = currentServices.OrderByDescending(p => p.Cost).ToList();

            }

            if (RButtonUp.IsChecked.Value)
            {
                currentServices = currentServices.OrderBy(p => p.Cost).ToList();
            }
            ServiceListView.ItemsSource = currentServices;
            TableList = currentServices;
            ChangePage(0, 0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEdit(null));
        }



        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            
            var currentService = (sender as Button).DataContext as Service;
            var currentClientServices = ЯкуповаАвтосервисEntities.GetContext().ClientService.ToList();
            currentClientServices = currentClientServices.Where(p => p.ServiceID == currentService.ID).ToList();

            if (currentClientServices.Count != 0)
                MessageBox.Show("Невозможно выполнить удаление, так как существуют записи на эту услугу");
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        ЯкуповаАвтосервисEntities.GetContext().Service.Remove(currentService);
                        ЯкуповаАвтосервисEntities.GetContext().SaveChanges();
                        ServiceListView.ItemsSource = ЯкуповаАвтосервисEntities.GetContext().Service.ToList();
                        UpdateServices();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;
            if(CountRecords % 10 > 0)
            {
                CountPage = CountRecords / 10 + 1;
            }
            else
            {
                CountPage = CountRecords / 10;
            }
            Boolean Ifupdate = true;
            int min;
            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for (int i = CurrentPage * 10; i < min; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch(direction)
                {
                    case 1:
                        if(CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage*10; i<min;i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }
            }
            if(Ifupdate)
            {
                PageListBox.Items.Clear();
                for(int i=1;i<=CountPage;i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;
                min = CurrentPage*10+10<CountRecords ? CurrentPage * 10+10 : CountRecords;
                TBCount.Text = min.ToString();
                TBAllRecords.Text = " из " + CountRecords.ToString();
                ServiceListView.ItemsSource = CurrentPageList;
                ServiceListView.Items.Refresh();
            }
               
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString())-1);

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEdit(null));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEdit((sender as Button).DataContext as Service));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(Visibility == Visibility.Visible)
            {
                ЯкуповаАвтосервисEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ServiceListView.ItemsSource = ЯкуповаАвтосервисEntities.GetContext().Service.ToList();
            }
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new SignUpPage((sender as Button).DataContext as Service));
        }
    }
}
