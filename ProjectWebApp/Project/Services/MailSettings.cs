using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Services
{
    public class MailSettings : IMailSettings
    {
        public string Host { get; set; }
        public string MailAddress { get; set; }
        public string MailPassword { get; set; }
    }

    public interface IMailSettings
    {
        string Host { get; set; }
        string MailAddress { get; set; }
        string MailPassword { get; set; }
    }
}
