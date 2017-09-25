using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tcs.Watson.BL
{
    public class Conversation
    {
        Conversation()
        {
            string baseUrl = ConfigurationManager.AppSettings["baseUrl"].ToString();

            //TODO: Incluir en constructor de WCF
            var workspace = "6dcec688-83d2-4cd0-99c5-66e90278f0e7";
            //TODO: Incluir en constructor de WCF
            var username = "6dae1a60-2bfa-4f62-890b-7be7048104ef";
            //TODO: Incluir en constructor de WCF
            var password = "uuiT4XrIZ5Wa";

        }
    }
}
