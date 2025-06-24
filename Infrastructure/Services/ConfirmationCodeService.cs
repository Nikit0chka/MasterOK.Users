using System.Security.Cryptography;
using Application.Contracts;

namespace Infrastructure.Services;

/// <inheritdoc />
/// <summary>
///     Confirmation code service implementation
/// </summary>
internal sealed class ConfirmationCodeService:IConfirmationCodeService
{
    private const int CodeLength = 6;
    private readonly static RandomNumberGenerator Rng = RandomNumberGenerator.Create();

    public string GenerateConfirmationCode()
    {
        var randomNumber = new byte[CodeLength];
        Rng.GetBytes(randomNumber);

        var code = string.Empty;

        for (var i = 0; i < CodeLength; i++)
        {
            code += (randomNumber[i] % 10).ToString();
        }

        return code;
    }
}