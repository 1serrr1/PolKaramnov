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
    }
}
