using System;
using System.Net;
using Microsoft.Exchange.WebServices.Data;

namespace ExCh
{
    public class Check
    {
        public void Test()
        {
            try
            {
                var exchangeService = GetService();
                var items = exchangeService.FindItems(
                    WellKnownFolderName.Inbox,
                    new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false),
                    new ItemView(1)).Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee");
                Console.WriteLine(e.InnerException?.Message);
            }
        }
        private ExchangeService GetService()
        {
            var version = ExchangeVersion.Exchange2016;
            var userName = "ekul";
            var password = "C0m1ndw4r3Pl@tf0rm";
            var domain = "corp";
            var displayName = "Часовой пояс";
            var credential = new NetworkCredential(userName, password, domain);
            var host = new Uri("https://mail.comindware.com/EWS/Exchange.asmx");
            return new ExchangeService(version,
                TimeZoneInfo.CreateCustomTimeZone(displayName,
                    TimeZoneInfo.Local.GetUtcOffset(DateTime.Now),
                    displayName,
                    displayName))
            {
                Timeout = (int)100000,
                Credentials = credential,
                Url = host,
            };
        }
    }
}