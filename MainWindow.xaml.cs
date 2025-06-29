using System.Linq;
using System.Windows;

namespace DiziVote
{
    public static class RatingNumbers
    {
        public static System.Collections.Generic.IEnumerable<int> TenStars { get; } = System.Linq.Enumerable.Range(1, 10).ToList();
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                DataContext = new DiziVote.ViewModels.MainViewModel();
            }
            catch (System.Exception ex)
            {
                var fullExceptionDetails = ex.ToString();
                System.Windows.MessageBox.Show($"Uygulama başlatılırken kritik bir hata oluştu ve kapatılacak:\n\n{fullExceptionDetails}", 
                                     "Kritik Başlatma Hatası", 
                                     System.Windows.MessageBoxButton.OK, 
                                     System.Windows.MessageBoxImage.Error);
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void OpenMyRatings_Click(object sender, RoutedEventArgs e)
        {
            var myRatingsView = new MyRatingsView();
            myRatingsView.Owner = this;
            myRatingsView.ShowDialog();
        }
    }
}
