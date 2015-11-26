using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(guahao.Startup))]
namespace guahao
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
