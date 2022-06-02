using System.Web.Http;

namespace cuentasctacte_web_api.Controllers
{
    [Authorize(Roles = "Cajero")]
    public class PagosController : ApiController
    {



    }
}
