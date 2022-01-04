using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Voting
{
    internal class User
    {
        public string UName { get; set; }
        public string UEmail { get; set; }
        public string UPassword { get; set; }
        public string UID { get; set; }
        public bool HadVoted = false;
    }
}
