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

namespace SmitePB.Manager.Windows
{
    public partial class Display : Window, INotifyPropertyChanged
    {
        public string Slot1 { get; private set; }
        public string Slot2 { get; private set; }
        public string Team1Colour { get; private set; }
        public string Team2Colour { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Display()
        {
            InitializeComponent();
            DataContext = this;
            Slot1 = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Assets", $"Pick.png");
            SetTeam(0, new("", "#558de2"));
            SetTeam(1, new("", "#55e269"));
        }

        public void SetGod(int slot, string god)
        {
            Slot1 = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Assets", $"{god}.png");
            PropertyChanged?.Invoke(this, new(nameof(Slot1)));
        }

        public void SetTeam(int slot, Team team)
        {
            switch (slot)
            {
                case 0:
                    Team1Colour = team.colour;
                    PropertyChanged?.Invoke(this, new(nameof(Team1Colour)));
                    break;

                case 1:
                    Team2Colour = team.colour;
                    PropertyChanged?.Invoke(this, new(nameof(Team2Colour)));
                    break;
            }
           
        }
    }
}
