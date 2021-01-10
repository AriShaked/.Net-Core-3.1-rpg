using System.Threading.Tasks;
using dotnet_rpg.Dtos.Fight;
using dotnet_rpg.models;

namespace dotnet_rpg.services.FightService
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
    }
}