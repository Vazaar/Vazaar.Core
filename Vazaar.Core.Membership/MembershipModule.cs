using Autofac;
using Vazaar.Core.Membership.DbContexts;

namespace Vazaar.Core.Membership
{
    public class MembershipModule : Module
    {
        private readonly string connectionString;
        private readonly string migrationAssemblyName;

        public MembershipModule(string connectionString, string migrationAssemblyName)
        {
            this.connectionString = connectionString;
            this.migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().AsSelf()
                .WithParameter("connectionString", connectionString)
                .WithParameter("migrationAssemblyName", migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationDbContext>().As<IApplicationDbContext>()
                .WithParameter("connectionString", connectionString)
                .WithParameter("migrationAssemblyName", migrationAssemblyName)
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
