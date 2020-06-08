using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupManager.Models
{
    public class Subscription
    {
        public string id { get; set; }
        //public string tenantId { get; set; }
        //public List<string> managedByTenants { get; set; } 
        public string subscriptionId { get; set; }


       //public virtual ICollection<GroupResponse> Groups { get; set; }
       public List<tenantIds> managedByTenants { get; set; }


    }
    public class tenantIds
    {

        public string tenantId { get; set; }

    }



    public class SubscriptionResponse
    {
        public List<Subscription> value { get; set; }
        //public List<Subscription.tenantIds> managedByTenants { get; set; }


    }
}