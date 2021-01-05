using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data
{
    public class AuthRepository : IAuthRepository
    {

        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public AuthRepository(DataContext DataContext, IMapper mapper = null)
        {
            _context = DataContext;
            _mapper = mapper;
        }

        public DataContext DataContext { get; }

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));
            if(user == null) {
                response.Success = false;
                response.Message = "user or password is incorrect";
            } else if(!VerfiyPasswordHash(password  , user.PasswordHash , user.PasswordSalt)){
                response.Success = false;
                response.Message = "user or password is incorrect";
            } else {
                response.Data = user.Id.ToString();
            }
            return response;

        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            if (await UserExsits(user.Username))
            {
                response.Success = false;
                response.Message = "somthing went wrong"; // "Username already taken";
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            response.Data = user.Id;
            return response;
        }

        public async Task<bool> UserExsits(string username)
        {
            var isUserExsits = await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
            return isUserExsits;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }

        private bool VerfiyPasswordHash(string password , byte[] passwordHash , byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length ; i++)
                {
                    if(computedHash[i] != passwordHash[i]) {
                        return false;
                    }
                }
                    return true;
            }
        }
    }
}