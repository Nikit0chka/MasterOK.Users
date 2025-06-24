namespace API.Endpoints.ConfirmEmail;

public readonly record struct ConfirmEmailRequest(string Email, string ConfirmationCode);