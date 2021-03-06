#pragma warning disable CA1707 // Need/want to separate event group from specific event.
namespace BellRichM.Logging
{
    /// <summary>
    /// The ids of the events being logged.
    /// </summary>
    public enum EventId
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

    /// <summary>
    /// The logging level controller event id.
    /// </summary>
    LoggingLevelController = 3000,

    /// <summary>
    /// The logging level  controller get event id.
    /// </summary>
    LoggingLevelController_Get,

    /// <summary>
    /// The logging level  controller update event id.
    /// </summary>
    LoggingLevelController_Update,

    /// <summary>
    /// The logging filter controller event id.
    /// </summary>
    LoggingFilterController = 3000,

    /// <summary>
    /// The logging filter  controller get event id.
    /// </summary>
    LoggingFilterController_Get,

    /// <summary>
    /// The logging filter  controller update event id.
    /// </summary>
    LoggingFilterController_Update,

    /// <summary>
    /// The conditions controller event id.
    /// </summary>
    ConditionsController = 4000,

    /// <summary>
    /// The conditions controller GetYearsConditionPage event id.
    /// </summary>
    ConditionsController_GetYearsConditionPage,

    /// <summary>
    /// The conditions controller GetYearDetail event id.
    /// </summary>
    ConditionsController_GetYearDetail,

    /// <summary>
    /// The conditions controller GetMonthsConditionPage event id.
    /// </summary>
    ConditionsController_GetMonthsConditionPage,

    /// <summary>
    /// The conditions controller GetMonthDetail event id.
    /// </summary>
    ConditionsController_GetMonthDetail,

    /// <summary>
    /// The conditions controller GetDaysConditionPage event id.
    /// </summary>
    ConditionsController_GetDaysConditionPage,

    /// <summary>
    /// The conditions controller GetDayDetail event id.
    /// </summary>
    ConditionsController_GetDayDetail,

    /// <summary>
    /// The conditions controller GetHoursConditionPage event id.
    /// </summary>
    ConditionsController_GetHoursConditionPage,

    /// <summary>
    /// The conditions controller GetHourDetail event id.
    /// </summary>
    ConditionsController_GetHourDetail,

    /// <summary>
    /// The conditions controller GetYearsMonthConditionPage event id.
    /// </summary>
    ConditionsController_GetYearsMonthConditionPage,

    /// <summary>
    /// The conditions controller GetYearsDayConditionPage event id.
    /// </summary>
    ConditionsController_GetYearsDayConditionPage,

    /// <summary>
    /// The conditions controller GetYearsHourConditionPage event id.
    /// </summary>
    ConditionsController_GetYearsHourConditionPage,

    /// <summary>
    /// The condition controller get min max conditions by minute.
    /// </summary>
    ConditionsController_GetMinMaxConditionsByMinute,

    /// <summary>
    /// The condition controller get min max conditions by hoyr.
    /// </summary>
    ConditionsController_GetMinMaxConditionsByHour,

    /// <summary>
    /// The condition controller get min max conditions by day.
    /// </summary>
    ConditionsController_GetMinMaxConditionsByDay,

    /// <summary>
    /// The condition controller get min max conditions by week.
    /// </summary>
    ConditionsController_GetMinMaxConditionsByWeek,    

    /// <summary>
    /// The condition controller get observations event id.
    /// </summary>
    ConditionsController_GetConditionsByDay,

    /// <summary>
    /// The observation controller event id.
    /// </summary>
    ObservationsController = 5000,

    /// <summary>
    /// The observation controller get event id.
    /// </summary>
    ObservationsController_Get,

    /// <summary>
    /// The observation controller get observations event id.
    /// </summary>
    ObservationsController_GetObservations,

    /// <summary>
    /// The observation controller get observstion datetimesevent id.
    /// </summary>
    ObservationsController_GetTimestamps,

    /// <summary>
    /// The observation controller create event id.
    /// </summary>
    ObservationsController_Create,

    /// <summary>
    /// The observation controller create event id.
    /// </summary>
    ObservationsController_Update,

    /// <summary>
    /// The observation controller delete event id.
    /// </summary>
    ObservationsController_Delete,

    /// <summary>
    /// The users controller event id.
    /// </summary>
    UsersController = 6000,

    /// <summary>
    /// The users controller get event id.
    /// </summary>
    UsersController_Get,

    /// <summary>
    /// The roles controller event id.
    /// </summary>
    RolesController = 7000,

    /// <summary>
    /// The roles controller get event id.
    /// </summary>
    RolesController_Get,
    }
}
#pragma warning restore CA1707
