using System;
using Examples.Core;
using FileHelpers.MasterDetail;
using Examples.Core.IO;
using System.Collections.Specialized;
using System.Configuration;

namespace Examples
{
    //Test change
    class MainClass
    {
        [STAThread]
        static void Main()
        {
            //#warning  Paypal API not yet working
            var paypalApi = new PaypalAPI();
            paypalApi.GetPaymentList();

            SetConsoleUp();
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            Paypal paypal = new Paypal();
            MasterDetails[] recordsMs = paypal.LoadPaypalFile(appSettings["PaypalFilePath"]);
            Core.Generator generator = new Generator();
            var mList = generator.Generate(appSettings, recordsMs);
            if (mList != null && mList.Count > 0)
            {
                Magento magento = new Magento();
                magento.SaveMagentoFile(appSettings["MagentoFilePath"], appSettings["HeaderRow"], mList);
                Console.WriteLine(Process.Summary.ToString());
                Console.WriteLine("{0} records generated into new file {1}", mList.Count, appSettings["MagentoFilePath"]);
            }
            else
            {   //todo output summary?
                Console.WriteLine("No records generated. \r\nCheck application settings in app.config.\r\n");
            }
            Console.WriteLine("Press any key...");
            Console.ReadLine();
        }

        static void SetConsoleUp()
        {
            Console.SetWindowSize(154, 60);
        }
    }
}
