using NUnit.Framework;
using Examples.Core.DataStructures;
using FileHelpers;
using System.IO;
using FileHelpers.MasterDetail;
using Examples.Core.IO;
namespace Examples.Unit_Tests
{
    [TestFixture]
    public class PaypalFileLoading
    {
        RecordAction PaypalSelector(string record)
        {
            if (string.IsNullOrEmpty(record.Split(',')[8]))
                return RecordAction.Detail;
            else
                return RecordAction.Master;
        }
          
    }

    [TestFixture]
    public class AddressParseTests
    {
        [Test]
        public void ParseAddress1()
        {
            string fulladdress = "Eveline Hodel Makins, Maple Downe, 10 Main Drive, Gerrards Cross, Buckinghamshire, SL97PS, United Kingdom";
            var address = StringFunctions.ParseAddress(fulladdress);
            Assert.That(address.AddressLine1, Is.EqualTo("Maple Downe"));
            Assert.That(address.AddressLine2, Is.EqualTo("10 Main Drive"));
            Assert.That(address.TownorCity, Is.EqualTo("Gerrards Cross"));
            Assert.That(address.StateorCounty, Is.EqualTo("Buckinghamshire"));
            Assert.That(address.Postcode, Is.EqualTo("SL97PS"));
            Assert.That(address.Country, Is.EqualTo("United Kingdom"));
        }
        [Test]
        public void ParseAddress2()
        {
            string fulladdress = "Karen Cameron, 3 corsica ave South Morang, Melbourne, Victoria 3752, Australia";
            var address = StringFunctions.ParseAddress(fulladdress);
            Assert.That(address.AddressLine1, Is.EqualTo("3 corsica ave South Morang"));
            Assert.That(address.TownorCity, Is.EqualTo("Melbourne"));
            Assert.That(address.StateorCounty, Is.EqualTo("Victoria 3752"));
            Assert.That(address.Country, Is.EqualTo("Australia"));
        }
        [Test]
        public void ParseAddress3()
        {
            string fulladdress = "Timothy Wee, 37a Woo Mon Chew Rd, Singapore 455108";
            var address = StringFunctions.ParseAddress(fulladdress);
            Assert.That(address.AddressLine1, Is.EqualTo("37a Woo Mon Chew Rd"));
            Assert.That(address.Country, Is.EqualTo("Singapore 455108"));
        }
    }

    [TestFixture]
    public class NameParseTests
    {
        [Test]
        public void FirstLast()
        {
            string fullname = "Naadir Akhtar";
            var name = StringFunctions.ParseName(fullname);
            Assert.That(name.FirstName, Is.EqualTo("Naadir"));
            Assert.That(name.LastName, Is.EqualTo("Akhtar"));
        }
        [Test]
        public void FirstLastMiddleOneMiddle()
        {
            string fullname = "Tom Michael Jones";
            var name = StringFunctions.ParseName(fullname);
            Assert.That(name.FirstName, Is.EqualTo("Tom"));
            Assert.That(name.MiddleName, Is.EqualTo("Michael"));
            Assert.That(name.LastName, Is.EqualTo("Jones"));
        }
        [Test]
        public void FirstLastMiddleTwoMiddle()
        {
            string fullname = "T J Hewes";
            var name = StringFunctions.ParseName(fullname);
            Assert.That(name.FirstName, Is.EqualTo("T"));
            Assert.That(name.MiddleName, Is.EqualTo("J"));
            Assert.That(name.LastName, Is.EqualTo("Hewes"));
        }
        [Test]
        public void MrFirstLast()
        {
            string fullname = "Mr J Hewes";
            var name = StringFunctions.ParseName(fullname);
            Assert.That(name.Title, Is.EqualTo("Mr"));
            Assert.That(name.FirstName, Is.EqualTo("J"));
            Assert.That(name.LastName, Is.EqualTo("Hewes"));
        }
        [Test]
        public void DrFirstLast()
        {
            string fullname = "Dr Abraham Hewes";
            var name = StringFunctions.ParseName(fullname);
            Assert.That(name.Title, Is.EqualTo("Dr"));
            Assert.That(name.FirstName, Is.EqualTo("Abraham"));
            Assert.That(name.LastName, Is.EqualTo("Hewes"));
        }
        [Test]
        public void DrFirstMiddleLast()
        {
            string fullname = "Dr Abraham Franklin Hewes";
            var name = StringFunctions.ParseName(fullname);
            Assert.That(name.Title, Is.EqualTo("Dr"));
            Assert.That(name.FirstName, Is.EqualTo("Abraham"));
            Assert.That(name.MiddleName, Is.EqualTo("Franklin"));
            Assert.That(name.LastName, Is.EqualTo("Hewes"));
        }
        [Test,Ignore("I think this isn't working")]
        public void DrFirstMultiMiddleLast()
        {
            string fullname = "Dr Abraham A F Hewes";
            var name = StringFunctions.ParseName(fullname);
            Assert.That(name.Title, Is.EqualTo("Dr"));
            Assert.That(name.FirstName, Is.EqualTo("Abraham"));
            Assert.That(name.MiddleName, Is.EqualTo("A F"));
            Assert.That(name.LastName, Is.EqualTo("Hewes"));
        }
        [Test]
        public void FirstMultiMiddleLast()
        {
            string fullname = "Naadir Akbar Munir Akhtar";
            var name = StringFunctions.ParseName(fullname);
            Assert.That(name.FirstName, Is.EqualTo("Naadir"));
            Assert.That(name.MiddleName, Is.EqualTo("Akbar Munir"));
            Assert.That(name.LastName, Is.EqualTo("Akhtar"));
        }
        [Test]
        public void RevFirstLast()
        {
            string fullname = "Rev Naadir Akhtar";
            var name = StringFunctions.ParseName(fullname);
            Assert.That(name.FirstName, Is.EqualTo("Naadir"));
            Assert.That(name.LastName, Is.EqualTo("Akhtar"));
            Assert.That(name.Title, Is.EqualTo("Rev"));
        }
    }

    [TestFixture]
    public class CreditMemoGenerationTests
    {
        [Test]
        public void GenerateCreditMemo()
        {
            string t = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "../../../Core.UnitTests/Data/Creditmemo.txt");
            string target = File.ReadAllText(t);
            var creditmemostring = Process.CreateCreditmemoString();
            Assert.That(creditmemostring, Is.EqualTo(target));
        }
    }

    [TestFixture]
    public class CommentGenerationTests
    {
        [Test]
        public void CreateCommentString()
        {
            var file = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "../../../Core.UnitTests/Data/Comments.txt");
            string target = File.ReadAllText(file);
            var comment = Process.CreateCommentString();
            Assert.That(comment, Is.EqualTo(target));
        }
    }
    
    [TestFixture,Ignore("Invoice String not confirmed")]
    public class InvoiceGenerationTests
    {
        [Test]
        public void GenerateSimpleInvoice()
        {
            string t = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,"../../../Core.UnitTests/Data/Invoice.txt");
            string target = File.ReadAllText(t);
            MasterDetailEngine engine = new MasterDetailEngine(typeof(PaypalRecord), typeof(PaypalRecord), new MasterDetailSelector(Process.PaypalSelectorClass.PaypalSelector));
            string PaypalFilePath = "../../../Core.UnitTests/Data/Paypal_InvoiceSingleItem.csv";
            MasterDetails[] recordsMS = (MasterDetails[])engine.ReadFile(PaypalFilePath); 
            var invoicestring = Process.CreateInvoicesString(recordsMS[0],1,1,2);
            Assert.That(invoicestring, Is.EqualTo(target));
        }
    }
    [TestFixture]
    public class CustomerGenerationTests
    {
        [Test]
        public void TestCustomerGenerator1()
        {
            string t = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "../../../Core.UnitTests/Data/Customer.txt");
            string target = File.ReadAllText(t);
            var customerstring = Process.CreateCustomerString();
            Assert.That(customerstring, Is.EqualTo(target));
        }
    }
    [TestFixture]
    public class ShippingGenerationTests
    {
        [Test]
        public void TestShippingGenerator()
        {
            string t = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "../../../Core.UnitTests/Data/Shipping.txt");
            string target = File.ReadAllText(t);
            string PaypalFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "../../../Core.UnitTests/Data/Paypal_ShippingTest.csv");
            FileHelperEngine<PaypalRecord> paypalEngine = new FileHelperEngine<PaypalRecord>();
            PaypalRecord[] records = paypalEngine.ReadFile(PaypalFilePath);
            var shippingstring = Process.CreateShippingString(records[0],1);
            Assert.That(shippingstring, Is.EqualTo(target));
        }
    }
    [TestFixture]
    public class PaymentGenerationTests
    {
        [Test]
        public void TestPaymentGenerator()
        {
            string t = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "../../../Core.UnitTests/Data/Payment.txt");
            string target = File.ReadAllText(t);
            string PaypalFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Core.UnitTests\\Data\\Paypal_ShippingTest.csv");
            Assert.That(File.Exists(PaypalFilePath), "Paypal test file does not exist in directory");
            FileHelperEngine<PaypalRecord> paypalEngine = new FileHelperEngine<PaypalRecord>();
            PaypalRecord[] records = paypalEngine.ReadFile(PaypalFilePath);
            var paymentstring = Process.CreatePaymentString(records[0],1);
            Assert.That(paymentstring, Is.EqualTo(target));
        }
    }
    [TestFixture]
  
    [TestFixture]
    public class TestItemDescriptionParsing
    {
        [Test]
        public void TestItemDescriptionParsing1()
        {
            string stringTestDescription = "Plain Zipped Hooded Sweatshirt Boys Girls Childrens Kids Hoodie All Colours [Light grey,Age 4-5 years]";
            var ebayItem = StringFunctions.ParseEbayItem(stringTestDescription);
            Assert.That(ebayItem.Decription, Is.EqualTo("Plain Zipped Hooded Sweatshirt Boys Girls Childrens Kids Hoodie All Colours"));
            Assert.That(ebayItem.Size, Is.EqualTo(PCCsize.XS));
            Assert.That(ebayItem.Colour, Is.EqualTo("Light grey"));
            Assert.That(ebayItem.Item, Is.EqualTo(PCCitemcode.KHZA));
            Assert.That(ebayItem.SKU, Is.EqualTo("KHZA011OXS"));
        }
        [Test]
        public void TestItemDescriptionParsing2()
        {
            string stringTestDescription = "Plain Zipped Hooded Sweatshirt Boys Girls Childrens Kids Hoodie All Colours [Red,Age 4-5 years]";
            var ebayItem = StringFunctions.ParseEbayItem(stringTestDescription);
            Assert.That(ebayItem.Decription, Is.EqualTo("Plain Zipped Hooded Sweatshirt Boys Girls Childrens Kids Hoodie All Colours"));
            Assert.That(ebayItem.Size, Is.EqualTo(PCCsize.XS));
            Assert.That(ebayItem.Colour, Is.EqualTo("Red"));
            Assert.That(ebayItem.Item, Is.EqualTo(PCCitemcode.KHZA));
            Assert.That(ebayItem.SKU, Is.EqualTo("KHZA007OXS"));
        }
        [Test]
        public void TestItemDescriptionParsing3()
        {
            string stringTestDescription = "Plain Zipped Hooded Sweatshirt Boys Girls Childrens Kids Hoodie All Colours [red,Age 4-5 years]";
            var ebayItem = StringFunctions.ParseEbayItem(stringTestDescription);
            Assert.That(ebayItem.Decription, Is.EqualTo("Plain Zipped Hooded Sweatshirt Boys Girls Childrens Kids Hoodie All Colours"));
            Assert.That(ebayItem.Size, Is.EqualTo(PCCsize.XS));
            Assert.That(ebayItem.Colour, Is.EqualTo("red"));
            Assert.That(ebayItem.Item, Is.EqualTo(PCCitemcode.KHZA));
            Assert.That(ebayItem.SKU, Is.EqualTo("KHZA007OXS"));
        }
        [Test]
        public void TestItemDescriptionParsing4()
        {
            string stringTestDescription = "Kids Sweat Jog Pants Jogging Childrens Bottoms Sports Gym Heavy Boys Girls Plain [10-11,Black]";
            var ebayItem = StringFunctions.ParseEbayItem(stringTestDescription);
            Assert.That(ebayItem.Decription, Is.EqualTo("Kids Sweat Jog Pants Jogging Childrens Bottoms Sports Gym Heavy Boys Girls Plain"));
            Assert.That(ebayItem.Size, Is.EqualTo(PCCsize.L));
            Assert.That(ebayItem.Colour, Is.EqualTo("Black"));
            Assert.That(ebayItem.Item, Is.EqualTo(PCCitemcode.KJPA));
            Assert.That(ebayItem.SKU, Is.EqualTo("KJPA006OOL"));
        }
        [Test]
        public void TestItemDescriptionParsing5()
        {
            string stringTestDescription = "Womens Ladies Sweat Joggers Pants Bottoms Jogging New Gym Fitness Plain Quality [Light Grey,XS (6-8)]";
            var ebayItem = StringFunctions.ParseEbayItem(stringTestDescription);
            Assert.That(ebayItem.Decription, Is.EqualTo("Womens Ladies Sweat Joggers Pants Bottoms Jogging New Gym Fitness Plain Quality"));
            Assert.That(ebayItem.Size, Is.EqualTo(PCCsize.XS));
            Assert.That(ebayItem.Colour, Is.EqualTo("Light Grey"));
            Assert.That(ebayItem.Item, Is.EqualTo(PCCitemcode.LJPA));
            Assert.That(ebayItem.SKU, Is.EqualTo("LJPA011OXS"));
        }
        [Test]
        public void TestItemDescriptionParsing6()
        {
            string stringTestDescription = "Mens Sweat Pull Over Hoodie";
            var ebayItem = StringFunctions.ParseEbayItem(stringTestDescription);
            Assert.That(ebayItem, Is.Null);
        }
        [Test]
        public void TestItemDescriptionParsing7()
        {
            string stringTestDescription = "Womens Ladies Gym Yoga Pilates Dance  Pants Trousers All Sizes 6 8 10 12 14 16 [Charcoal,XL (14-16)]";
            var ebayItem = StringFunctions.ParseEbayItem(stringTestDescription);
            Assert.That(ebayItem.Decription, Is.EqualTo("Womens Ladies Gym Yoga Pilates Dance  Pants Trousers All Sizes 6 8 10 12 14 16"));
            Assert.That(ebayItem.Size, Is.EqualTo(PCCsize.XL));
            Assert.That(ebayItem.Colour, Is.EqualTo("Charcoal"));
            Assert.That(ebayItem.Item, Is.EqualTo(PCCitemcode.LJPA));
            Assert.That(ebayItem.SKU, Is.EqualTo("LJPA020OXL"));
        }
    }
    [TestFixture]
    public class TestSizeParsing
    {
        [Test]
        public void TestLadiesSizeParse1()
        {
            var size = StringFunctions.DetermineItemSizeFromSizestring("6-8", PCCitemcode.LJPA);
            Assert.That(PCCsize.XS, Is.EqualTo(size));
        }
        [Test]
        public void TestKidsSizeParse1()
        {
            var size = StringFunctions.DetermineItemSizeFromSizestring("2-3", PCCitemcode.KJPA);
            Assert.That(PCCsize.XXS, Is.EqualTo(size));
        }
        [Test]
        public void TestMensSizeParse1()
        {
            var size = StringFunctions.DetermineItemSizeFromSizestring("M", PCCitemcode.MHPA);
            Assert.That(PCCsize.M, Is.EqualTo(size));
        }
        [Test]
        public void TestMensSizeParse2()
        {
            var size = StringFunctions.DetermineItemSizeFromSizestring("xs", PCCitemcode.MHZA);
            Assert.That(PCCsize.XS, Is.EqualTo(size));
        }
        [Test]
        public void TestMensSizeParse3()
        {
            var size = StringFunctions.DetermineItemSizeFromSizestring("XS", PCCitemcode.MHZA);
            Assert.That(PCCsize.XS, Is.EqualTo(size));
        }
        [Test]
        public void TestMensSizeParse4()
        {
            var size = StringFunctions.DetermineItemSizeFromSizestring("XL", PCCitemcode.MHZA);
            Assert.That(PCCsize.XL, Is.EqualTo(size));
        }
    }
}
