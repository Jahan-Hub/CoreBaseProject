using System.ComponentModel.DataAnnotations.Schema;

namespace CoreBaseProject.Model.Models.MasterSettings
{
    [Table("UserLoginHistories", Schema = "MasterSettings")]
    public class UserLoginHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string LoginIp { get; set; }
        public DateTime? LoginDateTime { get; set; }
        public DateTime? LogoutDateTime { get; set; }
    }
}