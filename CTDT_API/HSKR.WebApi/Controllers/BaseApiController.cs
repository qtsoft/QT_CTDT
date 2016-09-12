using System.Web.Http;
using NLog;

namespace CTDT.WebApi.Controllers
{
    //[Authorize]
   
    public class BaseApiController:ApiController
    {
        public static Logger logger;
        public BaseApiController()
        {
            logger = LogManager.GetLogger(GetType().FullName);
        }
    }
}