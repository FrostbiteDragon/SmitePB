using System.ComponentModel;
using System.IO;
using System.Windows;


namespace SmitePB.Manager.Windows
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string Text { get; private set; } = "Test string";
        private Display _display;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Text = Directory.GetCurrentDirectory();
        }

        private void OpenScreen(object sender, RoutedEventArgs e)
        {
            if (_display is not null)
            {
                _display.Focus();
                return;
            }

            var window = new Display();
            window.Show();
            _display = window;
        }

        private void SelectGod(object sender, RoutedEventArgs e)
        {
            _display?.SetGod(1, "Agni");
            Text = "selected Agni";

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
        }
    }
}
