using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GeeksForLess_testApp.Startup))]
namespace GeeksForLess_testApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
