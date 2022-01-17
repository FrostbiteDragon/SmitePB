using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media;
using System;
using System.Windows.Controls;
using SmitePB.Domain;
using SmitePB.Manager.Services;
using System.IO;

namespace SmitePB.Manager.Windows
{
    public partial class Display : Window, INotifyPropertyChanged
    {
        public double WindowScale { get; private set; } = 4.0 / 4;
        public Team[] Teams { get; }
        public God[] Picks { get; private set; } = new God[10];
        public GodStats[] GodStats { get; } = new GodStats[10];
        public bool[] LockedIn { get; private set; } = new bool[10];
        public God[] Bans { get; private set; } = new God[10];
        public int[] Wins { get; } = new int[2] { 0, 1 };
        public string[] PlayerNames { get; } = new string[10];
        public string BackgroundImage { get; }
        public string LeagueLogo { get; }

        public Visibility[] PickVisibilities { get; private set; } = new Visibility[10];

        public Team Team0 { get; private set; }
        public Team Team1 { get; private set; }

        public Brush Team0Colour { get; private set; }

        public string[] GetGodNames() => gods.Select(x => x.Name).ToArray();

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly God[] gods;
        private readonly MediaPlayer mediaPlayer = new();
        private const int defaultHeight = 1080;
        private const int defaultWidth = 1920;

        public Team GetTeambyName(string name) => Teams.FirstOrDefault(x => x.DisplayName == name);
        private God GetGodbyName(string name) => gods.FirstOrDefault(x => x.Name == name);

        private readonly ApiService _apiService;

        public Display(ApiService apiService)
        {
            Height = 1080 * WindowScale;
            Width = 1920 * WindowScale;

            _apiService = apiService;

            BackgroundImage = FileService.GetFile(FileService.AssetsFolder, "Background");
            LeagueLogo = FileService.GetFile(FileService.AssetsFolder, "Logo");

            Teams = FileService.GetTeams().ToArray();
            gods = FileService.GetGods();

            ClearPicksAndBans();
           
            DataContext = this;

            Show();

            //must be run before components are initialized
            var window = new MainWindow(this, Teams)
            {
                Owner = this
            };

            InitializeComponent();
        }

        public void SetGod(int slot, string godName)
        {
            var god = GetGodbyName(godName);
            if (god is null)
                return;

            LockIn(slot, false);
            Picks[slot] = GetGodbyName(godName);
            PickVisibilities[slot] = Visibility.Visible;
            PropertyChanged?.Invoke(this, new(nameof(PickVisibilities)));
            PropertyChanged?.Invoke(this, new(nameof(Picks)));
        }

        public async void LockIn(int slot, bool state)
        {
            if (state)
            {

                GodStats[slot] = await _apiService.GetStatsForGod(Picks[slot].Name);

                PickVisibilities[slot] = Visibility.Hidden;
                if (Picks[slot].LockInSound != null)
                    mediaPlayer.Open(new(Picks[slot].LockInSound));
                mediaPlayer.Volume = 0.25f;
                mediaPlayer.Play();
            }
            else
            {
                GodStats[slot] = null;
                PickVisibilities[slot] = Visibility.Visible;
            }

            LockedIn[slot] = state;
            PropertyChanged?.Invoke(this, new(nameof(PickVisibilities)));
            PropertyChanged?.Invoke(this, new(nameof(LockedIn)));
            PropertyChanged?.Invoke(this, new(nameof(GodStats)));
        }

        public void SetBan(int slot, string godName)
        {
            var god = GetGodbyName(godName);
            if (god is null)
                return;

            Bans[slot] = god;
            PropertyChanged?.Invoke(this, new(nameof(Bans)));
        }

        public void SetTeam(int slot, string teamName)
        {
            var team = GetTeambyName(teamName);

            switch (slot)
            {
                case 0:
                    Team0 = team;
                    PropertyChanged?.Invoke(this, new(nameof(Team0)));

                    Team0Colour = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2596be"));
                    break;

                case 1:
                    Team1 = team;
                    PropertyChanged?.Invoke(this, new(nameof(Team1)));
                    break;
            }
        }

        public void SetWins(int slot, int wins)
        {
            Wins[slot] = wins;
            PropertyChanged?.Invoke(this, new(nameof(Wins)));
        }

        public void SetPlayerName(int slot, string name)
        {
            PlayerNames[slot] = name;
            PropertyChanged?.Invoke(this, new(nameof(PlayerNames)));
        }

        private void OnMediaEnded(object sender, RoutedEventArgs e)
        {
            var media = sender as MediaElement;
            media.Position = TimeSpan.Zero;
        }

        public async void SaveResult(bool team0Won)
        {
            var gameResult = new GameResult(
                team0Won,
                Team0.DisplayName.ToLower(),
                Team1.DisplayName.ToLower(),
                Picks.Select(x => x.Name).ToArray(),
                Bans.Select(x => x.Name).ToArray()
            );

            await _apiService.SaveGameResult(gameResult);
        }

        public void ClearPicksAndBans()
        {
            var none = GetGodbyName("NONE");

            Picks = Picks.Select(x => none).ToArray();
            Bans = Bans.Select(x => none).ToArray();
            LockedIn = new bool[10];
            PickVisibilities = PickVisibilities.Select(x => Visibility.Hidden).ToArray();

            PropertyChanged?.Invoke(this, new(nameof(Picks)));
            PropertyChanged?.Invoke(this, new(nameof(Bans)));
            PropertyChanged?.Invoke(this, new(nameof(LockedIn)));
            PropertyChanged?.Invoke(this, new(nameof(PickVisibilities)));
        }

        public void SetWindowScale(double scale)
        {
            Height = defaultHeight * scale;
            Width = defaultWidth * scale;
            WindowScale = scale;
            PropertyChanged?.Invoke(this, new(nameof(WindowScale)));
        }

        public void SnapToTopLeft()
        {
            Left = 0;
            Top = 0;
        }

        private void WindowMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
