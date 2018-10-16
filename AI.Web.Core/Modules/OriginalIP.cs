using System;
using System.Text.RegularExpressions;
using System.Web;

namespace AI.Web.Core.Modules
{
    /// <summary>
    /// This module handles complications from our load balancer configuration not properly passing the client's true IP
    /// address to our code via the REMOTE_ADDR and REMOTE_HOST variables. We tried to use URL Rewrite to compensate for
    /// this, but it does not run when default documents are being accessed (a longstanding bug).
    ///
    /// Add a reference to this class to your <modules> section in either applicationHost.config (for your entire server),
    /// or the web.config of a specific web server. 
    /// From Gist: https://gist.github.com/winzig/ee57e559341a2a92d8ee7bc0daa1304a
    /// </summary>
    public class OriginalIP : HttpModuleBase
    {
        /// <summary>
        /// The IP we want will be in $1. X-Forwarded-For can carry multiple IPs in a comma-separated
        /// list, but the first IP should belong to the original client.
        /// </summary>
        private static Regex REGEX_FIRST_IP = new Regex(@"^\s*(\d+\.\d+\.\d+\.\d+)", RegexOptions.Compiled);

        public override void OnBeginRequest(HttpContextBase context)
        {
            if (context == null)
                return;

            var headervalue = context.Request.Headers["X-Forwarded-For"];

            if (headervalue != null)
            {
                var m = REGEX_FIRST_IP.Match(headervalue);

                if (m.Success)
                {
                    context.Request.ServerVariables["REMOTE_ADDR"] = m.Groups[1].Value;
                    context.Request.ServerVariables["REMOTE_HOST"] = m.Groups[1].Value;
                }
            }
        }        
    }
}
