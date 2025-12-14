using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Goman_WPF_PROJ_UP02
{
    public partial class AdsPage : Page
    {
        public AdsPage()
        {
            InitializeComponent();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                AdsServiceDBEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                UpdateContent();
                LoadCombos();

                if (App.CurrentUser == null)
                {
                    BtnAdd.Visibility = Visibility.Collapsed;
                    BtnDelete.Visibility = Visibility.Collapsed;
                    BtnEdit.Visibility = Visibility.Collapsed;
                    BtnHistory.Visibility = Visibility.Collapsed;
                }
                else
                {
                    BtnAdd.Visibility = Visibility.Visible;
                    BtnDelete.Visibility = Visibility.Visible;
                    BtnEdit.Visibility = Visibility.Visible;
                    BtnHistory.Visibility = Visibility.Visible;
                }
            }
        }

        private void LoadCombos()
        {
            CityCheck.ItemsSource = AdsServiceDBEntities.GetContext().Cities.ToList();
            CategoryCheck.ItemsSource = AdsServiceDBEntities.GetContext().Categories.ToList();
            TypeCheck.ItemsSource = AdsServiceDBEntities.GetContext().Ad_Types.ToList();
            StatusCheck.ItemsSource = AdsServiceDBEntities.GetContext().Ad_Statuses.ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditAdPage(null));
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridAds.SelectedItem is Advertisements selectedAd)
            {
                if (selectedAd.User_Id == App.CurrentUser.Id)
                {
                    NavigationService.Navigate(new AddEditAdPage(selectedAd));
                }
                else
                {
                    MessageBox.Show("Вы можете редактировать только собственные объявления.", "Доступ запрещен", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Выберите объявление для редактирования.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedAds = DataGridAds.SelectedItems.Cast<Advertisements>().ToList();

            if (selectedAds.Count == 0)
            {
                MessageBox.Show("Выберите элементы для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedAds.Any(ad => ad.User_Id != App.CurrentUser.Id))
            {
                MessageBox.Show("Среди выбранных элементов есть чужие объявления. Удаление невозможно.", "Ошибка доступа", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show($"Вы точно хотите удалить следующие {selectedAds.Count} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    AdsServiceDBEntities.GetContext().Advertisements.RemoveRange(selectedAds);
                    AdsServiceDBEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateContent(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка при удалении", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DataGridAds_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataGridAds.SelectedItem is Advertisements selectedAd)
            {
                if (selectedAd.User_Id == App.CurrentUser.Id)
                    NavigationService.Navigate(new AddEditAdPage(selectedAd));
                else
                    MessageBox.Show("Это не ваше объявление.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CompletedAdsPage());
        }

    }
}
