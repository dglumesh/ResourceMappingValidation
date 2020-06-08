using Microsoft.WindowsAzure.Storage.Table;

namespace GroupManager.Models
{
    public class MigrationCustomer : TableEntity
    {
       
        public MigrationCustomer()
        {

        }

        public string CustomerName { get; set; }
        public string EmailId { get; set; }
        public string SalesPersonName { get; set; }
        public string SalesPersonEmail { get; set; }
        public string Status { get; set; } = "Access Granted";

    }
}