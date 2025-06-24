namespace Application.Contracts;

/// <summary>
///     Logic for confirmation codes
/// </summary>
public interface IConfirmationCodeService
{
    /// <summary>
    ///     Logic for generating confirmation code
    /// </summary>
    /// <returns> </returns>
    public string GenerateConfirmationCode();
}