using Ardalis.SharedKernel;
using Domain.Utils;

namespace Application.CQRS.Users.ConfirmEmail;

public readonly record struct ConfirmEmailCommand(string Email, string ConfirmationCode):ICommand<OperationResult>;