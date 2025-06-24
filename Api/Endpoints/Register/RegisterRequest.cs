namespace API.Endpoints.Register;

public readonly record struct RegisterRequest(string Email, string Password);