﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCommon;

namespace A1
{
    public class Q2AddExitToMaze : Processor
    {
        public Q2AddExitToMaze(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long nodeCount, long[][] edges)
        {
            Graph = new Dictionary<long, Vertex>();
            for (long i = 1; i <= nodeCount; i++)
                Graph[i] = new Vertex(i);

            foreach (var e in edges) {
                Graph[e.First()].Adjs.Add(Graph[e.Last()]);
                Graph[e.Last()].Adjs.Add(Graph[e.First()]);
            }

            DFS();

            return CC - 1;
        }

        public Dictionary<long, Vertex> Graph;
        public long CC = 1;

        public void DFS()
        {
            foreach (var v in Graph.Values)
                v.Visited = false;

            CC = 1;
            foreach (var v in Graph.Values) {
                if (!v.Visited) {
                    Explore(v);
                    CC++;
                }
            }
        }

        public void Explore(Vertex v)
        {
            v.Visited = true;
            v.CCNum = CC;
            foreach (var w in v.Adjs)
                if (!w.Visited)
                    Explore(w);
        }

        public class Vertex
        {
            public bool Visited { get; set; }
            public long CCNum { get; set; }
            public long Key { get; private set; }
            public HashSet<Vertex> Adjs { get; private set; }
            public Vertex(long key, bool visited = false, long ccNum = -1)
            {
                this.Key = key;
                this.Visited = visited;
                this.CCNum = ccNum;
                Adjs = new HashSet<Vertex>();
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.Append($"{this.Key} (");
                sb.Append(String.Join(", ", Adjs.Select(a => a.Key.ToString())));
                sb.Append($") of {this.CCNum}");
                return sb.ToString();
            }
        }
    }
}
