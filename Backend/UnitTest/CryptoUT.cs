using Gucci.ServiceLayer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class CryptoUT
    {
        [TestMethod]
        public void HashHMAC_Pass()
        {
            string message = "ssoUserId=0743cd2c-fec3-4b79-a5b6-a6c52a752c71;email=julianpoyo+22@gmail.com;timestamp=1552766624957;";
            CryptoService cs = new CryptoService("D078F2AFC7E59885F3B6D5196CE9DB716ED459467182A19E04B6261BBC8E36EE");
            

            var expected = "4T5Csu2U9OozqN66Us+pEc5ODcBwPs1ldaq2fmBqtfo=";
            var actual = cs.HashHMAC(message);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HashHMAC_Fail()
        {
            string message = "ssoUserId=0743cd2c-fec3-4b79-a5b6-a6c52a752c71;email=julianpoyo+22@gmail.com;timestamp=1552766624957;";
            CryptoService cs = new CryptoService("D078F2AFC7E59885F3B6D5196CE9DB716ED459467182A19E04B6261BBC8E36EE");


            var expected = "asdf";
            var actual = cs.HashHMAC(message);

            Assert.AreNotEqual(expected, actual);
        }
    }
}
