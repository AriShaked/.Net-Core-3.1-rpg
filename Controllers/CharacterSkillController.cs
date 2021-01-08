using System.Threading.Tasks;
using dotnet_rpg.Dtos.CharacterSkill;
using dotnet_rpg.services.CharacterSkillService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace dotnet_rpg.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[Controller]")]
    public class CharacterSkillController : ControllerBase
    {
        private readonly CharacterSkillService _characterSkillService;

        public CharacterSkillController(CharacterSkillService characterSkillService)
        {
            _characterSkillService = characterSkillService;
        }
        [HttpPost]
        public async Task<IActionResult> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            return Ok(await _characterSkillService.AddCharacterSkill(newCharacterSkill));
        }
    }
}