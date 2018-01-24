namespace BellRichM.Identity.Api.Exceptions
{
  /// <summary>
  /// Exception codes when creating the user fails.
  /// </summary>
  public static class CreateUserExceptionCode
  {
    /// <summary>
    /// The create user failed.
    /// </summary>
    public const string CreateUserFailed = "CreateUserFailed";

    /// <summary>
    /// The role not found.
    /// </summary>
    public const string RoleNotFound = "RoleNotFound";

    /// <summary>
    /// The add role failed.
    /// </summary>
    public const string AddRoleFailed = "AddRoleFailed";
  }
}