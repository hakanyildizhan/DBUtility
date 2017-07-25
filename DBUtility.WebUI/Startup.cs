using Microsoft.Owin;
using Owin;

namespace DBUtility.WebUI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}