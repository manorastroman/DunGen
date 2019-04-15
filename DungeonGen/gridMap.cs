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
                        workMap[rco] = testCnt;
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
                    if (CheckRoomOverlap(anchorPos, oppPos, rmLst) && (anchorPos.Item1 < oppPos.Item1) && (anchorPos.Item2 < oppPos.Item2))
                        {
                        rmLst.Add(new Tuple<Tuple<int, int>, Tuple<int, int>>(anchorPos, oppPos));
                        break;
                        }
                    if (lim == 20000) break;
                    lim++;
                    }
                }
            return rmLst;
            }

        private bool CheckRoomOverlap(Tuple<int, int> anchorPos, Tuple<int, int> oppPos, List<Tuple<Tuple<int, int>, Tuple<int, int>>> rLst)
            {

            int cX1 = anchorPos.Item1;
            int cY1 = anchorPos.Item2;
            int cX2 = oppPos.Item1;
            int cY2 = oppPos.Item2;


            IEnumerable<int> cXRange = Enumerable.Range(cX1, (cX2 - cX1));
            IEnumerable<int> cYRange = Enumerable.Range(cY1, (cY2 - cY1));

            if (rLst.Count == 0)
                {
                return true;
                }
            else
                { 
            foreach (Tuple<Tuple<int, int>, Tuple<int, int>> room in rLst)
                {
                int refX1 = room.Item1.Item1;
                int refY1 = room.Item1.Item2;
                int refX2 = room.Item2.Item1;
                int refY2 = room.Item2.Item2;
                IEnumerable<int> refXRange = Enumerable.Range(refX1 - 1, (refX2 - refX1) + 1);
                IEnumerable<int> refYRange = Enumerable.Range(refY1 - 1, (refY2 - refY1) + 1);
                var xinter = cXRange.Intersect(refXRange).DefaultIfEmpty();
                var yinter = cYRange.Intersect(refYRange).DefaultIfEmpty();

                if(xinter.ElementAt(0) == 0 && yinter.ElementAt(0) == 0)
                    {
                    
                    }
                else
                    {
                    return false;
                    }

                } }
            return true;
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


    }