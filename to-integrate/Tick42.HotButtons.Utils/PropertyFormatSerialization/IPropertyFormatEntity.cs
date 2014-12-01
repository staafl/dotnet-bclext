namespace Tick42.HotButtons.Utils.PropertyFormatSerialization
{
    /// <summary>
    /// Implemented by an entity type that can be deserialized from a .property format storage.
    /// </summary>
    public interface IPropertyFormatEntity
    {
        string Name { get; set; }
    }
}