using OM.Notifications.Entities;

namespace OM.Notifications.Test.Stubs
{
    internal static class MessageVariableStub
    {
        public static MessageVariable Create(string name, string value)
        {
            return new MessageVariable
            {
                Name = name,
                Value = value
            };
        }
    }
}
