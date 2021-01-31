using System;
using System.Threading.Tasks;
using AutoMapper;
using Students.Logic.Models.Users;
using Students.Repository.Entities;
using Students.Repository.Interfaces;

namespace Students.Logic.Services.Users
{
  public class UsersService : IUsersService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    public UsersService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
      _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponse> TryInsertIfUniqueAsync(LoginRequest loginRequest)
    {
      var dbUser = _mapper.Map<DbUser>(loginRequest);
      dbUser.Password = _passwordHasher.Hash(loginRequest.Password);
      try
      {
        await _unitOfWork.UserRepository.CreateAsync(dbUser);
        return _mapper.Map<LoginResponse>(dbUser);
      }
      catch (Exception)//TODO: catch sql exception
      {
        return null;
      }
    }

    public async Task<LoginResponse> GetUserAsync(LoginRequest loginRequest)
    {
      var dbUser = await _unitOfWork.UserRepository.GetAsync(x => x.UserName == loginRequest.UserName);
      if (dbUser == null || !_passwordHasher.Compare(dbUser.Password, loginRequest.Password))
        return null;

      var result = _mapper.Map<LoginResponse>(dbUser);
      return result;
    }
  }
}
