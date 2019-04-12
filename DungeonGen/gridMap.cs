using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen
    {
    public class gridMap
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
        private Dictionary<Tuple<int, int>, byte> mapGrid;
        public int dim_x, dim_y, rooms;
        private Random random = new Random();

        public gridMap(int X, int Y, int rms)
            {
            this.dim_x = X;
            this.dim_y = Y;
            this.rooms = rms;
            mapGrid = GenMap(dim_x, dim_y, rooms);
            }

        public byte getFromMap(int t1, int t2)
            {
            Tuple<int, int> coords = new Tuple<int, int>(t1, t2);
            return mapGrid[coords];
            }

        private Dictionary<Tuple<int, int>, byte> GenMap(int Dim_X, int Dim_Y, int numRooms)
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
            roomList = genRooms(numRooms, 4, 10);
            int rmN = 1;
            foreach(Tuple<Tuple<int,int>,Tuple<int,int>> room in roomList)
                {
                Console.WriteLine("Room " + rmN + ": (" + room.Item1.Item1 + "," + room.Item1.Item2 + ")" + "," + "(" + room.Item2.Item1 + ", " + room.Item2.Item2 + ")");
                rmN++;
                IEnumerable<int> xRange = Enumerable.Range(room.Item1.Item1, room.Item2.Item1);
                IEnumerable<int> yRange = Enumerable.Range(room.Item1.Item2, room.Item2.Item2);
                foreach(int x in xRange)
                    {
                    foreach(int y in yRange)
                        {
                        Tuple<int, int> rco = new Tuple<int, int>(x, y);
                        workMap[rco] = 1;
                        }
                    }
                }
            return workMap;
            }

        private List<Tuple<Tuple<int, int>, Tuple<int, int>>> genRooms(int numRms, int minSz, int maxSz)
            {
            List<Tuple<Tuple<int, int>, Tuple<int, int>>> rmLst = new List<Tuple<Tuple<int, int>, Tuple<int, int>>>();
            Tuple<int, int> anchorPos;
            Tuple<int, int> oppPos;
            for (int i = 0; i < numRms; i++)
                {
                int lim = 0;
                while (true)
                    {
                    anchorPos = new Tuple<int, int>(random.Next(1, this.dim_x - (maxSz + 2)), random.Next(1, this.dim_y - (maxSz + 2)));
                    oppPos = new Tuple<int, int>(anchorPos.Item1 + (random.Next(minSz, maxSz) - 1), anchorPos.Item2 + (random.Next(minSz, maxSz) - 1));
                    if (checkOverlap(anchorPos, oppPos, rmLst) && (anchorPos.Item1 < oppPos.Item1) && (anchorPos.Item2 < oppPos.Item2))
                        {
                        rmLst.Add(new Tuple<Tuple<int, int>, Tuple<int, int>>(anchorPos, oppPos));
                        break;
                        }
                    if (lim == 20) break;
                    lim++;
                    }
                }
            return rmLst;
            }

        private bool checkOverlap(Tuple<int, int> anchorPos, Tuple<int, int> oppPos, List<Tuple<Tuple<int, int>, Tuple<int, int>>> rLst)
            {
       
            int cX1 = anchorPos.Item1;
            int cY1 = anchorPos.Item2;
            int cX2 = oppPos.Item1;
            int cY2 = oppPos.Item2;
            List<int> cXRange = new List<int>();
            List<int> cYRange = new List<int>();
            int refX1, refX2, refY1, refY2;

            for (int x = 0; x < (cX2 - cX1); x++)
                {
                cXRange.Add(x);
                }
            for (int y = 0; y < (cY2 - cY1); y++)
                {
                cXRange.Add(y);
                }

            foreach (Tuple<Tuple<int, int>, Tuple<int, int>> room in rLst)
                {
                refX1 = room.Item1.Item1;
                refY1 = room.Item1.Item2;
                refX2 = room.Item2.Item1;
                refY2 = room.Item2.Item2;
                List<int> refXRange = new List<int>();
                List<int> refYRange = new List<int>();
                for (int rx = 0; rx < (refX2 - refX1); rx++)
                    {
                    refXRange.Add(rx);
                    }
                for (int ry = 0; ry < (refY2 - refY1); ry++)
                    {
                    refYRange.Add(ry);
                    }
                if (cXRange.Any(x => refXRange.Any(y => y == x)) && cYRange.Any(x => refYRange.Any(y => y == x)))
                    {
                    return false;
                    }
                else
                    {
                    return true;
                    }

                }
            return true;
            }

        public void printMap()
            {

            for (int sub_y = 0; sub_y < this.dim_y; sub_y++)
                {
                for (int sub_x = 0; sub_x < this.dim_x; sub_x++)
                    {
                    //Position current_coord = new Position(sub_x, sub_y);
                    try
                        {
                        Console.Write(getFromMap(sub_x, sub_y));
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