using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YummyCures.Startup))]
namespace YummyCures
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
