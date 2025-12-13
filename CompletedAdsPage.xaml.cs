using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Goman_WPF_PROJ_UP02
{
    public partial class CompletedAdsPage : Page
    {
        public CompletedAdsPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                int currentUserId = App.CurrentUser.Id;

                var completedAds = AdsServiceDBEntities.GetContext().Advertisements
                    .Where(ad => ad.User_Id == currentUserId && ad.Ad_Statuses.Name == "Завершено")
                    .ToList();

                DataGridCompleted.ItemsSource = completedAds;

                decimal totalProfit = completedAds.Sum(ad => ad.Price);

                TxtTotalSum.Text = totalProfit.ToString("N2") + " ₽";

                if (completedAds.Count == 0)
                {
                    MessageBox.Show("У вас пока нет завершенных объявлений.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
