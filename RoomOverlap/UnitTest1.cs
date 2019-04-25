using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace RoomOverlap
    {
    [TestClass]
    public class UnitTest1
        {
        [TestMethod]
        bool CheckRoomOverlap(Tuple<int, int> startPos1, Tuple<int, int> endPos1, Tuple<int, int> startPos2, Tuple<int, int> endPos2)
            {
            // Ranges of new room.
            IEnumerable<int> XRange1 = Enumerable.Range(startPos1.Item1, (endPos1.Item1 - startPos1.Item1));
            IEnumerable<int> YRange1 = Enumerable.Range(startPos1.Item2, (endPos1.Item2 - startPos1.Item2));
            // Ranges of reference room.
            IEnumerable<int> XRange2 = Enumerable.Range(startPos2.Item1, (endPos2.Item1 - startPos2.Item1));
            IEnumerable<int> YRange2 = Enumerable.Range(startPos2.Item2, (endPos2.Item2 - startPos2.Item2));


            // check for intersections. return -1 if empty
            var xinter = XRange1.Intersect(XRange2).DefaultIfEmpty(-1);
            var yinter = YRange1.Intersect(YRange1).DefaultIfEmpty(-1);

            if (xinter.ElementAt(0) == -1 && yinter.ElementAt(0) == -1)
                {
                foreach(int xitem in xinter)
                    {
                    Console.WriteLine(xitem);
                    }
                foreach (int yitem in yinter)
                    {
                    Console.WriteLine(yitem);
                    }
                return true;
                }
            else
                {
                return false;
                }
            }
        }
    }