namespace BitCrafts.Application.Abstraction.Events;

public static partial class ViewEvents
{
    public static class Base
    {
        public const string AppearedEventName = "Base.Appeared";
        public const string DisappearedEventName = "Base.Disappeared";
        public const string CloseWindowEventName = "Base.Window.Close";
    }


    public static class Authentication
    {
        public const string CancelAuthenticationEventName = "Authentication.Cancel";
        public const string AuthenticateEventName = "Authentication.Authenticate";
        public const string ShowEnvironmentsEventName = "Authentication.ShowEnvironments";
    }
}