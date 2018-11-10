#pragma warning disable SA1514
/// <summary>
/// The ids of the events being logged.
/// </summary>
public enum EventIds
{
    /// <summary>
    /// The request end event id.
    /// </summary>
    EndRequest,

    /// <summary>
    /// The user controller event id.
    /// </summary>
    UserController = 1000,

    /// <summary>
    /// The user controller get by id event id.
    /// </summary>
    UserController_GetById,

    /// <summary>
    /// The user controller create event id.
    /// </summary>
    UserController_Create,

    /// <summary>
    /// The user controller delete event id.
    /// </summary>
    UserController_Delete,

    /// <summary>
    /// The user controller login event id.
    /// </summary>
    UserController_Login,

    /// <summary>
    /// The role controller event id.
    /// </summary>
    RoleController = 2000,

    /// <summary>
    /// The role controller get by id event id.
    /// </summary>
    RoleController_GetById,

    /// <summary>
    /// The role controller create event id.
    /// </summary>
    RoleController_Create,

    /// <summary>
    /// The role controller delete event id.
    /// </summary>
    RoleController_Delete,
}
#pragma warning restore SA1514
