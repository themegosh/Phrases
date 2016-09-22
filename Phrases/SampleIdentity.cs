using System.Web.Security;

namespace Phrases
{
    internal class SampleIdentity
    {
        private FormsAuthenticationTicket ticket;

        public SampleIdentity(FormsAuthenticationTicket ticket)
        {
            this.ticket = ticket;
        }
    }
}