using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    public class CharacterSkill : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}