using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class CryptoUT
    {
        [TestMethod]
        public void HashHMAC_Pass()
        {
            string message = "ssoUserId=0743cd2c-fec3-4b79-a5b6-a6c52a752c71;email=julianpoyo+22@gmail.com;timestamp=1552766624957;";
            string sharedSecretKey = "D078F2AFC7E59885F3B6D5196CE9DB716ED459467182A19E04B6261BBC8E36EE";
            CryptoService cs = new CryptoService();
            

            var expected = "4T5Csu2U9OozqN66Us+pEc5ODcBwPs1ldaq2fmBqtfo=";
            var actual = cs.HashHMAC(Encoding.ASCII.GetBytes(sharedSecretKey), message);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HashHMAC_Fail()
        {
            string message = "ssoUserId=0743cd2c-fec3-4b79-a5b6-a6c52a752c71;email=julianpoyo+22@gmail.com;timestamp=1552766624957;";
            string sharedSecretKey = "D078F2AFC7E59885F3B6D5196CE9DB716ED459467182A19E04B6261BBC8E36EE";
            CryptoService cs = new CryptoService();


            var expected = "asdf";
            var actual = cs.HashHMAC(Encoding.ASCII.GetBytes(sharedSecretKey), message);

            Assert.AreNotEqual(expected, actual);
        }
    }
}
