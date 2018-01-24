namespace BellRichM.Identity.Api.Exceptions
{
  /// <summary>
  /// Exception codes when creating the role fails.
  /// </summary>
  public static class CreateRoleExceptionCode
  {
    /// <summary>
    /// The create role failed.
    /// </summary>
    public const string CreateRoleFailed = "CreateRoleFailed";

    /// <summary>
    /// The add claim failed.
    /// </summary>
    public const string AddClaimFailed = "AddClaimFailed";
  }
}