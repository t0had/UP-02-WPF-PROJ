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

namespace Goman_WPF_PROJ_UP02
{
    /// <summary>
    /// Логика взаимодействия для AdsPage.xaml
    /// </summary>
    public partial class AdsPage : Page
    {
        public AdsPage()
        {
            InitializeComponent();
            try
            {
                ItemsControl itemsControl = new ItemsControl();

                DataGridAds.ItemsSource = AdsServiceDBEntities.GetContext().Advertisements.ToList();
                CityCheck.ItemsSource = AdsServiceDBEntities.GetContext().Cities.ToList();
                CategoryCheck.ItemsSource = AdsServiceDBEntities.GetContext().Categories.ToList();
                TypeCheck.ItemsSource = AdsServiceDBEntities.GetContext().Ad_Types.ToList();
                StatusCheck.ItemsSource = AdsServiceDBEntities.GetContext().Ad_Statuses.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void UpdateContent()
        {
            try
            {
                var currentData = AdsServiceDBEntities.GetContext().Advertisements.ToList();
                if (CityCheck.SelectedItem != null)
                {
                    Cities city = (Cities)CityCheck.SelectedItem;
                    currentData = currentData.Where(x => x.City_Id.Equals(city.Id)).ToList();
                }
                if (CategoryCheck.SelectedItem != null)
                {
                    Categories category = (Categories)CategoryCheck.SelectedItem;
                    currentData = currentData.Where(x => x.Category_Id.Equals(category.Id)).ToList();
                }
                if (TypeCheck.SelectedItem != null)
                {
                    Ad_Types ad_Types = (Ad_Types)TypeCheck.SelectedItem;
                    currentData = currentData.Where(x => x.Ad_type_Id.Equals(ad_Types.Id)).ToList();
                }
                if (StatusCheck.SelectedItem != null)
                {
                    Ad_Statuses ad_Statuses = (Ad_Statuses)StatusCheck.SelectedItem;
                    currentData = currentData.Where(x => x.Ad_status_Id.Equals(ad_Statuses.Id)).ToList();
                }
                DataGridAds.ItemsSource = currentData;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbCheck_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateContent();
        }

        private void ButtonClearFilter_Click(object sender, RoutedEventArgs e)
        {
            CityCheck.SelectedItem = null;
            CategoryCheck.SelectedItem = null;
            TypeCheck.SelectedItem = null;
            StatusCheck.SelectedItem = null;
        }
    }
}
