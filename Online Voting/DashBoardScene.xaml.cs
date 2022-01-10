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
using System.Numerics;



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
        List<string> P_List = new List<string>();//Prisedint list
        List<string> VP_List = new List<string>();//Vice list
        List<string> HOA_List = new List<string>();//Head of activeties list
        List<string> PR_List = new List<string>();//pr list
        List<string> HOC_List = new List<string>();//head of clubs list
        List<string> SA_List = new List<string>();//head of social activeties
        public DashBoardScene()
        {
            InitializeComponent();
            Update_P_CBox();
            Update_Candidate_Boxs();
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
            GoToHomepn();
        }
        public void GoToHomepn()
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
                Leaderpn.Visibility = Visibility.Collapsed;
                Votepn.Visibility = Visibility.Visible;
                Homepn.Visibility = Visibility.Collapsed;
                AddCadidateBTN.Visibility = Visibility.Collapsed;
                if (CheckForUserIfVoted())
                {
                    SubmiteVoteBTN.Visibility = Visibility.Collapsed;
                }
                else
                {
                    SubmiteVoteBTN.Visibility = Visibility.Visible;
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
                    if (currentUser.HadVoted==true)
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
        public void Update_P_CBox()
        {
            List<string> CandidateNamesList = new List<string>();
            Candidate currentCandidate;
            try
            {
                FirebaseResponse response;
                client = new FireSharp.FirebaseClient(config);
                response = client.Get(@"CandidateList");
                Candidate candidate = response.ResultAs<Candidate>();
                var json = response.Body.ToString();
                Dictionary<string, Candidate> elist = JsonConvert.DeserializeObject<Dictionary<string, Candidate>>(json);
                foreach (KeyValuePair<string, Candidate> entry in elist)
                {
                    currentCandidate = new Candidate()
                    {
                        CID = entry.Key,
                        CName = entry.Value.CName,
                        CRule = entry.Value.CRule,
                        CVotes = entry.Value.CVotes
                    };
                    if (currentCandidate.CRule== "President")
                    {
                        CandidateNamesList.Add(currentCandidate.CName);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Check your internet connection.....");
            }

            P_CBox.ItemsSource= CandidateNamesList;
        }

        public void Update_Candidate_Boxs()
        {
            
            Candidate currentCandidate;
            try
            {
                FirebaseResponse response;
                client = new FireSharp.FirebaseClient(config);
                response = client.Get(@"CandidateList");
                Candidate candidate = response.ResultAs<Candidate>();
                var json = response.Body.ToString();
                Dictionary<string, Candidate> elist = JsonConvert.DeserializeObject<Dictionary<string, Candidate>>(json);
                foreach (KeyValuePair<string, Candidate> entry in elist)
                {
                    currentCandidate = new Candidate()
                    {
                        CID = entry.Key,
                        CName = entry.Value.CName,
                        CRule = entry.Value.CRule,
                        CVotes = entry.Value.CVotes
                    };
                    if (currentCandidate.CRule == "President")
                    {
                        P_List.Add(currentCandidate.CName);
                    }
                    else if (currentCandidate.CRule == "Social activities")
                    {
                        SA_List.Add(currentCandidate.CName);
                    }
                    else if (currentCandidate.CRule == "PR")
                    {
                        PR_List.Add(currentCandidate.CName);
                    }
                    else if (currentCandidate.CRule == "Vice Presidint")
                    {
                        VP_List.Add(currentCandidate.CName);
                    }
                    else if (currentCandidate.CRule == "Head of clubs")
                    {
                        HOC_List.Add(currentCandidate.CName);
                    }
                    else if (currentCandidate.CRule == "Head of Academics")
                    {
                        HOA_List.Add(currentCandidate.CName);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Check your internet connection.....");
            }

            P_CBox.ItemsSource = P_List;
            VP_CBox.ItemsSource = VP_List;
            SAR_CBox.ItemsSource = SA_List;
            PR_CBox.ItemsSource = PR_List;
            HOC_CBox.ItemsSource = HOC_List;
            HOA_CBox.ItemsSource = HOA_List;
        }
        private void P_CBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update_P_CBox();
        }

        private void SubmiteVoteBTN_Click(object sender, RoutedEventArgs e)
        {
            FirebaseResponse response;
            client = new FireSharp.FirebaseClient(config);
            //MessageBox.Show(P_CBox.SelectedValue.ToString());
            if (P_CBox.SelectedIndex == -1 || VP_CBox.SelectedIndex == -1 || SAR_CBox.SelectedIndex == -1 || PR_CBox.SelectedIndex == -1 || HOC_CBox.SelectedIndex == -1 || HOA_CBox.SelectedIndex == -1)
            {
                MessageBox.Show("You did not select all the roles");
            }
            else
            {
                MessageBox.Show(currentUser.UName);
                Candidate currentCandidate;
                response = client.Get(@"CandidateList");
                Candidate candidate = response.ResultAs<Candidate>();
                var json = response.Body.ToString();
                Dictionary<string, Candidate> elist = JsonConvert.DeserializeObject<Dictionary<string, Candidate>>(json);
                foreach (KeyValuePair<string, Candidate> entry in elist)
                {
                    currentCandidate = new Candidate()
                    {
                        CID = entry.Key,
                        CName = entry.Value.CName,
                        CRule = entry.Value.CRule,
                        CVotes = entry.Value.CVotes
                    };
                    if (currentCandidate.CName == P_CBox.SelectedItem.ToString())
                    {
                        currentCandidate.CVotes += 1;
                        response = client.Update("CandidateList/" + currentCandidate.CID, currentCandidate);
                    }
                    if (currentCandidate.CName == VP_CBox.SelectedItem.ToString())
                    {
                        currentCandidate.CVotes += 1;
                        response = client.Update("CandidateList/" + currentCandidate.CID, currentCandidate);
                    }
                    if (currentCandidate.CName == SAR_CBox.SelectedItem.ToString())
                    {
                        currentCandidate.CVotes += 1;
                        response = client.Update("CandidateList/" + currentCandidate.CID, currentCandidate);
                    }
                    if (currentCandidate.CName == PR_CBox.SelectedItem.ToString())
                    {
                        currentCandidate.CVotes += 1;
                        response = client.Update("CandidateList/" + currentCandidate.CID, currentCandidate);
                    }
                    if (currentCandidate.CName == HOC_CBox.SelectedItem.ToString())
                    {
                        currentCandidate.CVotes += 1;
                        response = client.Update("CandidateList/" + currentCandidate.CID, currentCandidate);
                    }
                    if (currentCandidate.CName == HOA_CBox.SelectedItem.ToString())
                    {
                        currentCandidate.CVotes += 1;
                        response = client.Update("CandidateList/" + currentCandidate.CID, currentCandidate);
                    }
                }
                currentUser.HadVoted = true;
                response = client.Update("UserList/" + currentUser.UID, currentUser);
                response = client.Update("CurrentUser/" + currentUser.UID, currentUser);
                GoToHomepn();
            }
        }
    }
}