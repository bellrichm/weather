namespace BellRichM.Identity.Api.Exceptions
{
  /// <summary>
  /// Exception codes when deleting the role fails.
  /// </summary>
  public static class DeleteRoleExceptionCode
  {
    /// <summary>
    /// The role not found.
    /// </summary>
    public const string RoleNotFound = "RoleNotFound";

    /// <summary>
    /// The delete role failed
    /// </summary>
    public const string DeleteRoleFailed = "DeleteRoleFailed";
  }
}