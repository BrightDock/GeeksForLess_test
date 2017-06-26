using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GeeksForLess_test.Startup))]
namespace GeeksForLess_test
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
