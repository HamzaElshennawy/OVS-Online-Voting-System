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
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using FireSharp.EventStreaming;
using FirebaseAdmin.Messaging;
using Newtonsoft.Json;

namespace Online_Voting
{
    /// <summary>
    /// Interaction logic for LoginScene.xaml
    /// </summary>
    public partial class LoginScene : Window
    {
        
        IFirebaseClient client;
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret= "tqRlM0Y9NpkstKBvsPDfVXw6aR2bVeRMn7x6ubTj",
            BasePath= "https://onlinevotingsystem-ovs-default-rtdb.europe-west1.firebasedatabase.app/"
        };
        public LoginScene()
        {
            InitializeComponent();
            Incorrectlbl.Visibility = Visibility.Collapsed;
        }

        private void LoginWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseBTN_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public bool CheckForUser()
        {
            try
            {
                FirebaseResponse response;
                var temp = EmailTBox.Text;
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
                        HadVoted = entry.Value.HadVoted,
                        isAdmin = entry.Value.isAdmin
                    };
                    if (EmailTBox.Text == u.UEmail)
                    {
                        if (PassTBox.Password.ToString() == u.UPassword)
                        {
                            DashBoardScene db = new DashBoardScene();
                            response = client.Delete("CurrentUser");
                            response = client.Set("CurrentUser/" + u.UID, u);
                            db.CurrentUserlbl.Content = u.UName.ToString();
                            db.Show();
                            this.Close();
                            
                            return true;
                        }
                    }
                }
                Incorrectlbl.Visibility = Visibility.Visible;
            }
            catch
            {
                MessageBox.Show("Check your internet connection.");
            }
            
            
            return false;
        }

        private void LoginBTN_Click(object sender, RoutedEventArgs e)
        {
            CheckForUser();
        }
        
    }
}
