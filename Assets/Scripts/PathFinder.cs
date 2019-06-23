using System;
using System.Collections.Generic;

namespace PathFinding
{
    public static class PathFinder
    {
        public static List<Tile> FindPath_BFS(TileGrid grid, Tile start, Tile end, List<IVisualStep> outSteps)
        {
            // Visual stuff
            outSteps.Add(new MarkStartTileStep(start));
            outSteps.Add(new MarkEndTileStep(end));
            // ~Visual stuff

            HashSet<Tile> visited = new HashSet<Tile>();
            visited.Add(start);

            Queue<Tile> frontier = new Queue<Tile>();
            frontier.Enqueue(start);

            start.PrevTile = null;

            while (frontier.Count > 0)
            {
                Tile current = frontier.Dequeue();

                // Visual stuff
                if (current != start && current != end)
                {
                    outSteps.Add(new VisitTileStep(current));
                }
                // ~Visual stuff

                if (current == end)
                {
                    break;
                }

                foreach (var neighbor in grid.GetNeighbors(current))
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        frontier.Enqueue(neighbor);

                        neighbor.PrevTile = current;

                        // Visual stuff
                        if (neighbor != end)
                        {
                            outSteps.Add(new PushTileInFrontierStep(neighbor, 0));
                        }
                        // ~Visual stuff
                    }
                }
            }

            List<Tile> path = BacktrackToPath(end);

            // Visual stuff
            foreach (var tile in path)
            {
                if (tile == start || tile == end)
                {
                    continue;
                }

                outSteps.Add(new MarkPathTileStep(tile));
            }
            // ~Visual stuff

            return path;
        }

        public static List<Tile> FindPath_Dijkstra(TileGrid grid, Tile start, Tile end, List<IVisualStep> outSteps)
        {
            // Visual stuff
            outSteps.Add(new MarkStartTileStep(start));
            outSteps.Add(new MarkEndTileStep(end));
            // ~Visual stuff

            foreach (var tile in grid.Tiles)
            {
                tile.Cost = int.MaxValue;
            }

            start.Cost = 0;

            HashSet<Tile> visited = new HashSet<Tile>();
            visited.Add(start);

            MinHeap<Tile> frontier = new MinHeap<Tile>((lhs, rhs) => lhs.Cost.CompareTo(rhs.Cost));
            frontier.Add(start);

            start.PrevTile = null;

            while (frontier.Count > 0)
            {
                Tile current = frontier.Remove();
                
                // Visual stuff
                if (current != start && current != end)
                {
                    outSteps.Add(new VisitTileStep(current));
                }
                // ~Visual stuff

                if (current == end)
                {
                    break;
                }

                foreach (var neighbor in grid.GetNeighbors(current))
                {
                    int newNeighborCost = current.Cost + neighbor.Weight;
                    if (newNeighborCost < neighbor.Cost)
                    {
                        neighbor.Cost = newNeighborCost;
                        neighbor.PrevTile = current;
                    }

                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        frontier.Add(neighbor);

                        // Visual stuff
                        if (neighbor != end)
                        {
                            outSteps.Add(new PushTileInFrontierStep(neighbor, neighbor.Cost));
                        }
                        // ~Visual stuff
                    }
                }
            }

            List<Tile> path = BacktrackToPath(end);

            // Visual stuff
            foreach (var tile in path)
            {
                if (tile == start || tile == end)
                {
                    continue;
                }

                outSteps.Add(new MarkPathTileStep(tile));
            }
            // ~Visual stuff

            return path;
        }

        public static List<Tile> FindPath_AStar(TileGrid grid, Tile start, Tile end, List<IVisualStep> outSteps)
        {
            // Visual stuff
            outSteps.Add(new MarkStartTileStep(start));
            outSteps.Add(new MarkEndTileStep(end));
            // ~Visual stuff

            foreach (var tile in grid.Tiles)
            {
                tile.Cost = int.MaxValue;
            }

            start.Cost = 0;

            Comparison<Tile> heuristicComparison = (lhs, rhs) =>
            {
                float lhsCost = lhs.Cost + GetEuclideanHeuristicCost(lhs, end);
                float rhsCost = rhs.Cost + GetEuclideanHeuristicCost(rhs, end);

                return lhsCost.CompareTo(rhsCost);
            };

            MinHeap<Tile> frontier = new MinHeap<Tile>(heuristicComparison);
            frontier.Add(start);

            HashSet<Tile> visited = new HashSet<Tile>();
            visited.Add(start);

            start.PrevTile = null;

            while (frontier.Count > 0)
            {
                Tile current = frontier.Remove();

                // Visual stuff
                if (current != start && current != end)
                {
                    outSteps.Add(new VisitTileStep(current));
                }
                // ~Visual stuff

                if (current == end)
                {
                    break;
                }

                foreach (var neighbor in grid.GetNeighbors(current))
                {
                    int newNeighborCost = current.Cost + neighbor.Weight;
                    if (newNeighborCost < neighbor.Cost)
                    {
                        neighbor.Cost = newNeighborCost;
                        neighbor.PrevTile = current;
                    }

                    if (!visited.Contains(neighbor))
                    {
                        frontier.Add(neighbor);
                        visited.Add(neighbor);

                        // Visual stuff
                        if (neighbor != end)
                        {
                            outSteps.Add(new PushTileInFrontierStep(neighbor, neighbor.Cost));
                        }
                        // ~Visual stuff
                    }
                }
            }

            List<Tile> path = BacktrackToPath(end);

            // Visual stuff
            foreach (var tile in path)
            {
                if (tile == start || tile == end)
                {
                    continue;
                }

                outSteps.Add(new MarkPathTileStep(tile));
            }
            // ~Visual stuff

            return path;
        }

        public static List<Tile> FindPath_GreedyBestFirstSearch(TileGrid grid, Tile start, Tile end, List<IVisualStep> outSteps)
        {
            // Visual stuff
            outSteps.Add(new MarkStartTileStep(start));
            outSteps.Add(new MarkEndTileStep(end));
            // ~Visual stuff

            Comparison<Tile> heuristicComparison = (lhs, rhs) =>
            {
                float lhsCost = GetEuclideanHeuristicCost(lhs, end);
                float rhsCost = GetEuclideanHeuristicCost(rhs, end);

                return lhsCost.CompareTo(rhsCost);
            };

            MinHeap<Tile> frontier = new MinHeap<Tile>(heuristicComparison);
            frontier.Add(start);

            HashSet<Tile> visited = new HashSet<Tile>();
            visited.Add(start);

            start.PrevTile = null;

            while (frontier.Count > 0)
            {
                Tile current = frontier.Remove();

                // Visual stuff
                if (current != start && current != end)
                {
                    outSteps.Add(new VisitTileStep(current));
                }
                // ~Visual stuff

                if (current == end)
                {
                    break;
                }

                foreach (var neighbor in grid.GetNeighbors(current))
                {
                    if (!visited.Contains(neighbor))
                    {
                        frontier.Add(neighbor);
                        visited.Add(neighbor);
                        neighbor.PrevTile = current;

                        // Visual stuff
                        if (neighbor != end)
                        {
                            outSteps.Add(new PushTileInFrontierStep(neighbor, 0));
                        }
                        // ~Visual stuff
                    }
                }
            }

            List<Tile> path = BacktrackToPath(end);

            // Visual stuff
            foreach (var tile in path)
            {
                if (tile == start || tile == end)
                {
                    continue;
                }

                outSteps.Add(new MarkPathTileStep(tile));
            }
            // ~Visual stuff

            return path;
        }

        private static float GetEuclideanHeuristicCost(Tile current, Tile end)
        {
            float heuristicCost = (current.ToVector2() - end.ToVector2()).magnitude;
            return heuristicCost;
        }

        private static List<Tile> BacktrackToPath(Tile end)
        {
            Tile current = end;
            List<Tile> path = new List<Tile>();

            while (current != null)
            {
                path.Add(current);
                current = current.PrevTile;
            }

            path.Reverse();

            return path;
        }
    }

    public interface IVisualStep
    {
        void Execute();
    }

    public abstract class VisualStep : IVisualStep
    {
        protected Tile _tile;

        public VisualStep(Tile tile)
        {
            _tile = tile;
        }

        public abstract void Execute();
    }

    public class MarkStartTileStep : VisualStep
    {
        public MarkStartTileStep(Tile tile) : base(tile)
        {
        }

        public override void Execute()
        {
            _tile.SetColor(_tile.Grid.TileColor_Start);
        }
    }

    public class MarkEndTileStep : VisualStep
    {
        public MarkEndTileStep(Tile tile) : base(tile)
        {
        }

        public override void Execute()
        {
            _tile.SetColor(_tile.Grid.TileColor_End);
        }
    }

    public class MarkPathTileStep : VisualStep
    {
        public MarkPathTileStep(Tile tile) : base(tile)
        {
        }

        public override void Execute()
        {
            _tile.SetColor(_tile.Grid.TileColor_Path);
        }
    }

    public class PushTileInFrontierStep : VisualStep
    {
        private int _cost;

        public PushTileInFrontierStep(Tile tile, int cost) : base(tile)
        {
            _cost = cost;
        }

        public override void Execute()
        {
            _tile.SetColor(_tile.Grid.TileColor_Frontier);
            _tile.SetText(_cost != 0 ? _cost.ToString() : "");
        }
    }

    public class VisitTileStep : VisualStep
    {
        public VisitTileStep(Tile tile) : base(tile)
        {
        }

        public override void Execute()
        {
            _tile.SetColor(_tile.Grid.TileColor_Visited);
        }
    }
}
