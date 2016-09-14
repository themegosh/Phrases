using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Phrases.Startup))]
namespace Phrases
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
