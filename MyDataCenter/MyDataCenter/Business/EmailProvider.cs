using Microsoft.Exchange.WebServices.Data;
using System;

namespace MyDataCenter.Business
{
    public class EmailProvider
    {
        public void GetEmails()
        {
            try
            {
                var service = new ExchangeService();
                service.Credentials = new WebCredentials("hapetersiv@gmail.com", "");
                service.Url = new Uri("hapetersiv@gmail.com");

                //service.AutodiscoverUrl("hapetersiv@gmail.com", RedirectionUrlValidationCallback);

                var view = new ItemView(int.MaxValue);

                PropertySet propSet = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Body, ItemSchema.DateTimeReceived);
                var response = service.FindItems(WellKnownFolderName.Inbox, view);


            }
            catch(Exception ex)
            {
                var thing = ex.Data;
            }
            
        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }
    }
}