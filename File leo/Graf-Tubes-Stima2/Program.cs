using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graf_Tubes_Stima2
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

public class DFS
{
    public static List<string> TraversalDFS(List<string> nodes, bool[,] adjMtx, string firstEl)
    {
        List<string> visited = new List<string>();
        DFSUtil(nodes, visited, adjMtx, firstEl);

        return visited;
    }

    private static void DFSUtil(List<string> nodes, List<string> visited, bool[,] adjMtx, string el)
    {
        int elIndex = nodes.IndexOf(el);
        visited.Add(el);

        //CODE BEBAS DIBAWAH INI
        Console.WriteLine(el);
        //CODE BEBAS DIATAS INI
        for (int i = 0; i < nodes.Count; i++)
        {
            if (adjMtx[elIndex, i] == true && !visited.Contains(nodes[i]))
            {
                DFSUtil(nodes, visited, adjMtx, nodes[i]);
            }
        }
    }
    public static List<string> FindPathDFS(List<string> nodes, bool[,] adjMtx, string firstEl, string target)
    {
        List<string> visited = new List<string>();
        List<string> path = new List<string>();
        FindPathDFSUtil(nodes, visited, adjMtx, firstEl, path, target);

        path.Reverse();
        return path;
    }

    private static bool FindPathDFSUtil(List<string> nodes, List<string> visited,
        bool[,] adjMtx, string el, List<string> path, string target)
    {
        //CODE BEBAS DIBAWAH INI
        visited.Add(el);
        //Console.WriteLine(el);
        if (el.Equals(target))
        {
            path.Add(el);
            return true;
        }
        //CODE BEBAS DIATAS INI
        int elIndex = nodes.IndexOf(el);
        for (int i = 0; i < nodes.Count; i++)
        {
            if (adjMtx[elIndex, i] == true && !visited.Contains(nodes[i]))
            {
                if (FindPathDFSUtil(nodes, visited, adjMtx, nodes[i], path, target) == true)
                {
                    path.Add(el);
                    return true;
                }
            }
        }

        return false;
    }
    public static Dictionary<string, List<string>> MutualFriendsDFS(List<string> nodes, bool[,] adjMtx, string firstEl)
    {
        List<string> visited = new List<string>();
        Dictionary<string, List<string>> mutuals = new Dictionary<string, List<string>>();
        foreach (string node in nodes)
        {
            mutuals[node] = new List<string>();
        }
        MutualDFSUtil(nodes, visited, adjMtx, firstEl, mutuals, nodes.IndexOf(firstEl));

        for (int i = 0; i < nodes.Count; i++)
        {
            if (adjMtx[nodes.IndexOf(firstEl), i] == true)
            {
                mutuals.Remove(nodes[i]);
            }
        }

        return mutuals;
    }

    private static void MutualDFSUtil(List<string> nodes, List<string> visited, bool[,] adjMtx,
        string el, Dictionary<string, List<string>> mutuals, int usrIdx)
    {
        int elIndex = nodes.IndexOf(el);
        visited.Add(el);

        //CODE BEBAS DIBAWAH INI
        //CEK BUKAN USER DAN BUKAN DIRECT FRIEND
        if (elIndex != usrIdx && adjMtx[usrIdx, elIndex] == false)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (adjMtx[usrIdx, i] == true && adjMtx[elIndex, i] == true)
                {
                    //user berteman dengan i dan el berteman dengan i -> nodes[i] mutual
                    //antara user dan el
                    if (!mutuals[el].Contains(nodes[i]))
                    {
                        mutuals[el].Add(nodes[i]);
                    }
                }
            }
        }
        //CODE BEBAS DIATAS INI

        for (int i = 0; i < nodes.Count; i++)
        {
            if (adjMtx[elIndex, i] == true && !visited.Contains(nodes[i]))
            {
                MutualDFSUtil(nodes, visited, adjMtx, nodes[i], mutuals, usrIdx);
            }
        }
    }
}

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
