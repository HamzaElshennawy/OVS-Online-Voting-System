﻿using System;
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
        public DashBoardScene()
        {
            InitializeComponent();
            Leaderpn.Visibility = Visibility.Collapsed;
            Votepn.Visibility = Visibility.Collapsed;
            Homepn.Visibility = Visibility.Visible;
            if(!CheckIfAdmin())
            {
                AddCadidateBTN.Visibility = Visibility.Hidden;
            }
            else
            {
                AddCadidateBTN.Visibility = Visibility.Visible;
            }
            //MessageBox.Show(CurrentUserlbl.Content.ToString());
            
        }
        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            // Set tooltip visibility

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
            if(CheckForUserIfVoted())
            {
                MessageBox.Show("Sorry you had voted before","Waring!!");
            }
            else
            {
                Leaderpn.Visibility = Visibility.Collapsed;
                Votepn.Visibility = Visibility.Visible;
                Homepn.Visibility = Visibility.Collapsed;
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
                response = client.Get(@"UserList");
                User user = response.ResultAs<User>();
                var json = response.Body.ToString();
                Dictionary<string, User> elist = JsonConvert.DeserializeObject<Dictionary<string, User>>(json);
                foreach (KeyValuePair<string, User> entry in elist)
                {
                    User u = new User()
                    {
                        UID = entry.Key,
                        UName = entry.Value.UName.ToString(),
                        UEmail = entry.Value.UEmail.ToString(),
                        UPassword = entry.Value.UPassword.ToString(),
                    };
                    if (temp == u.UName)
                    {
                        if (u.HadVoted==true)
                        {
                            return true;
                        }
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
            FirebaseResponse response2;
            Candidate candidate = new Candidate()
            {
                CID = "1234",
                CName = "Ahmed",
                CVotes = 0
            };
            client = new FireSharp.FirebaseClient(config);
            response2 = client.Set("CandidateList/" + candidate.CID.ToString(), candidate);
            MessageBox.Show(response2.Body);
        }
        public bool CheckIfAdmin()
        {
            User u = new User();
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
                    u = new User()
                    {
                        UID = entry.Key,
                        UName = entry.Value.UName.ToString(),
                        UEmail = entry.Value.UEmail.ToString(),
                        UPassword = entry.Value.UPassword.ToString(),
                        isAdmin=entry.Value.isAdmin,
                        HadVoted=entry.Value.HadVoted
                        
                    };
                    
                    if(u.isAdmin == true)
                    {
                        
                        return true;
                    }
                }  
            }
            catch
            {
                MessageBox.Show(u.UID, "UserID");
                MessageBox.Show("Check your internet connection.....");
            }
            
            return false;
        }

        private void AddCadidateBTN_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
