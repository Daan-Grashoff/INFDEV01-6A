using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using EntryPoint;

namespace EntryPoint
{
#if WINDOWS || LINUX
    public static class Program
    {
        static void CrashMe(int n)
        {
            Console.Write("Going strong at level " + n + "\r                       ");
            // CrashMe(n + 1);
        }

        [STAThread]
        static void Main()
        {
            var fullscreen = false;

            //Console.WriteLine("Which assignment shall run next? (1, 2, 3, 4, or q for quit) Choose assignment:");

            switch ("3")
            {
                case "1":
                    var game1 = VirtualCity.RunAssignment1(SortSpecialBuildingsByDistance, fullscreen);
                    game1.Run();
                    break;
                case "2":
                    var game2 = VirtualCity.RunAssignment2(FindSpecialBuildingsWithinDistanceFromHouse, fullscreen);
                    game2.Run();
                    break;
                case "3":
                    var game3 = VirtualCity.RunAssignment3(FindRoute, fullscreen);
                    game3.Run();
                    break;
                case "4":
                    var game4 = VirtualCity.RunAssignment4(FindRoutesToAll, fullscreen);
                    game4.Run();
                    break;
                case "q":
                    return;
                default:
                    var game12 = VirtualCity.RunAssignment1(SortSpecialBuildingsByDistance, fullscreen);
                    game12.Run();
                    break;
            }

        }

        private static IEnumerable<Vector2> SortSpecialBuildingsByDistance(Vector2 house, IEnumerable<Vector2> specialBuildings)
        {
            var SpecialBuildings = specialBuildings.ToArray();
            Sorting.MergeSortByHouse(SpecialBuildings, 0, SpecialBuildings.Length - 1, house);

            Console.WriteLine("Test");
            return SpecialBuildings;
        }

        private static IEnumerable<IEnumerable<Vector2>> FindSpecialBuildingsWithinDistanceFromHouse(
          IEnumerable<Vector2> specialBuildings,
          IEnumerable<Tuple<Vector2, float>> housesAndDistances)
        {
            var Specialbuildings = specialBuildings.ToArray();

            var kdTree = Sorting.BuildKdTree(Specialbuildings, 0);
            //var kdTree2 = Sorting.BuildKdTree2(Specialbuildings, 0, Specialbuildings.Length - 1, 0);

            Sorting.TreeWalker(kdTree);
            var houses = new List<Vector2>();
            foreach (var house in housesAndDistances)
            {
                Sorting.TreeRange(kdTree, house, 0, houses);
            }

            var list = new List<List<Vector2>>();
            list.Add(houses);
            return list;
        }

        class vertice
        {
            public bool Visited { get; set; }
            public float Distance { get; set; }
        }


        private static IEnumerable<Tuple<Vector2, Vector2>> FindRoute(Vector2 startingBuilding,
          Vector2 destinationBuilding, IEnumerable<Tuple<Vector2, Vector2>> roads)
        {
            var adjList = roads.GroupBy(road => road.Item1).ToDictionary(group => group.Key, group => group.Select(t => t.Item2));
            var NodeDistance = roads.GroupBy(road => road.Item1).ToDictionary(group => group.Key, group => new vertice { Visited = false, Distance = float.PositiveInfinity });






            var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
            List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
            var prevRoad = startingRoad;
            for (int i = 0; i < 300; i++)
            {
                prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, destinationBuilding)).First());
                fakeBestPath.Add(prevRoad);
            }


            return fakeBestPath;
        }

        private static IEnumerable<IEnumerable<Tuple<Vector2, Vector2>>> FindRoutesToAll(Vector2 startingBuilding,
          IEnumerable<Vector2> destinationBuildings, IEnumerable<Tuple<Vector2, Vector2>> roads)
        {
            List<List<Tuple<Vector2, Vector2>>> result = new List<List<Tuple<Vector2, Vector2>>>();
            foreach (var d in destinationBuildings)
            {
                var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
                List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
                var prevRoad = startingRoad;
                for (int i = 0; i < 30; i++)
                {
                    prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, d)).First());
                    fakeBestPath.Add(prevRoad);
                }


                result.Add(fakeBestPath);
            }
            return result;
        }
    }
#endif
}
