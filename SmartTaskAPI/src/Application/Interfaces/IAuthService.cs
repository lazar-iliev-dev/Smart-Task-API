using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface IAuthService
    {
        Task<UserDto> RegisterAsync(string username, string password, string role);
        Task<UserDto> LoginAsync(string username, string password);
}
