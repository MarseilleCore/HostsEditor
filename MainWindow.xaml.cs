using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HostsEditor
{
    public partial class MainWindow : Window
    {
        private readonly string hostsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts");

        public MainWindow()
        {
            InitializeComponent();
            LoadHosts();
        }

        private void LoadHosts()
        {
            listBoxHosts.Items.Clear();
            if (File.Exists(hostsPath))
                foreach (var line in File.ReadAllLines(hostsPath))
                    listBoxHosts.Items.Add(line);
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            string ip = textBoxIP.Text.Trim();
            string domain = textBoxDomain.Text.Trim();

            if (!string.IsNullOrEmpty(ip) && ip != "IP адрес" &&
                !string.IsNullOrEmpty(domain) && domain != "Домен")
            {
                listBoxHosts.Items.Add($"{ip} {domain}");
                ResetTextBoxes();
            }
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
{
    var selectedItems = listBoxHosts.SelectedItems.Cast<object>().ToList();
    if (selectedItems.Count == 0)
    {
        MessageBox.Show("Выберите строки для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }

    foreach (var item in selectedItems)
        listBoxHosts.Items.Remove(item);
}


        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.WriteAllLines(hostsPath, listBoxHosts.Items.Cast<string>());
                MessageBox.Show("Файл hosts сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonReload_Click(object sender, RoutedEventArgs e)
        {
            LoadHosts();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null && (tb.Text == "IP адрес" || tb.Text == "Домен"))
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null && string.IsNullOrWhiteSpace(tb.Text))
            {
                if (tb.Name == "textBoxIP") tb.Text = "IP адрес";
                if (tb.Name == "textBoxDomain") tb.Text = "Домен";
                tb.Foreground = Brushes.Gray;
            }
        }

        private void ResetTextBoxes()
        {
            textBoxIP.Text = "IP адрес";
            textBoxIP.Foreground = Brushes.Gray;
            textBoxDomain.Text = "Домен";
            textBoxDomain.Foreground = Brushes.Gray;
        }
    }
}
