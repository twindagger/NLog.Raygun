using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.Messages;
using NLog.Config;
using NLog.Targets;

namespace NLog.Raygun
{
    [Target("RayGun")]
    public class RayGunTarget : TargetWithLayout
    {
        [RequiredParameter]
        public string ApiKey { get; set; }

        [RequiredParameter]
        public string Tags { get; set; }

        [RequiredParameter]
        public string IgnoreFormFieldNames { get; set; }

        [RequiredParameter]
        public string IgnoreCookieNames { get; set; }

        [RequiredParameter]
        public string IgnoreServerVariableNames { get; set; }

        [RequiredParameter]
        public string IgnoreHeaderNames { get; set; }

        protected override void Write(LogEventInfo logEvent)
        {
            string logMessage = Layout.Render(logEvent);
            
            var client = CreateRaygunClient();
            client.Send(RaygunMessageBuilder.New
              .SetEnvironmentDetails()
              .SetMachineName(Environment.MachineName)
              .SetExceptionDetails(logEvent.Exception ?? logEvent.Parameters[0] as Exception)
              .SetClientDetails()
              .SetTags(SplitValues(Tags))
              .SetUserCustomData(new Dictionary<string, string>
              {
                  {"logMessage", logMessage}
              })
              .Build());
        }

        private RaygunClient CreateRaygunClient()
        {
            var client = new RaygunClient(ApiKey);

            client.IgnoreFormFieldNames(SplitValues(IgnoreFormFieldNames));
            client.IgnoreCookieNames(SplitValues(IgnoreCookieNames));
            client.IgnoreHeaderNames(SplitValues(IgnoreHeaderNames));
            client.IgnoreServerVariableNames(SplitValues(IgnoreServerVariableNames));

            return client;
        }

        private string[] SplitValues(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Split(',');
            }

            return new[]{""};
        }
    }
}