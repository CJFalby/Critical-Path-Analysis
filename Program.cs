using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;

namespace Project3
{
    class CriticalPathAnalysis
    {
        public static void Main()
        {
            //list of lists, each list is one path
            List<List<string>> allPaths = new List<List<string>>();
            allPaths.Add(new List<string> { "V1", "V2", "V5", "V7", "V9" });
            allPaths.Add(new List<string> { "V1", "V2", "V5", "V8", "V9" });
            allPaths.Add(new List<string> { "V1", "V3", "V5", "V7", "V9" });
            allPaths.Add(new List<string> { "V1", "V3", "V5", "V8", "V9" });
            allPaths.Add(new List<string> { "V1", "V4", "V6", "V8", "V9" });

            //create a dicitonary 
            Dictionary<string, int> connections = new Dictionary<string, int>();
            //add items to dictionary
            connections.Add("V1-V2", 6);
            connections.Add("V1-V3", 4);
            connections.Add("V1-V4", 5);
            connections.Add("V2-V5", 1);
            connections.Add("V3-V5", 1);
            connections.Add("V4-V6", 2);
            connections.Add("V5-V7", 9);
            connections.Add("V5-V8", 7);
            connections.Add("V6-V8", 4);
            connections.Add("V7-V9", 2);
            connections.Add("V8-V9", 4);

            List<int> allPathDurations = CriticalPathFind(allPaths, connections);

            int criticalCount = 0;
            int criticalCount2 = 0;
            string criticalPathString = "";
            string criticalPathString1 = "";
            foreach (List<string> path in allPaths)
            {
                foreach (string node in path)
                {
                    if (allPathDurations[criticalCount] == 18)
                    {
                        criticalPathString += node + " ";
                    }
                }
                if (criticalCount2 == 0)
                {
                    criticalPathString1 += criticalPathString;
                    criticalPathString = "";
                    criticalCount2++;
                }
                criticalCount++;
            }

            //critical paths
            Console.WriteLine("Critical Path String: " + criticalPathString1);
            Console.WriteLine("Critical Path String: " + criticalPathString);

            //earliestStartTime(allPaths, connections, allPathDurations);
            Dictionary<string, int> latestStartTime = new Dictionary<string, int>();
            Dictionary<string, int> slack = new Dictionary<string, int>();

        }
        public static void earliestStartTime(List<List<string>> allPaths, Dictionary<string, int> connections, List<int> allPathDurations)
        {
            Dictionary<string, int> earliestStartTime = new Dictionary<string, int>();
            int count = 1;
            int nodeCount = 2;

            foreach (KeyValuePair<string, int> kvp in connections)
            {
                //v1 will always be one as is the first node and represents the first day the project is started
                if ((kvp.Key).Contains("V1-"))
                {
                    earliestStartTime.Add("V1-V" + nodeCount, 1);
                    nodeCount++;
                }
            }
            foreach (KeyValuePair<string, int> kvp in connections)
            {
                earliestStartTime.Add("V" + kvp.Key[kvp.Key.Length - 1], kvp.Value);

            }
            //    if ((kvp.Key).Contains("-V2"))
            //    {
            //        int v2Node = kvp.Value + 1;
            //        earliestStartTime.Add("V2-V5", v2Node);

            //    }
            //}
            ////else if ((kvp.Key).Contains("-V2"))
            ////{
            ////    int v2Node = kvp.Value + 1;
            ////    earliestStartTime.Add("V2-V5", v2Node);
            ////}
            //else if((kvp.Key).Contains("-V3"))
            //{
            //    int v2Node = kvp.Value + 1;
            //    Console.Write(v2Node);
            //    earliestStartTime.Add("V" + count, v2Node);
            //}
        }

        //finds the durations of each path so that the critcal path can be determined
        public static List<int> CriticalPathFind(List<List<string>> allPaths, Dictionary<string, int> connections)
        {
            //all variables needed
            List<string> currentPath = new List<string>();
            List<int> allPathDurations = new List<int>();
            string prevNode = "";
            string currentNode = "";
            string connectionCheck1 = "";
            string connectionCheck2 = "";
            int count = 0;
            int connectionCount = 0;
            int duration = 0;

            //goes through each list in allpaths
            foreach (List<string> path in allPaths)
            {
                count = 0;
                duration = 0;
                currentPath.Clear();

                //goes through each node in a path
                foreach (string node in path)
                {
                    prevNode = "";
                    currentNode = "";

                    //if currentPath is empty add node
                    if (currentPath.Count == 0)
                    {
                        currentPath.Add(node);
                    }
                    //if currentPath isn't empty do this
                    else if (currentPath.Count >= 1)
                    {
                        prevNode = currentPath[count];
                        count++;
                        currentPath.Add(node);
                        currentNode = currentPath[count];

                        //goes through ecah value in dictionary connections
                        foreach (KeyValuePair<string, int> kvp in connections)
                        {
                            connectionCheck1 = "";
                            connectionCheck2 = "";
                            connectionCount = 0;
                            foreach (char c in kvp.Key)
                            {
                                if (connectionCount < 2)
                                {
                                    connectionCheck1 += c;
                                    //Console.WriteLine("Connection check1: " + connectionCheck1);
                                }
                                else if (connectionCount > 2)
                                {
                                    connectionCheck2 += c;
                                    //Console.WriteLine("Connection check2: " + connectionCheck2);
                                    if ((connectionCheck1 == prevNode) && (connectionCheck2 == currentNode))
                                    {
                                        duration += kvp.Value;
                                        //Console.WriteLine("duration: " + duration);
                                    }
                                }
                                connectionCount++;
                            }
                        }
                    }
                }
                allPathDurations.Add(duration);
            }
            foreach (int dur in allPathDurations)
            {
                Console.WriteLine(dur);
            }
            return allPathDurations;
        }
    }
}