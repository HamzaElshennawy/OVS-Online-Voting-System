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
    /// Interaction logic for AddCandidateWindow.xaml
    /// </summary>
    public partial class AddCandidateWindow : Window
    {
        IFirebaseClient client;
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "tqRlM0Y9NpkstKBvsPDfVXw6aR2bVeRMn7x6ubTj",
            BasePath = "https://onlinevotingsystem-ovs-default-rtdb.europe-west1.firebasedatabase.app/"
        };
        public AddCandidateWindow()
        {
            InitializeComponent();
        }

        private void AddBTN_Click(object sender, RoutedEventArgs e)
        {
            bool exists = false;
            FirebaseResponse response;
            Candidate newcandidate = new Candidate()
            {
                CID = CandidateIDTbox.Text,
                CName = CandidateNTbox.Text,
                CVotes = 0
            };
            client = new FireSharp.FirebaseClient(config);
            response = client.Get(@"CandidateList");
            Candidate candidate = response.ResultAs<Candidate>();
            var json = response.Body.ToString();
            Dictionary<string, Candidate> elist = JsonConvert.DeserializeObject<Dictionary<string, Candidate>>(json);
            foreach (KeyValuePair<string, Candidate> entry in elist)
            {
                Candidate currentCadidates = new Candidate()
                {
                    CName = entry.Value.CName,
                    CID = entry.Key,
                };
                if(newcandidate.CID == currentCadidates.CID || newcandidate.CName == currentCadidates.CName)
                {
                    MessageBox.Show("This Candidate already exists!!");
                    exists = true;
                    break;
                }
            }
            if(!exists)
            {
                response = client.Set("CandidateList/" + newcandidate.CID.ToString(), newcandidate);
                MessageBox.Show(response.Body, "Candidate Added.");
            }
        }

        private void ClearBTN_Click(object sender, RoutedEventArgs e)
        {
            CandidateIDTbox.Clear();
            CandidateNTbox.Clear();
        }
    }
}
