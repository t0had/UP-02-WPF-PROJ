using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Goman_WPF_PROJ_UP02
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = TxtLogin.Text.Trim();
            string password = PswPassword.Password.Trim();

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var user = AdsServiceDBEntities.GetContext().Users
                    .FirstOrDefault(u => u.User_login == login && u.User_password == password);

                if (user != null)
                {
                    MessageBox.Show($"Добро пожаловать, {user.User_login}!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    NavigationService.Navigate(new AdsPage());
                    App.CurrentUser = user;
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnLoginWithoutAuth_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdsPage());
            App.CurrentUser = null;
        }
    }
}
