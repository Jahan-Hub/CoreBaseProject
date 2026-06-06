using CoreBaseProject.Model.Models.MasterSettings.AccessControl;
using CoreBaseProject.Model.Models.MasterSettings.Enum;
using CoreBaseProject.Model.Models.MasterSettings.ReportAccessControl;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreBaseProject.Model.Models.MasterSettings
{
    public class User : IdentityUser
    {
        public User()
        {
            ReportUserAccesses = new HashSet<ReportUserAccess>();
            UserAccessMappings = new HashSet<UserAccessMapping>();
        }

        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Please, provide full name.")]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string FullName { get; set; }

        public int? CustomerId { get; set; }
        public bool ForcePasswordChanged { get; set; }
        public int ApplicationUserTypeId { get; set; }

        public Customer? Customer { get; set; }
        public EnumTypeCollection ApplicationUserType { get; set; }
        public ICollection<ReportUserAccess> ReportUserAccesses { get; set; }
        public ICollection<UserAccessMapping> UserAccessMappings { get; set; }
    }
}