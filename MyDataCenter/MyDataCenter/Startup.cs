using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyDataCenter.Startup))]
namespace MyDataCenter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
