using System;
using System.Collections.Generic;


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
    private Dictionary<Tuple<int,int>, byte> mapGrid;
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
        Tuple<int,int> coords = new Tuple<int, int>(t1, t2);
        return mapGrid[coords];
        }

    private Dictionary<Tuple<int,int>, byte> GenMap(int Dim_X, int Dim_Y, int numRooms)
        {
            Dictionary<Tuple<int,int>, byte> workMap = new Dictionary<Tuple<int, int>, byte>();
            // init map
            for(int sub_x = 0; sub_x < Dim_X; sub_x++)
            {
                for(int sub_y = 0; sub_y < Dim_Y; sub_y++)
                {
                    Tuple<int,int> current_coord = new Tuple<int, int>(sub_x, sub_y);
                    workMap[current_coord] = 0;
                    Console.WriteLine("Wrote (" + sub_x + "," + sub_y + ") with byte " + workMap[current_coord]);
                }
            }
            this.rooms = numRooms;
            List<Tuple<Tuple<int,int>,Tuple<int,int>>> roomList;
            roomList = genRooms(8,4,10);
            return workMap;
        }

    private List<Tuple<Tuple<int, int>, Tuple<int, int>>> genRooms(int numRms, int minSz, int maxSz)
            {
                List<Tuple<Tuple<int, int>, Tuple<int, int>>> rmLst = new List<Tuple<Tuple<int, int>, Tuple<int, int>>>();
                Tuple<int, int> anchor;
                Tuple<int, int> roomSize;
            for (int i = 0; i < numRms; i++)
                {
                int lim = 0;
                while(true){
                    roomSize = new Tuple<int, int>(random.Next(minSz, maxSz), random.Next(minSz, maxSz));
                    anchor = new Tuple<int, int>(random.Next(1, this.dim_x - 2), random.Next(1, this.dim_y - 2));
                    if (checkOverlap(anchor, roomSize, rmLst))
                        {
                        rmLst.Add(anchor, roomSize);
                        break;
                        }
                    if (lim = 20) break;
                    lim++;
                    }               
                }
            return rmLst;
            }

    public bool checkOverlap(Tuple<int,int> anch, Tuple<int, int>size, List<Tuple<Tuple<int, int>, Tuple<int, int>>> rLst)
            {
            return true;
            }
    
    public void printMap()
        {
            
            for(int sub_y = 0; sub_y < this.dim_y; sub_y++)
            {
                for(int sub_x = 0; sub_x < this.dim_x; sub_x++)                
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