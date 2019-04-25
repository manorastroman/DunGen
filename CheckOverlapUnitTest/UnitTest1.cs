using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace CheckOverlapUnitTest
    {
    [TestClass]
    public class UnitTest1
        {
        

        private const bool Expected = false;
        private Tuple<int, int> Rm1st = new Tuple<int, int>(3, 7);
        private Tuple<int, int> Rm1en = new Tuple<int, int>(8, 5);
        private Tuple<int, int> Rm2st = new Tuple<int, int>(3, 2);
        private Tuple<int, int> Rm2en = new Tuple<int, int>(8, 6);


        [TestMethod]
        public void TestMethod1()
            {
            bool result = DungeonGen.mathFunctions.CheckRoomOverlap(Rm1st, Rm1en, Rm2st, Rm2en);
            Assert.AreEqual(Expected, result);
            }
        }
    }
