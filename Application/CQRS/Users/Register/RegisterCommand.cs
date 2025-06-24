using Ardalis.SharedKernel;
using Domain.Utils;

namespace Application.CQRS.Users.Register;

public readonly record struct RegisterCommand(string Email, string Password):ICommand<OperationResult>;