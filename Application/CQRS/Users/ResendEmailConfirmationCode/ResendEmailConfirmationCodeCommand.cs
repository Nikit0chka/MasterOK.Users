using Ardalis.SharedKernel;
using Domain.Utils;

namespace Application.CQRS.Users.ResendEmailConfirmationCode;

public readonly record struct ResendEmailConfirmationCodeCommand(string Email):ICommand<OperationResult>;