namespace BellRichM.Identity.Api.Exceptions
{
  /// <summary>
  /// Exception codes when deleting the user fails.
  /// </summary>
  public static class DeleteUserExceptionCode
  {
    /// <summary>
    /// The user not found.
    /// </summary>
    public const string UserNotFound = "UserNotFound";

    /// <summary>
    /// The delete user failed.
    /// </summary>
    public const string DeleteUserFailed = "DeleteUserFailed";
  }
}