namespace TangoManagerAPI.Entities.Ports
{
    internal interface ICloneable<out T>
    {
        T Clone();
    }
}
