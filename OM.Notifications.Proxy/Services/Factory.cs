namespace OM.Notifications.Facade.Services
{
    public static class Factory
    {
        internal static BLL.INotificationsService Build(bool isProxy = true)
        {
            if (isProxy)
            {
                if (Properties.Settings.Default.UseWindowsServiceProxy)
                    return new WsProxyService(Properties.Settings.Default.StandardProxyEndpoint);
                else
                    return new WsProxyService(Properties.Settings.Default.LegacyProxyEndpoint);    
            }
            else
            {
                return new BLL.Impl.NotificationsService(
                    new BLL.Impl.Builders.MessageBuilderFactory(),
                    new Processors.BizTalkProxy.WebServiceMessageProcessorFactory(),
                    new Processors.Services.Impl.SmtpMailService());
            }
        }
    }

    internal enum ServiceType
    {
        Implementation,
        Proxy
    }
}