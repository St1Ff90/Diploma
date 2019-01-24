using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyProject.Startup))]
namespace MyProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
