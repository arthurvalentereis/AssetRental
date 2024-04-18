using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UAParser;

namespace AssetRental.API.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        // returns the current authenticated account (null if not logged in)
        public IdentityUser userHttpContext => (IdentityUser)HttpContext.Items["User"];

        public BaseController()
        {
        }

        protected string IpAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString();
        }

        protected ClientInfo DeviceType()
        {
            var userAgent = HttpContext?.Request?.Headers["User-Agent"];
            var uaParser = Parser.GetDefault();
            ClientInfo client;

            if (uaParser == null && string.IsNullOrEmpty(userAgent)) return null;
            
            client = uaParser.Parse(userAgent);
            return client;
        }

    }
}



