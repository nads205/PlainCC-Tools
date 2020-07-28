using System.Collections.Generic;
using PayPal.Api;
using System;

namespace Examples.Core.IO
{
#warning  Paypal API not yet working
    public class PaypalAPI
    {        
        Dictionary<string, string> config;
        string accessToken; 
        APIContext apiContext;

        public PaypalAPI()
        {
            // Authenticate with PayPal
			config = ConfigManager.Instance.GetProperties();
            accessToken = new OAuthTokenCredential(config).GetAccessToken();
            apiContext = new APIContext(accessToken);
        }
        public void GetPaymentList()
        {
#warning this only returns two records
            var paymentList = Payment.List(apiContext, count: 999999, startIndex: 0);

#warning this only returns two records
            var today = DateTime.Today.ToString();
            var minus30days = DateTime.Today.AddDays(-30).ToString();

            var paymentList1 = Payment.List(apiContext, null, null, null,"","",today,minus30days);

#warning this isn't working
            // var payment = Payment.Get(apiContext, "2M342429CR958354C"); 
        }
        public void HelloWorld()
        {
		
            // Make an API call
            //        var payment = Payment.Create(apiContext, new Payment
            //        {
            //            intent = "sale",
            //            payer = new Payer
            //            {
            //                payment_method = "paypal"
            //            },
            //            transactions = new List<Transaction>
            //{
            //	new Transaction
            //	{
            //		description = "Transaction description.",
            //		invoice_number = "001",
            //		amount = new Amount
            //		{
            //			currency = "USD",
            //			total = "100.00",
            //			details = new Details
            //			{
            //				tax = "15",
            //				shipping = "10",
            //				subtotal = "75"
            //			}
            //		},
            //		item_list = new ItemList
            //		{
            //			items = new List<Item>
            //			{
            //				new Item
            //				{
            //					name = "Item Name",
            //					currency = "USD",
            //					price = "15",
            //					quantity = "5",
            //					sku = "sku"
            //				}
            //			}
            //		}
            //	}
            //            },
            //            redirect_urls = new RedirectUrls
            //            {
            //                return_url = "http://mysite.com/return",
            //                cancel_url = "http://mysite.com/cancel"
            //            }
            //        });
        }
    }
}
