using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Win32;
using System.IO;       // Для File

namespace Goman_WPF_PROJ_UP02
{
    public partial class AddEditAdPage : Page
    {
        private Advertisements _currentAd = new Advertisements();
        private bool _isEditMode = false;

        public AddEditAdPage(Advertisements selectedAd)
        {
            InitializeComponent();

            if (selectedAd != null)
            {
                _currentAd = selectedAd;
                _isEditMode = true;
            }
            else
            {
                _currentAd.Price = 0;
                _currentAd.Ad_post_date = DateTime.Now;
                _currentAd.User_Id = App.CurrentUser.Id; // привязываем к текущему юзеру
                _isEditMode = false;
            }

            DataContext = _currentAd;

            ComboCity.ItemsSource = AdsServiceDBEntities.GetContext().Cities.ToList();
            ComboCategory.ItemsSource = AdsServiceDBEntities.GetContext().Categories.ToList();
            ComboType.ItemsSource = AdsServiceDBEntities.GetContext().Ad_Types.ToList();
            ComboStatus.ItemsSource = AdsServiceDBEntities.GetContext().Ad_Statuses.ToList();

            if (_isEditMode)
            {
                ComboCity.SelectedItem = _currentAd.Cities;
                ComboCategory.SelectedItem = _currentAd.Categories;
                ComboType.SelectedItem = _currentAd.Ad_Types;
                ComboStatus.SelectedItem = _currentAd.Ad_Statuses;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentAd.Ad_title))
                errors.AppendLine("Укажите заголовок объявления.");
            if (_currentAd.Cities == null && ComboCity.SelectedItem == null)
                errors.AppendLine("Выберите город.");
            if (_currentAd.Categories == null && ComboCategory.SelectedItem == null)
                errors.AppendLine("Выберите категорию.");
            if (_currentAd.Ad_Types == null && ComboType.SelectedItem == null)
                errors.AppendLine("Выберите тип.");
            if (_currentAd.Ad_Statuses == null && ComboStatus.SelectedItem == null)
                errors.AppendLine("Выберите статус.");

            // предварительная обработка текста - меняем точку на запятую и убираем лишние пробелы
            string rawPrice = TxtPrice.Text.Replace(".", ",").Replace(" ", "").Trim();

            if (!decimal.TryParse(rawPrice, out decimal price) || price < 0)
            {
                errors.AppendLine("Стоимость должна быть неотрицательным числом.");
            }
            else
            {
                _currentAd.Price = price;
            }



            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _currentAd.Cities = ComboCity.SelectedItem as Cities;
            _currentAd.Categories = ComboCategory.SelectedItem as Categories;
            _currentAd.Ad_Types = ComboType.SelectedItem as Ad_Types;
            _currentAd.Ad_Statuses = ComboStatus.SelectedItem as Ad_Statuses;

            _currentAd.City_Id = _currentAd.Cities.Id;
            _currentAd.Category_Id = _currentAd.Categories.Id;
            _currentAd.Ad_type_Id = _currentAd.Ad_Types.Id;
            _currentAd.Ad_status_Id = _currentAd.Ad_Statuses.Id;

            if (_currentAd.Ad_Statuses.Name == "Завершено")
            {

                MessageBoxResult result = MessageBox.Show($"Объявление завершено. Полученная прибыль составит {_currentAd.Price}. Верно?",
                    "Подтверждение прибыли", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                    return; 
            }

            if (_currentAd.Id == 0)
            {
                AdsServiceDBEntities.GetContext().Advertisements.Add(_currentAd);
            }

            try
            {
                AdsServiceDBEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ComboStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboStatus.SelectedItem is Ad_Statuses selectedStatus && selectedStatus.Name == "Завершено")
            {
                MessageBox.Show("Вы выбрали статус 'Завершено'. Пожалуйста, убедитесь, что в поле 'Стоимость' указана итоговая полученная прибыль.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Считываем файл в массив байтов
                    _currentAd.Ad_photo = File.ReadAllBytes(openFileDialog.FileName);

                    // Обновляем превью на форме (WPF сам поймет, как отобразить byte[] в Image)
                    ImgPreview.Source = (System.Windows.Media.ImageSource)new ImageSourceConverter().ConvertFrom(_currentAd.Ad_photo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки фото: {ex.Message}");
                }
            }
        }
    }
}
