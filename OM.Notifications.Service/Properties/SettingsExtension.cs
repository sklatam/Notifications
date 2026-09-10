namespace OM.Notifications.Properties
{
    /// <summary>
    /// Implementaciones explicitas de intefaces de Settings.
    /// Permite pasar los Settings como dependencia para la resolucion de servicios en el Container
    /// </summary>
    public partial class Settings : Processors.OnlineService.Settings.IOnlineProcessorSettings
    {
    }
}