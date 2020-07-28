using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayPal.Api;

namespace Paypal.API
{
    public class Class1
    {
        public void ConnectToPaypal()
        {

            // Get a reference to the config
            var config = ConfigManager.Instance.GetProperties();

            // Use OAuthTokenCredential to request an access token from PayPal
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();

            var apiContext = new APIContext(accessToken);

            // Initialize the apiContext's configuration with the default configuration for this application.
            apiContext.Config = ConfigManager.Instance.GetProperties();

            // Define any custom configuration settings for calls that will use this object.
            apiContext.Config["connectionTimeout"] = "1000"; // Quick timeout for testing purposes

            // Define any HTTP headers to be used in HTTP requests made with this APIContext object
            if (apiContext.HTTPHeaders == null)
            {
                apiContext.HTTPHeaders = new Dictionary<string, string>();
            }
            apiContext.HTTPHeaders["some-header-name"] = "some-value";
            //try invoices
            var inoices = Invoice.GetAll(apiContext);

            var today = DateTime.Today.ToString();
            var minus30days = DateTime.Today.AddDays(-60).ToString();
            var paymentList1 = Payment.List(apiContext, null, null, null, "", "", today, minus30days);

            var payments = Payment.List(apiContext, count: 100, startIndex: 50);

            var paymentsAll = Payment.List(apiContext);

            try
            {
                var payment = Payment.Get(apiContext, "4D1701275C336714W");
            }
            catch { }
            try
            {
                var payment = Payment.Get(apiContext, "5V7546219T721753A");
            }
            catch
            {

            }
            try
            {
                var payment = Payment.Get(apiContext, "PAY-2XY28890EU215374H");
            }
            catch { }
           

        }
    }
}
