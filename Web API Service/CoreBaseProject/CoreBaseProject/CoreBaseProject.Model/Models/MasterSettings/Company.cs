using CoreBaseProject.Model.Models.MasterSettings.Enum;
using CoreBaseProject.Shared.Mappings;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreBaseProject.Model.Models.MasterSettings
{
    [Table("Companies", Schema = "MasterSettings")]
    public class Company : IAuditableEntity, IDelatableEntity
    {
        public Company()
        {
            Customers = new List<Customer>();
        }

        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Please, provide name.")]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string Name { get; set; }

        public string? Logo { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Please, provide country.")]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string Country { get; set; }

        public string? Website { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public int StatusId { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }
     
        public EnumTypeCollection Status { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }
}