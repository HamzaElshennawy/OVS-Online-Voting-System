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
    /// Interaction logic for RegisterScene.xaml
    /// </summary>
    public partial class RegisterScene : Window
    {
        IFirebaseClient client;
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "tqRlM0Y9NpkstKBvsPDfVXw6aR2bVeRMn7x6ubTj",
            BasePath = "https://onlinevotingsystem-ovs-default-rtdb.europe-west1.firebasedatabase.app/"
        };
        public RegisterScene()
        {
            InitializeComponent();
        }

        private void RegisterWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void RegisterBTN_Click(object sender, RoutedEventArgs e)
        {
            register();
        }

        private void CloseBTN_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public void register()
        {
            if(checkForEmailIfExist() == false)
            {
                User tempUser = new User()
                {
                    UEmail = EmailTBox.Text,
                    UID = UserIDTbox.Text,
                    UName = UserNTbox.Text,
                    UPassword = PassTBox.Password.ToString()
                };
                client = new FireSharp.FirebaseClient(config);
                var setter = client.Set("UserList/" + tempUser.UID, tempUser);
                MessageBox.Show("Successfuly Registered.");
            }
            else { MessageBox.Show("This account already registered."); }
        }
        public bool checkForEmailIfExist()
        {
            FirebaseResponse response;
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
                if (EmailTBox.Text == u.UEmail)
                {
                    return true;  
                }
            }
            return false;
        }
    }
}
