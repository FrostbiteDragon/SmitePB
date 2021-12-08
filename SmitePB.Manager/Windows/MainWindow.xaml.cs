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

        public Team[] Teams { get; }
        public string[] TeamSource { get; }

        public string SelectedTeam0 { get; set; } = "";
        public string SelectedTeam1 { get; set; } = "";
        public string[] Gods { get; } = new string[] { "None", "Achilles", "Agni"};

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Display _display;
        private Team GetTeambyName(string name) => Teams.First(x => x.DisplayName == name);

        public MainWindow(Display owner, Team[] teams)
        {
            _display = owner;
            Teams = teams;
            TeamSource = teams.Select(x => x.DisplayName).ToArray();

            SelectedTeam0 = Teams[0].DisplayName;
            SelectedTeam1 = Teams[0].DisplayName;
            _display.SetTeam(0, GetTeambyName(SelectedTeam0));
            _display.SetTeam(1, GetTeambyName(SelectedTeam1));


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
        private void OnDropDownClosed(object sender, System.EventArgs e)
        {
            var comboBox = (ComboBox)sender;
            _display.SetGod(int.Parse((string)comboBox.Tag), (string)comboBox.SelectedItem);
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
            _display.SetTeam(teamId, GetTeambyName(teamId == 0 ? SelectedTeam0 : SelectedTeam1));
        }
    }
}
