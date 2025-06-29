using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiziVote.Data;
using DiziVote.Models;
using DiziVote.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace DiziVote.ViewModels
{
    public partial class MyRatingsViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private readonly TMDbService _tmdbService;

        [ObservableProperty]
        private ObservableCollection<RatedTVShow> _ratedShows;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(UpdateRatingCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteRatingCommand))]
        private RatedTVShow _selectedShow;

        [ObservableProperty]
        private TVShow _selectedShowDetails;

        [ObservableProperty]
        private int _userRating;

        public MyRatingsViewModel()
        {
            _databaseService = new DatabaseService(new DiziVoteDbContext());
            _tmdbService = new TMDbService();
            RatedShows = new ObservableCollection<RatedTVShow>();
            LoadRatedShowsCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadRatedShowsAsync()
        {
            var shows = await _databaseService.GetRatedShowsAsync();
            RatedShows.Clear();
            foreach (var show in shows)
            {
                RatedShows.Add(show);
            }
        }

        async partial void OnSelectedShowChanged(RatedTVShow value)
        {
            if (value != null)
            {
                SelectedShowDetails = await _tmdbService.GetShowDetailsAsync(value.Id);
                UserRating = value.UserRating;
            }
            else
            {
                SelectedShowDetails = null;
                UserRating = 0;
            }
            UpdateRatingCommand.NotifyCanExecuteChanged();
            DeleteRatingCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private void SetRating(int rating)
        {
            UserRating = rating;
        }

        private bool CanUpdateRating() => SelectedShow != null && UserRating > 0;

        [RelayCommand(CanExecute = nameof(CanUpdateRating))]
        private async Task UpdateRatingAsync()
        {
            if (SelectedShow == null || UserRating == 0) return;

            var ratedShowToUpdate = new RatedTVShow
            {
                Id = SelectedShow.Id,
                Name = SelectedShow.Name,
                PosterUrl = SelectedShowDetails?.PosterUrl,
                UserRating = UserRating
            };

            await _databaseService.AddOrUpdateRatingAsync(ratedShowToUpdate);
            MessageBox.Show("Puan başarıyla güncellendi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            await LoadRatedShowsAsync();
        }

        private bool CanDeleteRating() => SelectedShow != null;

        [RelayCommand(CanExecute = nameof(CanDeleteRating))]
        private async Task DeleteRatingAsync()
        {
            if (SelectedShow == null) return;

            var result = MessageBox.Show($"'{SelectedShow.Name}' dizisini puan listenizden kaldırmak istediğinize emin misiniz?", "Puanı Sil", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _databaseService.DeleteRatingAsync(SelectedShow.Id);
                MessageBox.Show("Puan başarıyla silindi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadRatedShowsAsync();
            }
        }
    }
}
