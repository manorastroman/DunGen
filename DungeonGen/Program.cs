using System;

namespace DungeonGen
{
    class Program
    {
        static void Main(string[] args)
        {
            gridMap map = new gridMap(80, 60, 4);
            map.printMap();
        }
    }
}
