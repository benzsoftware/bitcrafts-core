namespace BitCrafts.Application.Abstraction.Events;

public static class ViewEvents
{
    public static class Base
    {
        public const string AppearedEventName = "Base.Appeared";
        public const string DisappearedEventName = "Base.Disappeared";
        public const string CloseWindowEventName = "Base.Window.Close";
    }
}