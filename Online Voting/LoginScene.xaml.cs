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
using FirebaseAdmin.Auth;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace Online_Voting
{
    /// <summary>
    /// Interaction logic for LoginScene.xaml
    /// </summary>
    public partial class LoginScene : Window
    {
        //const auth = FirebaseAuth.GetAuth();//getAuth();
        IFirebaseClient client;
        FirebaseAuth auth;
        
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret= "tqRlM0Y9NpkstKBvsPDfVXw6aR2bVeRMn7x6ubTj",
            BasePath= "https://onlinevotingsystem-ovs-default-rtdb.europe-west1.firebasedatabase.app/"
        };
        public LoginScene()
        {
            InitializeComponent();
        }

        private void LoginWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseBTN_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public void CheckForUser()
        {
            User user = new User()
            {
                UName = "Hamza",
                UEmail = "elshennawyhamza@gmail.com",
                UID = "1710874",
                UPassword = "Hamzamohammed159"
            };
            var temp = EmailTBox.Text;
            client = new FireSharp.FirebaseClient(config);
            MessageBox.Show(user.UEmail);
            var setter = client.Set("/UserList/" + user.UEmail, user);
            var respond = client.Get("UserList/" + temp);
            if (respond.Body != "null")
            {
                MessageBox.Show("User exists");
            }
            if (respond.Body == "null")
            {
                MessageBox.Show("Not exist");
            }

        }

        private void LoginBTN_Click(object sender, RoutedEventArgs e)
        {
            CheckForUser();
        }
    }
}
