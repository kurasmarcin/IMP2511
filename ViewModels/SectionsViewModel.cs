using IMP.Models;
using IMP.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Firebase.Auth;

namespace IMP.ViewModels
{
    public class SectionsViewModel : INotifyPropertyChanged
    {
        private readonly RealtimeDatabaseService _databaseService;

        private string _sectionName;
        private string _startTime;
        private string _duration;
        private string _selectedDays;

        public string SectionName
        {
            get => _sectionName;
            set
            {
                if (_sectionName != value)
                {
                    _sectionName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedDays
        {
            get => _selectedDays;
            set
            {
                if (_selectedDays != value)
                {
                    _selectedDays = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddSectionCommand { get; }

        public List<Section> Sections { get; private set; }

        public SectionsViewModel()
        {
            _databaseService = new RealtimeDatabaseService();
            Sections = new List<Section>();

            AddSectionCommand = new Command(async () => await OnAddSection());
        }

        private async Task OnAddSection()
        {
            try
            {
                var newSection = new Section
                {
                    Name = SectionName,
                    StartTime = StartTime,
                    Duration = Duration,
                    SelectedDays = SelectedDays
                };

                // Zastąp 'userId' rzeczywistym identyfikatorem użytkownika
                string userId = FirebaseAuth.DefaultInstance.CurrentUser.Uid;

                await _databaseService.AddSection(userId, newSection);
                Sections.Add(newSection); // Dodanie sekcji do lokalnej listy

                // Wyczyszczenie pól po dodaniu sekcji
                SectionName = string.Empty;
                StartTime = string.Empty;
                Duration = string.Empty;
                SelectedDays = string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas dodawania sekcji: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
