using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupManager.Models
{
    public class InviteUser
    {

        public string invitedUserEmailAddress { get; set; }
        public string invitedUser { get; set; }


    }
    public class ids
    {

        public string id { get; set; }

    }
    public class InviteUserResponse
        {
            public List<InviteUser> value { get; set; }
        }
    
}