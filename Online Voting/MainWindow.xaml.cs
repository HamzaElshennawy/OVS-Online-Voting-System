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


namespace Online_Voting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();           
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void CloseBTN_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LoginBTN_Click(object sender, RoutedEventArgs e)
        {
            LoginScene Loginscene = new LoginScene();
            Loginscene.Show();

            Main_Window.Close();
        }

        private void RegisterBTN_Click(object sender, RoutedEventArgs e)
        {
            RegisterScene registerScene = new RegisterScene();
            registerScene.Show();
            Main_Window.Close();
        }
    }
}
