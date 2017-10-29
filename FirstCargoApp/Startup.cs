using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FirstCargoApp.Startup))]
namespace FirstCargoApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
