using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IdentifyProject.Startup))]
namespace IdentifyProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
