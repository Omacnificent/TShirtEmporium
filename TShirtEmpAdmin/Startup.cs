using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TShirtEmpAdmin.Startup))]
namespace TShirtEmpAdmin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
