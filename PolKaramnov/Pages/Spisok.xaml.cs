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
            // Получение истории реализации из базы данных
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

            // Привязка данных к DataGrid
            DataGridHistory.ItemsSource = history;
        }

        private void ButtonCalculateMaterial_Click(object sender, RoutedEventArgs e)
        {
            // Получаем все продукты, связанные с выбранным партнером
            var partnerProducts = KaramnovPolEntities3.GetContext().PartnerProducts
                .Where(pp => pp.NamePartner == _partnerId)
                .ToList();

            if (partnerProducts.Count == 0)
            {
                MessageBox.Show("У партнера нет продукции для расчета материала.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            decimal totalMaterialRequired = 0;

            // Для каждого товара партнера рассчитываем необходимое количество материала
            foreach (var partnerProduct in partnerProducts)
            {
                var product = partnerProduct.Product;

                // Получаем тип продукта и коэффициент типа продукции
                var productType = product.ProductType; // Связь с типом продукции
                var typeFactor = productType?.CoefficientTypeProducts;

                decimal defectPercentage = typeFactor.HasValue ? (decimal)typeFactor.Value : 1; // Преобразование с обработкой null


                decimal materialPerProduct = 1;  

                // Допустим, вы используете количество продукции для расчета
                totalMaterialRequired += materialPerProduct * partnerProduct.Quantity;

                totalMaterialRequired += materialPerProduct * partnerProduct.Quantity;
                totalMaterialRequired *= (1 + defectPercentage / 100); // Увеличиваем на процент брака
            }

            // Выводим результат
            MessageBox.Show($"Необходимое количество материала: {Math.Ceiling(totalMaterialRequired)} единиц с учетом брака.", "Рассчет материала", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
