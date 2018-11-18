namespace SmartHome.Pwa.Infrastructure.Common
{
    public interface IMappableToBusinessModel<out T>
    {
        T MapToBusinessModel();
    }
}