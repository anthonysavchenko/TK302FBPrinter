using Microsoft.AspNetCore.Mvc;

namespace TK302FBPrinter
{
    [Route("")]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        public string Index()
        {
            return "TK302-FB Printer Service is running...";
        }
    }
}
