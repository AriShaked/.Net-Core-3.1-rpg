using System;
using System.Linq;
using System.Security.Claims;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Dtos.CharacterSkill;
using dotnet_rpg.models;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.services.CharacterSkillService
{
    public class CharacterSkillService : ICharacterSkillService
    {
        private DataContext _context;
        private IHttpContextAccessor _httpContextAccessor;
        private IMapper _mapper;

        public CharacterSkillService(DataContext context , IHttpContextAccessor httpContextAccessor , IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character character = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.CharacterSkills).ThenInclude(cs => cs.Skill)
                    .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId &&
                      c.User.Id ==  int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                if(character == null)
                {
                    response.Success = false;
                    response.Message = "Character not found";
                    return response;
                }

                Skill skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);

                if (skill == null)
                {
                    response.Success = false;
                    response.Message = "Skill not found";
                    return response;
                }

                CharacterSkill characterSkill = new CharacterSkill
                {
                    Character = character,
                    Skill = skill
                };

                await _context.CharacterSkills.AddAsync(characterSkill);
                await _context.SaveChangesAsync();
                response.Data = 
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
        }
    }
}