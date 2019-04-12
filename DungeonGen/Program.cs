using System;

namespace DungeonGen
{
    class Program
    {
        static void Main(string[] args)
        {
            gridMap map = new gridMap(30, 40, 4);
            map.printMap();
        }
    }
}
