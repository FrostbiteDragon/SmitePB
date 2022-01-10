using SmitePB.Domain;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SmitePB.Manager.Windows
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string SearchBoxText { get; set; } = "";
        public string[] TeamSource { get; }
        public string SelectedTeam0 { get; set; } = "";
        public string SelectedTeam1 { get; set; } = "";
        public string[] GodNames { get; }


        public bool[] LockedIn { get; } = new bool[10];
        public event PropertyChangedEventHandler PropertyChanged;

        private bool saveResults;
        public string[] Players { get; set; } = new string[10];
        private readonly Display _display;

        public MainWindow(Display owner, Team[] teams)
        {
            _display = owner;
            TeamSource = teams.Select(x => x.DisplayName).ToArray();


            GodNames = _display.GetGodNames();
            SelectedTeam0 = _display.Teams[0].DisplayName;
            SelectedTeam1 = _display.Teams[0].DisplayName;
            _display.SetTeam(0, SelectedTeam0);
            _display.SetTeam(1, SelectedTeam1);


            DataContext = this;
            InitializeComponent();

            Show();
        }


        private void CloseScreen(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var cb = (ComboBox)sender;
            //cb.IsDropDownOpen = true;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox)sender;
            cb.IsDropDownOpen = true;
        }
        private void OnPickDropDownClosed(object sender, System.EventArgs e)
        {
            var comboBox = (ComboBox)sender;
            var slot = int.Parse((string)comboBox.Tag);
            _display.SetGod(slot, (string)comboBox.SelectedItem);

            LockedIn[slot] = false;
            PropertyChanged?.Invoke(this, new(nameof(LockedIn)));

        }

        private void OnBanDropDownClosed(object sender, System.EventArgs e)
        {
            var comboBox = (ComboBox)sender;
            _display.SetBan(int.Parse((string)comboBox.Tag), (string)comboBox.SelectedItem);
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void SwapTeamSides(object sender, RoutedEventArgs e)
        {
            var selectedTeam0 = SelectedTeam0;

            SelectedTeam0 = SelectedTeam1;
            SelectedTeam1 = selectedTeam0;

            var team0Players = Players.Take(5);

            Players = Players.TakeLast(5).Concat(team0Players).ToArray();

            PropertyChanged?.Invoke(this, new(nameof(Players)));
            PropertyChanged?.Invoke(this, new(nameof(SelectedTeam0)));
            PropertyChanged?.Invoke(this, new(nameof(SelectedTeam1)));
        }

        private void OnTeamSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            var teamId = int.Parse((string)comboBox.Tag);
            _display.SetTeam(teamId, teamId == 0 ? SelectedTeam0 : SelectedTeam1);
        }

        private void OnLockedIn(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var slot = int.Parse(button.Tag as string);
            if (LockedIn[slot])
            {
                LockedIn[slot] = false;
                _display.LockIn(slot, false);
                PropertyChanged?.Invoke(this, new(nameof(LockedIn)));
            }
            else
            {
                LockedIn[slot] = true;
                _display.LockIn(slot, true);
                PropertyChanged?.Invoke(this, new(nameof(LockedIn)));
            }
        }

        private void OnWinCountChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (int.TryParse(textBox.Text, out int wins))
                _display.SetWins(int.Parse(textBox.Tag as string), wins);
            else
                textBox.Text = "";
        }

        private void OnPlayerNameChnaged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var slot = int.Parse(textBox.Tag as string);

            Players[slot] = textBox.Text;
            _display.SetPlayerName(slot, textBox.Text);
        }

        private void OnTeamWon(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            bool team0Won = (string)button.Tag == "0";

            var team0WinDisplay = FindName("Team0WinDisplay") as TextBox;
            var team1WinDisplay = FindName("Team1WinDisplay") as TextBox;

            if (team0Won)
                team0WinDisplay.Text = (int.Parse(team0WinDisplay.Text) + 1).ToString();
            else
                team1WinDisplay.Text = (int.Parse(team0WinDisplay.Text) + 1).ToString();

            if (saveResults)
                _display.SaveResult(team0Won);

            _display.ClearPicksAndBans();
        }

        private void OnSaveGameResultChecked(object sender, RoutedEventArgs e) => saveResults = true;
        private void OnSaveGameResultUnchecked(object sender, RoutedEventArgs e) => saveResults = false;
    }
}
