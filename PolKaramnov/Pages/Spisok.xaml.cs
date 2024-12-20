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
    /// Логика взаимодействия для Spisok.xaml
    /// </summary>
    public partial class Spisok : Page
    {
        private int _partnerId;
        public Spisok(int partnerId)
        {
            InitializeComponent();
            _partnerId = partnerId;

            LoadHistoryData();
        }
        private void LoadHistoryData()
        {

            var history = KaramnovPolEntities3.GetContext()
                             .PartnerProducts
                             .Where(h => h.NamePartner == _partnerId)
                            .Select(h => new
                            {
                                Название_Партнера = h.Partners.NamePartners,
                                Название_Продукта = h.Product.NameProducts,         
                                Количество = h.Quantity,                     
                                Дата_Продажи = h.DateSale

                            })
                     .ToList();

            DataGridHistory.ItemsSource = history;
        }

        private void ButtonCalculateMaterial_Click(object sender, RoutedEventArgs e)
        {

            decimal totalMaterialRequired = CalculateMaterialRequired(_partnerId);

            if (totalMaterialRequired == 0)
            {
                MessageBox.Show("У партнера нет продукции для расчета материала.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show($"Необходимое количество материала: {Math.Ceiling(totalMaterialRequired)} единиц с учетом брака.", "Рассчет материала", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private decimal CalculateMaterialRequired(int partnerId)
        {
            var partnerProducts = KaramnovPolEntities3.GetContext().PartnerProducts
                .Where(pp => pp.NamePartner == partnerId)
                .ToList();

            if (partnerProducts.Count == 0)
            {
                return 0; 
            }

            decimal totalMaterialRequired = 0;

            foreach (var partnerProduct in partnerProducts)
            {
                var product = partnerProduct.Product;

                var productType = product.ProductType;
                var typeFactor = productType?.CoefficientTypeProducts;

                decimal defectPercentage = typeFactor.HasValue ? (decimal)typeFactor.Value : 1;

                decimal materialPerProduct = 1; 

                totalMaterialRequired += materialPerProduct * partnerProduct.Quantity;
                totalMaterialRequired *= (1 + defectPercentage / 100);
            }

            return totalMaterialRequired;
        }
    }
}
