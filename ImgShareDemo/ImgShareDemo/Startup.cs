using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImgShareDemo.Startup))]
namespace ImgShareDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
