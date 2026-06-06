using CoreBaseProject.Application.ApplicationLogic.DatabaseSeedConfiguration.command;
using CoreBaseProject.Application.ApplicationLogic.MasterSettings.AccessControl.ActionLogic.Command;
using CoreBaseProject.Application.ApplicationLogic.MasterSettings.AccessControl.FeatureActionMapping.Command;
using CoreBaseProject.Application.ApplicationLogic.MasterSettings.AccessControl.UserAccessMapping.Command;
using CoreBaseProject.Application.ApplicationLogic.MasterSettings.CompanyLogic.Command;
using CoreBaseProject.Application.ApplicationLogic.MasterSettings.EnumLogic.Command;
using CoreBaseProject.Application.ApplicationLogic.MasterSettings.FeaturesLogic.Command;
using CoreBaseProject.Application.ApplicationLogic.MasterSettings.ReportAccessControl.ReportRegistryLogic.Command;
using CoreBaseProject.Application.ApplicationLogic.MasterSettings.UserLogic.Command;
using CoreBaseProject.Repository.Contracts.MasterSettings.AccressControl;
using CoreBaseProject.Repository.Contracts.MasterSettings.Enum;
using CoreBaseProject.Repository.Contracts.MasterSettings.ReportAccessControl;
using CoreBaseProject.Repository.Repository.MasterSettings;
using CoreBaseProject.Repository.Repository.MasterSettings.AccessControl;
using CoreBaseProject.Repository.Repository.MasterSettings.Enum;
using Microsoft.Extensions.DependencyInjection;
using SadaqaAccounting.Repository.Repository.MasterSettings.ReportAccessControl;
using System.Reflection;

namespace CoreBaseProject.Application.Configurations
{
    public static class ApplicationDependencyInjectionConfigurationService
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            // Add auto mapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<AutomapperMappingProfile>();
            }, Assembly.GetExecutingAssembly());

            // Add mediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            // Register IHttpContextAccessor 
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Register Image Service 
            //services.AddSingleton<IImageService, ImageService>();

            #region Master Setting
            services.AddScoped<IEnumTypeRepository, EnumTypeRepository>();
            services.AddScoped<IEnumTypeCollectionRepository, EnumTypeCollectionRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IActionRepository, ActionRepository>();
            services.AddScoped<IFeaturesRepository, FeaturesRepository>();
            services.AddScoped<IFeatureActionMappingRepository, FeatureActionMappingRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleActionMappingRepository, RoleActionMappingRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IUserAccessMappingRepository, UserAccessMappingRepository>();
            services.AddScoped<IUserLoginHistoryRepository, UserLoginHistoryRepository>();

            services.AddScoped<IReportRegistryRepository, ReportRegistryRepository>();
            services.AddScoped<IReportUserAccessRepository, ReportUserAccessRepository>();

            // Register other repositories as needed
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            #endregion

            #region Crypto Service
            services.AddSingleton<CryptoService>();
            #endregion

            #region Register Seed
            services.AddScoped<DatabaseSeederConfiguration>();
            services.AddScoped<CreateEnumSeedCommand>();
            services.AddScoped<CreateActionSeedCommand>();
            services.AddScoped<CreateFeatureSeedCommand>();
            services.AddScoped<CreateFeatureActionMappingSeedCommand>();
            services.AddScoped<CreateCompanySeedCommand>();
            services.AddScoped<CreateSuperAdminUserCommand>();
            services.AddScoped<CreateUserAccessMappingSeedCommand>();
            services.AddScoped<CreateReportRegistrySeedCommand>();
            #endregion

            // Notification Service - AddSignalR
            services.AddSignalR();

            return services;
        }
    }
}