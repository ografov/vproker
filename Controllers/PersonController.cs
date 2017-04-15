using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using vproker.Models;

namespace vproker.Controllers
{
    [Authorize]
    public class PersonController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            var persons = new PersonDatabase();
            return View(persons);
        }
    }
}