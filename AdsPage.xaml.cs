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
            DataGridAds.ItemsSource = AdsServiceDBEntities.GetContext().Advertisements.ToList();
            CityCheck.ItemsSource = AdsServiceDBEntities.GetContext().Cities.ToList();
            CategoryCheck.ItemsSource = AdsServiceDBEntities.GetContext().Categories.ToList();
            TypeCheck.ItemsSource = AdsServiceDBEntities.GetContext().Ad_Types.ToList();
            StatusCheck.ItemsSource = AdsServiceDBEntities.GetContext().Ad_Statuses.ToList();
        }
        private void UpdateCmb()
        {

        }
    }
}
