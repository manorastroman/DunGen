using System;
using System.Collections.Generic;


namespace DungeonGen
{
public class gridMap
{

    
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
                for(int i = 0; i < numRms; i++)
                {
                Tuple<int, int> roomSize = new Tuple<int, int>(random.Next(minSz, maxSz), random.Next(minSz, maxSz)); 
                }
            return rmLst;
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