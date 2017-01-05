using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyVoiceMVC.Startup))]
namespace MyVoiceMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
