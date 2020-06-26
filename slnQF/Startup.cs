using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(slnQF.Startup))]
namespace slnQF
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
