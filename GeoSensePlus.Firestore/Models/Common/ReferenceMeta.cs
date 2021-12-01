namespace GeoSensePlus.Firestore.Models
{
    /// <summary>
    /// Meta data of document reference and collection reference.
    /// </summary>
    public class ReferenceMeta
    {
        public string Path { get; }
        public string Type { get; }

        public ReferenceMeta(string path, string type)
        {
            Path = path;
            Type = type;
        }
    }
}