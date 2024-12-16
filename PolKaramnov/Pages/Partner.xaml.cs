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

namespace PolKaramnov.Pages
{
    /// <summary>
    /// Логика взаимодействия для Partner.xaml
    /// </summary>
    public partial class Partner : Page
    {
        public Partner()
        {
            InitializeComponent();
            DataGridUser.ItemsSource = KaramnovPolEntities.GetContext().Partners.ToList();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AddPartner(null));
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            List<Partners> usersForRemoving = DataGridUser.SelectedItems.Cast<Partners>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {usersForRemoving.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    KaramnovPolEntities.GetContext().Partners.RemoveRange(usersForRemoving);
                    KaramnovPolEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");

                    DataGridUser.ItemsSource = KaramnovPolEntities.GetContext().Partners.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}");
                }
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AddPartner((sender as Button).DataContext as Partners));
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                KaramnovPolEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                DataGridUser.ItemsSource = KaramnovPolEntities.GetContext().Partners.ToList();
            }
        }
    }
}
