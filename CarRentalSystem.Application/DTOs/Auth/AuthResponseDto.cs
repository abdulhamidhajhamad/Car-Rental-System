namespace CarRentalSystem.Application.DTOs.Auth;

public record AuthResponseDto(
    string UserId,
    string Email,
    string Token
);