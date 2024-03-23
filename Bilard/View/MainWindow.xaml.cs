using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace View
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void actionApplyButtonClicked(object sender, RoutedEventArgs e)
        {
            int val = int.Parse(value.Text);
            SecondWindow secondWindow = new SecondWindow(val);
            this.Close();
            secondWindow.Show();

        }

        private void actionAdd(object sender, RoutedEventArgs e)
        {
            int val = int.Parse(value.Text);
            if(val > 14)
            {
                MessageBox.Show("Nie może być więcej kul!");
            }
            else
            {
                val++;
                value.Text = val.ToString();
            }
        }


        private void actionSubtract(object sender, RoutedEventArgs e)
        {
            int val = int.Parse(value.Text);
            if(val < 2)
            {
                MessageBox.Show("Nie może być mniej kul!");
            }
            else
            {
                val--;
                value.Text = val.ToString();
            }
        }
    }
}