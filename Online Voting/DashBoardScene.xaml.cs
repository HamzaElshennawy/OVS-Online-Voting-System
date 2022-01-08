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
using System.Windows.Shapes;
using Prism.Services.Dialogs;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using FireSharp.EventStreaming;
using FirebaseAdmin.Messaging;
using Newtonsoft.Json;

namespace Online_Voting
{
    /// <summary>
    /// Interaction logic for DashBoardScene.xaml
    /// </summary>
    public partial class DashBoardScene : Window
    {
        IFirebaseClient client;
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "tqRlM0Y9NpkstKBvsPDfVXw6aR2bVeRMn7x6ubTj",
            BasePath = "https://onlinevotingsystem-ovs-default-rtdb.europe-west1.firebasedatabase.app/"
        };
        User currentUser;
        public DashBoardScene()
        {
            InitializeComponent();
            Leaderpn.Visibility = Visibility.Collapsed;
            Votepn.Visibility = Visibility.Collapsed;
            Homepn.Visibility = Visibility.Visible;
        }
        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == true)
            {
                tt_home.Visibility = Visibility.Collapsed;
                tt_contacts.Visibility = Visibility.Collapsed;
                tt_messages.Visibility = Visibility.Collapsed;
                tt_signout.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_home.Visibility = Visibility.Visible;
                tt_contacts.Visibility = Visibility.Visible;
                tt_messages.Visibility = Visibility.Visible;
                tt_signout.Visibility = Visibility.Visible;
            }
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 1;
            leaderPhotoBTN.Opacity = 2;
            home_Leaderlbl.Opacity = 1;
            home_Votelbl.Opacity = 1;
        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 0.3;
            Homepn.Opacity = 0.9;
            home_Leaderlbl.Opacity = 0.3;
            home_Votelbl.Opacity = 0.3;
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void homeBTN_Click(object sender, RoutedEventArgs e)
        {
            
            Leaderpn.Visibility = Visibility.Collapsed;
            Votepn.Visibility = Visibility.Collapsed;
            Homepn.Visibility = Visibility.Visible;
        }

        
        private void VoteBTN_Click(object sender, RoutedEventArgs e)
        {
            if(CheckIfAdmin())
            {
                Leaderpn.Visibility = Visibility.Collapsed;
                Votepn.Visibility = Visibility.Visible;
                Homepn.Visibility = Visibility.Collapsed;
            }
            if(!CheckIfAdmin())
            {
                if(CheckForUserIfVoted())
                    MessageBox.Show("Sorry you had voted before", "Waring!!");
                else
                {
                    Leaderpn.Visibility = Visibility.Collapsed;
                    Votepn.Visibility = Visibility.Visible;
                    Homepn.Visibility = Visibility.Collapsed;
                    AddCadidateBTN.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void LeaderBTN_Click(object sender, RoutedEventArgs e)
        {
            
            Leaderpn.Visibility = Visibility.Visible;
            Votepn.Visibility = Visibility.Collapsed;
            Homepn.Visibility = Visibility.Collapsed;
        }

        private void signoutBTN_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }
        public bool CheckForUserIfVoted()
        {
            try
            {
                FirebaseResponse response;
                var temp = CurrentUserlbl.Content;
                client = new FireSharp.FirebaseClient(config);
                response = client.Get(@"CurrentUser");
                User user = response.ResultAs<User>();
                var json = response.Body.ToString();
                Dictionary<string, User> elist = JsonConvert.DeserializeObject<Dictionary<string, User>>(json);
                foreach (KeyValuePair<string, User> entry in elist)
                {
                    currentUser = new User()
                    {
                        UID = entry.Key,
                        UName = entry.Value.UName.ToString(),
                        UEmail = entry.Value.UEmail.ToString(),
                        UPassword = entry.Value.UPassword.ToString(),
                        HadVoted = entry.Value.HadVoted,
                        isAdmin = entry.Value.isAdmin
                    };
                    if (currentUser.HadVoted)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Check your internet connection.");
            }
            return false;
        }
        public void AddCadidate()
        {
            
        }
        public bool CheckIfAdmin()
        {
            
            try
            {
                FirebaseResponse response;  
                client = new FireSharp.FirebaseClient(config);
                response = client.Get(@"CurrentUser");
                User user = response.ResultAs<User>();
                var json = response.Body.ToString();
                Dictionary<string, User> elist = JsonConvert.DeserializeObject<Dictionary<string, User>>(json);
                foreach (KeyValuePair<string, User> entry in elist)
                {
                    currentUser = new User()
                    {
                        UID = entry.Key,
                        UName = entry.Value.UName.ToString(),
                        UEmail = entry.Value.UEmail.ToString(),
                        UPassword = entry.Value.UPassword.ToString(),
                        isAdmin=entry.Value.isAdmin,
                        HadVoted=entry.Value.HadVoted                        
                    };                    
                    if(currentUser.isAdmin == true)
                    {
                        return true;
                    }
                }  
            }
            catch
            {                
                MessageBox.Show("Check your internet connection.....");
            }           
            return false;
        }
        private void AddCadidateBTN_Click(object sender, RoutedEventArgs e)
        {
            AddCandidateWindow ADW = new AddCandidateWindow();
            ADW.Show();
        }
        private void DashBoard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
