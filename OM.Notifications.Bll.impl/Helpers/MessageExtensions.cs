using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OM.Notifications.BLL.Impl.Helpers
{
    public static class MessageExtensions
    {
        public static void AddVariable(this Processors.Message message, string name, string value)
        {
            if (message == null)
                return;

            if (message.Variables == null)
                message.Variables = new List<Entities.MessageVariable>();

            message.Variables.Add(new Entities.MessageVariable { Name = name, Value = value });
        }

        public static void AddVariables(this Processors.Message message, IEnumerable<Entities.MessageVariable> variables)
        {
            if (message == null || variables == null)
                return;

            if (message.Variables == null)
                message.Variables = new List<Entities.MessageVariable>();

            var existingVarNames = message.Variables
                .Select(v => v.Name.Replace("@", string.Empty))
                .ToList();

            message.Variables = message.Variables.Concat(
                variables
                    .Where(v => !existingVarNames.Contains(v.Name)))
                .ToList();
        }

        public static string GetParameter(this Processors.Message message, string name)
        {
            var variable = message?.Variables?.FirstOrDefault(v => v.Name == name);
            return variable?.Value;
        }
    }
}
