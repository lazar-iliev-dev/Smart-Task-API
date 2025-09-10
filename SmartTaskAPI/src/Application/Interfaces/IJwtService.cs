using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(UserDto user);
}