namespace CoreBaseProject.Application.ApplicationLogic.MasterSettings.Customer.CustomerLogic.Model
{
    public class CustomerViewModel
    {
        public CustomerCreateModel CreateModel { get; set; }
        public CustomerUpdateModel UpdateModel { get; set; }
        public CustomerGridModel GridModel { get; set; }
        public dynamic OptionsDataSources { get; set; } = new ExpandoObject();
    }

    public class CustomerCreateModel : IMapFrom<CoreBaseProject.Model.Models.MasterSettings.Customer>
    {
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Please, provide full name.")]
        [StringLength(50, MinimumLength = 2)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please, provide company.")]
        public int CompanyId { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }
        public string? Address { get; set; }
        public int StatusId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CoreBaseProject.Model.Models.MasterSettings.Customer, CustomerCreateModel>();
            profile.CreateMap<CustomerCreateModel, CoreBaseProject.Model.Models.MasterSettings.Customer>();
        }
    }

    public class CustomerUpdateModel : IMapFrom<CoreBaseProject.Model.Models.MasterSettings.Customer>
    {
        public int Id { get; set; }
        public string? EncryptedId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Please, provide full name.")]
        [StringLength(50, MinimumLength = 2)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please, provide company.")]
        public int CompanyId { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }
        public string? Address { get; set; }
        public int StatusId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CoreBaseProject.Model.Models.MasterSettings.Customer, CustomerUpdateModel>()
                .ForMember(d => d.EncryptedId, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())));
            profile.CreateMap<CustomerUpdateModel, CoreBaseProject.Model.Models.MasterSettings.Customer>();
        }
    }

    public class CustomerGridModel : IMapFrom<CoreBaseProject.Model.Models.MasterSettings.Customer>
    {
        public int Id { get; set; }
        public string? EncryptedId { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }
        public string? Address { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public bool HasUserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CoreBaseProject.Model.Models.MasterSettings.Customer, CustomerGridModel>()
                .ForMember(d => d.EncryptedId, s => s.MapFrom(m => EncryptionService.Encrypt(m.Id.ToString())))
                .ForMember(d => d.CompanyName, s => s.MapFrom(m => m.Company.Name))
                .ForMember(d => d.StatusName, s => s.MapFrom(m => m.Status.Name));
        }
    }

    public class CustomerImageUpdateModel
    {
        public int Id { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}