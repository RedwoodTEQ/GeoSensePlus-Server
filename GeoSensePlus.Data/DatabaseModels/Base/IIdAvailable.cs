namespace GeoSensePlus.Data.DatabaseModels.Base
{
    public interface IIdAvailable<T>
    {
        T GetId();
    }
}
