using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp2
{
    public class BacaFile
    {
        public static bool[,] getAdjMatrix(string[] file, List<string> nodes)
        {
            bool[,] adjMatrix = new bool[nodes.Count, nodes.Count];

            for (int i = 1; i < file.Length; i++)
            {
                string[] split = file[i].Split(" ");
                adjMatrix[nodes.IndexOf(split[0]), nodes.IndexOf(split[1])] = true;
                adjMatrix[nodes.IndexOf(split[1]), nodes.IndexOf(split[0])] = true;
            }

            return adjMatrix;
        }
        public static List<string> getNodes(string[] file)
        {
            List<string> nodes = new List<string>();
            //line 0 isinya integer
            for (int i = 1; i < file.Length; i++)
            {
                string[] split = file[i].Split(" ");
                if (!nodes.Contains(split[0]))
                {
                    nodes.Add(split[0]);
                }
                if (!nodes.Contains(split[1]))
                {
                    nodes.Add(split[1]);
                }
            }
            return nodes;
        }
    }
}
