using System.Windows;

namespace CustomPcStoreApp.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        // Временная заглушка - будет реализовано в задаче 4
        MessageBox.Show("Функция входа будет реализована в следующих задачах", "Информация", 
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}