using System;
using System.Collections.Generic;
using System.Text;

namespace Graf_Tubes_Stima2
{
    public class BFS
    {
        public static List<string> FindPathBFS(List<string> nodes, bool[,] adjMtx, string firstEl, string target)
        {
            string[] prev = new string[nodes.Count];
            int[] dist = new int[nodes.Count];
            for (int i = 0; i < nodes.Count; i++)
            {
                dist[i] = -1;
                prev[i] = null;
            }

            if (!FindPathBFSUtil(nodes, adjMtx, firstEl, target, prev, dist))
            {
                Console.WriteLine("Tidak ada jalur koneksi yang tersedia.");
                Console.WriteLine("Anda harus memulai koneksi baru itu sendiri.");
            }

            List<string> path = new List<string>();
            string prevTarget = target;
            path.Add(prevTarget);

            while (prev[nodes.IndexOf(prevTarget)] != null)
            {
                path.Add(prev[nodes.IndexOf(prevTarget)]);
                prevTarget = prev[nodes.IndexOf(prevTarget)];
            }

            if (dist[nodes.IndexOf(target)] == 1)
            {
                Console.WriteLine("Friend");
            }
            else
            {
                Console.WriteLine((dist[nodes.IndexOf(target)] - 1) + " degree connection");
            }


            for (int i = path.Count - 1; i > 0; i--)
            {
                Console.Write(path[i] + " > ");
            }
            Console.Write(path[0]);

            return path;
        }

        public static bool FindPathBFSUtil(List<string> nodes, bool[,] adjMtx, string firstEl, string target, string[] prev, int[] dist)
        {
            List<string> queue = new List<string>();
            List<string> visited = new List<string>();

            queue.Add(firstEl);
            visited.Add(firstEl);
            dist[nodes.IndexOf(firstEl)] = 0;

            while (queue.Count != 0)
            {
                int elIndex = nodes.IndexOf(queue[0]);
                queue.RemoveAt(0);

                for (int i = 0; i < nodes.Count; i++)
                {
                    if (adjMtx[elIndex, i] == true && !visited.Contains(nodes[i]))
                    {
                        queue.Add(nodes[i]);
                        visited.Add(nodes[i]);
                        dist[i] = dist[elIndex] + 1;
                        prev[i] = nodes[elIndex];

                        if (nodes[i].Equals(target))
                            return true;
                    }
                }
            }
            return false;
        }

        public static Dictionary<string, List<string>> MutualFriendsBFS(List<string> nodes, bool[,] adjMtx, string firstEl)
        {
            Dictionary<string, List<string>> mutuals = new Dictionary<string, List<string>>();
            foreach (string node in nodes)
            {
                mutuals[node] = new List<string>();
            }
            MutualBFSUtil(nodes, adjMtx, firstEl, mutuals);

            for (int i = 0; i < nodes.Count; i++)
            {
                if (adjMtx[nodes.IndexOf(firstEl), i] == true)
                {
                    mutuals.Remove(nodes[i]);
                }
            }

            return mutuals;
        }

        private static void MutualBFSUtil(List<string> nodes, bool[,] adjMtx,
            string el, Dictionary<string, List<string>> mutuals)
        {
            int usrIdx = nodes.IndexOf(el);
            List<string> queue = new List<string>();
            List<string> visited = new List<string>();

            visited.Add(el);
            queue.Add(el);

            while (queue.Count != 0)
            {
                int elIndex = nodes.IndexOf(queue[0]);
                queue.RemoveAt(0);

                for (int i = 0; i < nodes.Count; i++)
                {
                    if (adjMtx[elIndex, i] == true && !visited.Contains(nodes[i]))
                    {
                        visited.Add(nodes[i]);
                        queue.Add(nodes[i]);
                        //CEK BUKAN USER DAN BUKAN DIRECT FRIEND
                        if (i != usrIdx && adjMtx[usrIdx, i] == false)
                        {
                            for (int j = 0; j < nodes.Count; j++)
                            {
                                if (adjMtx[usrIdx, j] == true && adjMtx[i, j] == true)
                                {
                                    // user berteman dengan j dan i berteman dengan j
                                    // -> nodes[j] mutual antara user dan el
                                    if (!mutuals[el].Contains(nodes[j]))
                                    {
                                        mutuals[el].Add(nodes[j]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
