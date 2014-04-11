using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApiScheme.Server;
using System.Reflection;
using System.Threading.Tasks;
using ApiServer.Models;
using Newtonsoft.Json;

namespace ApiServer.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            for (var n = 0; n < 10000; n++)
            {
                var assembly = Assembly.Load("ApiScheme");
                Assert.IsNotNull(assembly);
            }
        }
    }
}
