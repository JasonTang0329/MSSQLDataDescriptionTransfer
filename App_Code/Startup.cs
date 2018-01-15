using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DBSchemaTrans.Startup))]
namespace DBSchemaTrans
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
