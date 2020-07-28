using NUnit.Framework;
using Paypal.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paypal.API.Tests
{
    [TestFixture]
    public class Paypal
    {
        [Test]
        public void TestConnection()
        {
            var p = new Class1();
            p.ConnectToPaypal();
        }
    }
}
