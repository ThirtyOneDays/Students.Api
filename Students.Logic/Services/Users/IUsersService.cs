using System.Threading.Tasks;
using Students.Logic.Models.Users;

namespace Students.Logic.Services.Users
{
  public interface IUsersService
  {
    Task<LoginResponse> TryInsertIfUniqueAsync(LoginRequest request);
    Task<LoginResponse> GetUserAsync(LoginRequest request);
  }
}
