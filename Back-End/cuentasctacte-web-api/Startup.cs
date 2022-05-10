using Microsoft.Owin;
using Owin;


[assembly: OwinStartup(typeof(cuentasctacte_web_api.Startup))]
namespace cuentasctacte_web_api
{


    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            ConfigureAuth(app);
        }
    }
}
