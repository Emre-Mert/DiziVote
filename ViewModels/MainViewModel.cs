using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiziVote.Data;
using DiziVote.Models;
using DiziVote.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DiziVote.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly TMDbService _tmdbService;
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private string _searchQuery;

        [ObservableProperty]
        private ObservableCollection<TVShow> _shows;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RateShowCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteRatingCommand))]
        private TVShow _selectedShow;

        [ObservableProperty]
        private int _userRating;

        [ObservableProperty]
        private string _previousRatingText;

        public MainViewModel()
        {
            _tmdbService = new TMDbService();
            _databaseService = new DatabaseService(new DiziVoteDbContext());
            Shows = new ObservableCollection<TVShow>();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            try
            {
                await LoadPopularShowsAsync();
            }
            catch (Exception ex)
            {
                var fullExceptionMessage = new System.Text.StringBuilder();
                var currentEx = ex;
                while (currentEx != null)
                {
                    fullExceptionMessage.AppendLine(currentEx.GetType().FullName);
                    fullExceptionMessage.AppendLine(currentEx.Message);
                    fullExceptionMessage.AppendLine(currentEx.StackTrace);
                    fullExceptionMessage.AppendLine("---");
                    currentEx = currentEx.InnerException;
                }
                MessageBox.Show($"Uygulama başlatılırken bir hata oluştu:\n\n{fullExceptionMessage.ToString()}", "Başlatma Hatası", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        [RelayCommand]
        private async Task SearchShowsAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery)) return;
            var result = await _tmdbService.SearchShowsAsync(SearchQuery);
            Shows.Clear();
            if (result != null)
            {
                foreach (var show in result)
                {
                    Shows.Add(show);
                }
            }
        }

        [RelayCommand]
        private async Task LoadPopularShowsAsync()
        {
            var result = await _tmdbService.GetPopularShowsAsync();
            Shows.Clear();
            if (result != null)
            {
                foreach (var show in result)
                {
                    Shows.Add(show);
                }
            }
        }

        [RelayCommand]
        private void SetRating(int rating)
        {
            UserRating = rating;
        }

        private bool CanRateShow() => SelectedShow != null;

        [RelayCommand(CanExecute = nameof(CanRateShow))]
        private async Task RateShowAsync()
        {
            if (SelectedShow == null || UserRating == 0) return;

            var ratedShow = new RatedTVShow
            {
                Id = SelectedShow.Id,
                Name = SelectedShow.Name,
                PosterUrl = SelectedShow.PosterUrl,
                UserRating = UserRating
            };

            await _databaseService.AddOrUpdateRatingAsync(ratedShow);
            MessageBox.Show($"'{SelectedShow.Name}' dizisine {UserRating} puan verdiniz.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            await LoadRatingForSelectedShowAsync(SelectedShow);
        }

        private bool CanDeleteRating() => SelectedShow != null && !string.IsNullOrEmpty(PreviousRatingText) && PreviousRatingText.StartsWith("Daha önce");

        [RelayCommand(CanExecute = nameof(CanDeleteRating))]
        private async Task DeleteRatingAsync()
        {
            if (SelectedShow == null) return;

            var result = MessageBox.Show($"'{SelectedShow.Name}' dizisinin puanını silmek istediğinize emin misiniz?", "Puanı Sil", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _databaseService.DeleteRatingAsync(SelectedShow.Id);
                MessageBox.Show("Puan başarıyla silindi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadRatingForSelectedShowAsync(SelectedShow);
            }
        }

        async partial void OnSelectedShowChanged(TVShow value)
        {
            await LoadRatingForSelectedShowAsync(value);
        }

        private async Task LoadRatingForSelectedShowAsync(TVShow show)
        {
            if (show == null)
            {
                PreviousRatingText = string.Empty;
                return;
            }

            var rating = await _databaseService.GetRatingForShowAsync(show.Id);
            if (rating > 0)
            {
                PreviousRatingText = $"Daha önce bu diziye {rating} puan verdiniz.";
                UserRating = rating;
            }
            else
            {
                PreviousRatingText = string.Empty;
                UserRating = 5;
            }
            DeleteRatingCommand.NotifyCanExecuteChanged();
        }
    }
}
