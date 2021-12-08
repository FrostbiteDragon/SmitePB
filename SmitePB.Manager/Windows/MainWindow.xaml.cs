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

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Display _display;

        public MainWindow(Display owner, Team[] teams)
        {
            _display = owner;
            TeamSource = teams.Select(x => x.DisplayName).ToArray();


            GodNames = _display.Gods.Select(x => x.name).ToArray();
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
            cb.IsDropDownOpen = true;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox)sender;
            cb.IsDropDownOpen = true;
        }
        private void OnPickDropDownClosed(object sender, System.EventArgs e)
        {
            var comboBox = (ComboBox)sender;
            _display.SetGod(int.Parse((string)comboBox.Tag), (string)comboBox.SelectedItem);
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

            PropertyChanged?.Invoke(this, new(nameof(SelectedTeam0)));
            PropertyChanged?.Invoke(this, new(nameof(SelectedTeam1)));
        }

        private void OnTeamSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            var teamId = int.Parse((string)comboBox.Tag);
            _display.SetTeam(teamId, teamId == 0 ? SelectedTeam0 : SelectedTeam1);
        }
    }
}
