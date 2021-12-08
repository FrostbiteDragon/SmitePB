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
        public Team[] Teams { get; }
        public God[] Gods { get; }

        public string[] Picks { get; private set; } = new string[10];
        public string[] Bans { get; private set; } = new string[10];

        public Team Team0 { get; private set; }
        public Team Team1 { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private Team GetTeambyName(string name) => Teams.FirstOrDefault(x => x.DisplayName == name);
        private God GetGodbyName(string name) => Gods.FirstOrDefault(x => x.name == name);


        public Display()
        {
            Teams = TeamService.GetTeams().ToArray();
            Gods = GodService.GetGods().ToArray();

            var none = GetGodbyName("None");
            Picks = Picks.Select(x => none.Pick).ToArray();
            Bans = Bans.Select(x => none.Ban).ToArray();


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

            Picks[slot] = god.Pick;
            PropertyChanged?.Invoke(this, new(nameof(Picks)));
        }

        public void SetBan(int slot, string godName)
        {
            var god = GetGodbyName(godName);

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
                    break;

                case 1:
                    Team1 = team;
                    PropertyChanged?.Invoke(this, new(nameof(Team1)));
                    break;
            }
           
        }
    }
}
