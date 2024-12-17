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
    /// Логика взаимодействия для AddPartner.xaml
    /// </summary>
    public partial class AddPartner : Page
    {
        private Partners _currentUser = new Partners();
        public AddPartner(Partners selectedUser)
        {
            InitializeComponent();
            if (selectedUser != null)
                _currentUser = selectedUser;

            DataContext = _currentUser;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (_currentUser.TypePartners == 0)
                errors.AppendLine("Укажите тип партнёра!");
            if (string.IsNullOrWhiteSpace(_currentUser.NamePartners))
                errors.AppendLine("Укажите название партнёра!");
            if (string.IsNullOrWhiteSpace(_currentUser.Director))
                errors.AppendLine("Укажите имя директора!");
            if (string.IsNullOrWhiteSpace(_currentUser.Mail) || !IsValidEmail(_currentUser.Mail))
                errors.AppendLine("Укажите корректный адрес электронной почты!");
            if (string.IsNullOrWhiteSpace(_currentUser.Phone) || !IsValidPhoneNumber(_currentUser.Phone))
                errors.AppendLine("Укажите корректный номер телефона!");
            if (string.IsNullOrWhiteSpace(_currentUser.Address))
                errors.AppendLine("Укажите адрес!");
            if (string.IsNullOrWhiteSpace(_currentUser.INN) || !IsValidINN(_currentUser.INN))
                errors.AppendLine("Укажите корректный ИНН!");
            if (_currentUser.Rating < 0 || _currentUser.Rating > 5)
                errors.AppendLine("Рейтинг должен быть в диапазоне от 0 до 5!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentUser.IdPartners == 0)
                KaramnovPolEntities2.GetContext().Partners.Add(_currentUser);

            try
            {
                KaramnovPolEntities2.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Проверка номера телефона (должен содержать только цифры и быть длиной 10-15 символов)
        private bool IsValidPhoneNumber(string phone)
        {
            return phone.All(char.IsDigit) && phone.Length >= 10 && phone.Length <= 15;
        }

        // Проверка ИНН (должен содержать только цифры и быть длиной 10 или 12 символов)
        private bool IsValidINN(string inn)
        {
            return inn.All(char.IsDigit) && (inn.Length == 10 || inn.Length == 12);
        }
    }
}
