using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace ModernCalculator.UserControls
{
    /// <summary>
    /// Interaction logic for TotalButton.xaml
    /// </summary>
    public partial class TotalButton : UserControl, INotifyPropertyChanged
    {
        public TotalButton()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event RoutedEventHandler ButtonClick;
        public event PropertyChangedEventHandler? PropertyChanged;

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        private string myFontSize;
        public string MyFontSize
        {
            get { return myFontSize; }
            set
            {
                myFontSize = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick?.Invoke(this, e);
        }



    }
}
