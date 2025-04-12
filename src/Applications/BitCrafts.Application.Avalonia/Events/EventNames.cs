namespace BitCrafts.Application.Avalonia.Events;

public static class EventNames
{
    public static class Base
    {
        public const string Appeared = "Base.AppearedEvent";
        public const string Disappeared = "Base.DisappearedEvent";
    }

    public static class Editable
    {
        public const string Save = "Editable.SaveEvent";
        public const string Cancel = "Editable.CancelEvent";
        public const string Clear = "Editable.ClearEvent";
    }
}