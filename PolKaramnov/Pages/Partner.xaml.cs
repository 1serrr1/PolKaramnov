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

            ListBoxPartners.ItemsSource = KaramnovPolEntities3.GetContext()
         .Partners
         .ToList()
         .Select(p => new PartnerDiscountViewModel
         {
             IdPartners = p.IdPartners,
             NamePartners = p.NamePartners,
             Director = p.Director,  
             Phone = p.Phone,
             Rating = p.Rating, 
             TypePartners = p.TypePartner.NameType,
             TotalSales = p.PartnerProducts.Sum(pp => pp.Quantity),  
             Discount = CalculateDiscount(p.PartnerProducts.Sum(pp => pp.Quantity))
         })
         .ToList();
        }
        private string CalculateDiscount(decimal totalSales)
        {
            if (totalSales > 300000) return "15%";
            if (totalSales > 50000) return "10%";
            if (totalSales > 10000) return "5%";
            return "0%";
        }

        // Класс для хранения данных о партнерах и их скидках
        public class PartnerDiscountViewModel
        {
            public int IdPartners { get; set; }
            public string NamePartners { get; set; }
            public decimal TotalSales { get; set; }
            public decimal Rating { get; set; }
            public string Discount { get; set; }
            public string Director { get; set; }
            public string Phone { get; set; }
            public string TypePartners { get; set; }
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AddPartner(null));
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            List<PartnerDiscountViewModel> selectedPartners = ListBoxPartners.SelectedItems.Cast<PartnerDiscountViewModel>().ToList();

            if (selectedPartners.Any())
            {

                if (MessageBox.Show($"Вы точно хотите удалить {selectedPartners.Count} партнеров?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        foreach (var partner in selectedPartners)
                        {
                            var partnerToRemove = KaramnovPolEntities3.GetContext().Partners
                                .FirstOrDefault(p => p.IdPartners == partner.IdPartners);
                            if (partnerToRemove != null)
                            {
                                KaramnovPolEntities3.GetContext().Partners.Remove(partnerToRemove);
                            }
                        }

                        KaramnovPolEntities3.GetContext().SaveChanges();

                        MessageBox.Show("Партнеры успешно удалены!");

                        ListBoxPartners.ItemsSource = KaramnovPolEntities3.GetContext()
                            .Partners
                            .ToList()
                            .Select(p => new PartnerDiscountViewModel
                            {
                                IdPartners = p.IdPartners,
                                NamePartners = p.NamePartners,
                                Director = p.Director,
                                Phone = p.Phone,
                                Rating = p.Rating,
                                TypePartners = p.TypePartner.NameType,
                                TotalSales = p.PartnerProducts.Sum(pp => (decimal)(pp.Quantity * pp.Product.MinCostPartners)),
                                Discount = CalculateDiscount(p.PartnerProducts.Sum(pp => (decimal)(pp.Quantity * pp.Product.MinCostPartners)))
                            })
                            .ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите хотя бы одного партнера для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                KaramnovPolEntities3.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                ListBoxPartners.ItemsSource = KaramnovPolEntities3.GetContext().Partners.ToList();
            }
        }

        private void ButtonHistory_Click(object sender, RoutedEventArgs e)
        {
            var selectedPartner = ListBoxPartners.SelectedItem as PartnerDiscountViewModel;

            if (selectedPartner != null)
            {
                NavigationService.Navigate(new Spisok(selectedPartner.IdPartners));
            }
            else
            {
                MessageBox.Show("Выберите партнера для просмотра истории реализации.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void ListBoxPartners_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedPartner = ListBoxPartners.SelectedItem as PartnerDiscountViewModel;
            if (selectedPartner != null)
            {

                var partner = KaramnovPolEntities3.GetContext().Partners
                    .FirstOrDefault(p => p.IdPartners == selectedPartner.IdPartners);
                if (partner != null)
                {
                    NavigationService.Navigate(new Pages.AddPartner(partner));
                }
            }
            else
            {
                MessageBox.Show("Выберите партнёра для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        
    }
}