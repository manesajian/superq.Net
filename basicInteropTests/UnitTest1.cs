using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using superqDotNet;

namespace basicInteropTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void BasicInstantiation()
        {
            superq sq = superq.Create(new object[0]);
        }
    }
}
