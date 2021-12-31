using System.Linq;
using System.Windows;
using System.ComponentModel;
using SmitePB.Domain;
using System.Windows.Media;
using System;
using System.Windows.Controls;

namespace SmitePB.Manager.Windows
{
    public partial class Display : Window, INotifyPropertyChanged
    {
        public Team[] Teams { get; }

        public God[] SelectedGods { get; } = new God[10];
        public GodStats[] GodStats { get; } = new GodStats[10];
        public bool[] LockedIn { get; } = new bool[10];
        public string[] Bans { get; private set; } = new string[10];
        public int[] Wins { get; } = new int[2] { 0, 1 };
        public string[] PlayerNames { get; } = new string[10];


        public Visibility[] PickVisibilities { get; private set; } = new Visibility[10];

        public Team Team0 { get; private set; }
        public Team Team1 { get; private set; }

        public Brush Team0Colour { get; private set; }

        public string[] GetGodNames() => gods.Select(x => x.Name).ToArray();

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly God[] gods;
        private readonly MediaPlayer mediaPlayer = new();

        private Team GetTeambyName(string name) => Teams.FirstOrDefault(x => x.DisplayName == name);
        private God GetGodbyName(string name) => gods.FirstOrDefault(x => x.Name == name);


        public Display()
        {
            //temp
            PlayerNames = PlayerNames.Select(x => "PLAYER").ToArray();

            Teams = TeamService.GetTeams().ToArray();
            gods = GodService.GetGods();
            var none = GetGodbyName("NONE");
            SelectedGods = SelectedGods.Select(x => none).ToArray();

            Bans = Bans.Select(x => none.Ban).ToArray();
            PickVisibilities = PickVisibilities.Select(x => Visibility.Hidden).ToArray();

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
            SelectedGods[slot] = GetGodbyName(godName);
            PickVisibilities[slot] = Visibility.Visible;
            PropertyChanged?.Invoke(this, new(nameof(PickVisibilities)));
            PropertyChanged?.Invoke(this, new(nameof(SelectedGods)));
        }

        public async void LockIn(int slot, bool state)
        {
            if (state)
            {

                GodStats[slot] = await GodService.GetStatsForGod(SelectedGods[slot].Name);

                PickVisibilities[slot] = Visibility.Hidden;
                mediaPlayer.Open(new(SelectedGods[slot].LockInSound));
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

            Bans[slot] = god.Ban;
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
    }
}
