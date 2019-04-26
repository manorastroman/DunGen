/*
 *  This file is part of DunGen.
 *
 *  DunGen is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  DunGen is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with DunGen.  If not, see <https://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Linq;


namespace DungeonGen
    {
    public class GridMap
        {
        /* room byte values
         * 0 = Wall/unaccessable
         * 1 = Open Floor
         * 2 = NW corner
         * 3 = NE corner
         * 4 = SW corner
         * 5 = SE corner
         * 6 = N Wall
         * 7 = S Wall
         * 8 = W Wall
         * 9 = E Wall
         * 10 = NS corridor
         * 11 = EW corridor
         * 12 = N Wall door
         * 13 = S Wall door
         * 14 = W Wall door
         * 15 = E Wall door
         * 16 = WN corner Coridor
         * 17 = WS corner coridor
         * 18 = EN corner coridor
         * 19 = ES corner coridor
         * 20 = Dead End coridor
         */

        // delcare mapGrid
        private Dictionary<Tuple<int, int>, byte> mapGrid;
        // declare other map data
        public int mapX, mapY, numberRooms, minRoomSize, maxRoomSize;
        private Random random = new Random();

        public GridMap(int X, int Y, int rms)
            {
            this.mapX = X;
            this.mapY = Y;
            this.numberRooms = rms;
            mapGrid = GenerateMap(mapX, mapY, numberRooms);
            }

        public byte GetFromMap(int t1, int t2)
            {
            Tuple<int, int> coords = new Tuple<int, int>(t1, t2);
            return mapGrid[coords];
            }

        private Dictionary<Tuple<int, int>, byte> GenerateMap(int Dim_X, int Dim_Y, int numRooms)
            {
            Dictionary<Tuple<int, int>, byte> workMap = new Dictionary<Tuple<int, int>, byte>();
            // init map
            for (int sub_x = 0; sub_x < Dim_X; sub_x++)
                {
                for (int sub_y = 0; sub_y < Dim_Y; sub_y++)
                    {
                    Tuple<int, int> current_coord = new Tuple<int, int>(sub_x, sub_y);
                    workMap[current_coord] = 0;
                    }
                }
       
            List<Tuple<Tuple<int, int>, Tuple<int, int>>> roomList;
            roomList = GenerateRooms(numRooms, 4, 10);
            int rmN = 1;
            byte testCnt = 1;
            foreach (Tuple<Tuple<int,int>,Tuple<int,int>> room in roomList)
                {
                rmN++;
                IEnumerable<int> xRange = Enumerable.Range(room.Item1.Item1, room.Item2.Item1 - room.Item1.Item1);
                IEnumerable<int> yRange = Enumerable.Range(room.Item1.Item2, room.Item2.Item2 - room.Item1.Item2);
                
                foreach (int x in xRange)
                    {
                    foreach(int y in yRange)
                        {
                        Tuple<int, int> rco = new Tuple<int, int>(x, y);
                        workMap[rco] = testCnt; // need routine to define tile types
                        }
                    }
                testCnt++;
                }
            return workMap;
            }

        private List<Tuple<Tuple<int, int>, Tuple<int, int>>> GenerateRooms(int numRms, int minSz, int maxSz)
            {
            List<Tuple<Tuple<int, int>, Tuple<int, int>>> rmLst = new List<Tuple<Tuple<int, int>, Tuple<int, int>>>();
            Tuple<int, int> anchorPos;
            Tuple<int, int> oppPos;
            for (int i = 0; i < numRms; i++)
                {
                int lim = 0;
                while (true)
                    {
                    // Generate upper left corner of room. From point 1 to mapsize - roomsize - 2
                    anchorPos = new Tuple<int, int>(random.Next(1, this.mapX - (maxSz + 2)), random.Next(1, this.mapY - (maxSz + 2)));
                    // Generate lower right corner of room. From 
                    oppPos = new Tuple<int, int>(anchorPos.Item1 + (random.Next(minSz, maxSz) - 1), anchorPos.Item2 + (random.Next(minSz, maxSz) - 1));
                    if (rmLst.Count == 0)
                        {
                        rmLst.Add(new Tuple<Tuple<int, int>, Tuple<int, int>>(anchorPos, oppPos));
                        break;
                        }
                    else
                        {
                        bool rmChk = true;
                        foreach (Tuple<Tuple<int, int>, Tuple<int, int>> rm1 in rmLst)
                            {
                            if (!mathFunctions.CheckRoomOverlap(anchorPos, oppPos, rm1.Item1, rm1.Item2))
                                {
                                rmChk = false;
                                break;
                                }
                            }
                        if (rmChk == true)
                            {
                            rmLst.Add(new Tuple<Tuple<int, int>, Tuple<int, int>>(anchorPos, oppPos));
                            break;
                            }
                        }
                    
                    if (lim == 20000) break;
                    lim++;
                    }
                }
            return rmLst;
            }

        

        public void PrintMap()
            {
            string output;
            for (int sub_y = 0; sub_y < this.mapY; sub_y++)
                {
                for (int sub_x = 0; sub_x < this.mapX; sub_x++)
                    {
                    //Position current_coord = new Position(sub_x, sub_y);
                    try
                        {
                        if (GetFromMap(sub_x, sub_y) == 0)
                            {
                            output = ".";
                            }
                        else output = "X";
                        Console.Write(output);
                        }
                    catch
                        {
                        Console.WriteLine("Failed on (" + sub_x + "," + sub_y + ")");
                        Environment.Exit(1);
                        }
                    }
                Console.Write('\n');
                }
            }
        }

    public static class mathFunctions
        {
        public static bool CheckRoomOverlap(Tuple<int, int> startPos1, Tuple<int, int> endPos1, Tuple<int, int> startPos2, Tuple<int, int> endPos2)
            {

            /* !!!!The start position needs to be set to the lower in the range.  if x1 = 5 and x2 = 7 you would expect a set of 5,6,7
             *  if x1= 7 and x2 = 5 you would still expect a set of 5,6,7.
             * in the range function the lower of the two x values should the the start position. 
             * **range(x1,|x1-x2| +1) if x1=5 and x2=7 , range(x2,|x2-x1| +1) if x1=7 and x2=5
             * 
             */
             // make start the lowest value and end the highest value.
            int sx1 = Math.Min(startPos1.Item1, endPos1.Item1);
            int ex1 = Math.Max(startPos1.Item1, endPos1.Item1);
            int sy1 = Math.Min(startPos1.Item2, endPos1.Item2);
            int ey1 = Math.Max(startPos1.Item2, endPos1.Item2);
            int sx2 = Math.Min(startPos2.Item1, endPos2.Item1);
            int ex2 = Math.Max(startPos2.Item1, endPos2.Item1);
            int sy2 = Math.Min(startPos2.Item2, endPos2.Item2);
            int ey2 = Math.Max(startPos2.Item2, endPos2.Item2);

            // Ranges of new room.
            IEnumerable<int> XRange1 = Enumerable.Range(sx1, Math.Abs(ex1 - sx1) + 1);
            IEnumerable<int> YRange1 = Enumerable.Range(sy1, Math.Abs(ey1 - sy1) + 1);
            // Ranges of reference room.
            IEnumerable<int> XRange2 = Enumerable.Range(sx2, Math.Abs(ex2 - sx2) + 1);
            IEnumerable<int> YRange2 = Enumerable.Range(sy2, Math.Abs(ey2 - sy2) + 1);


            // check for intersections. return -1 if empty
            var xinter = XRange1.Intersect(XRange2).DefaultIfEmpty();
            var yinter = YRange1.Intersect(YRange2).DefaultIfEmpty();
            

            if (xinter.ElementAt(0) == 0 | yinter.ElementAt(0) == 0)
                {
                return true;
                }
            else
                {
                return false;
                }
            }
        }
    }
