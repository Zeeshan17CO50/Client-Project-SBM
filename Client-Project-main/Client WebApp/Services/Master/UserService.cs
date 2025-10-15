using Client.Application.Features.User.Dtos;
using Client.Application.Interfaces;

namespace Client_WebApp.Services.Master
{
    public class UserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public Task<List<UserDto>> GetAllUsersAsync(int companyId, int? id = null, string? search = null)
        {
            return _repository.GetUsersAsync(id, search, companyId);
        }

        public Task<List<UserDto>> CreateUserAsync(CreateUserDto dto)
        {
            return _repository.CreateUserAsync(dto);
        }

        public Task<List<UserDto>> UpdateUserAsync(UpdateUserDto dto)
        {
            return _repository.UpdateUserAsync(dto);
        }

        public Task<List<UserDto>> DeleteUserAsync(int id, int updatedBy, int companyId)
        {
            return _repository.DeleteUserAsync(id, updatedBy, companyId);
        }

        public Task<List<UserDto>> ToggleActiveAsync(ToggleUserActiveDto dto, int companyId)
        {
            return _repository.ToggleIsActiveAsync(dto, companyId);
        }
    }
}
