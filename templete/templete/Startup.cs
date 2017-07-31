using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(templete.Startup))]
namespace templete
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
