using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using SmitePB.Domain;
using Path = System.IO.Path;

namespace SmitePB.Manager.Windows
{
    public partial class Display : Window, INotifyPropertyChanged
    {

        public Team[] Teams { get; private set; }

        public string[] Picks { get; private set; }
        public Team Team0 { get; private set; }
        public Team Team1 { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private string GetPickPath(string godName) => Path.Combine(Directory.GetCurrentDirectory(), "Assets", godName, "Pick.png");

        public Display()
        {
            Teams = TeamService.GetTeams().ToArray();

            Picks = new string[10];
            Picks = Picks.Select(x => GetPickPath("None")).ToArray();

            DataContext = this;

            Show();

            //must be run before components are initialized
            var window = new MainWindow(this, Teams);
            window.Owner = this;

            InitializeComponent();
        }

        public void SetGod(int slot, string god)
        {
            Picks[slot] = GetPickPath(god ?? "None");
            PropertyChanged?.Invoke(this, new(nameof(Picks)));
        }

        public void SetTeam(int slot, Team team)
        {
            switch (slot)
            {
                case 0:
                    Team0 = team;
                    PropertyChanged?.Invoke(this, new(nameof(Team0)));
                    break;

                case 1:
                    Team1 = team;
                    PropertyChanged?.Invoke(this, new(nameof(Team1)));
                    break;
            }
           
        }
    }
}
