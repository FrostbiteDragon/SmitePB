using SmitePB.Manager.Windows;
using System;
using System.Collections.Generic;
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

namespace SmitePB.Manager.Controls
{
    /// <summary>
    /// Interaction logic for PickItem.xaml
    /// </summary>
    public partial class PickItem : UserControl
    {
        public string ImageSource { get; set; }

        //public Brush FillColour 
        //{ 
        //    get 
        //    {
        //        return (FindName("background") as Rectangle).Fill;
        //    } 
        //    set
        //    {
        //        (FindName("background") as Rectangle).Fill = value;
        //    }
        //}
        public Brush FillColourBrush { get; private set; }
        private void SetFillColourBrush(string hex)
        {
            FillColourBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
        }

        public string FillColour
        {
            get { return (string)GetValue(SetFillColourProperty); }
            set 
            { 
                SetValue(SetFillColourProperty, value);
                SetFillColourBrush(value);
            }
        }

        public static readonly DependencyProperty SetFillColourProperty =
            DependencyProperty.Register(
                "FillColour",
                typeof(string),
                typeof(PickItem));

        public PickItem()
        {
            DataContext = this;
            InitializeComponent();
        }
    }
}
