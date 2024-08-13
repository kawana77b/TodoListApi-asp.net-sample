namespace TodoApi.Models.Attributes
{
    /// <summary>
    /// Attribute to mark a class as a database record
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RecordAttribute : Attribute
    {
    }
}