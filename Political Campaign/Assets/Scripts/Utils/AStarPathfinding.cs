using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding
{ 
    private static List<Vector2> returnPath(AStarNode currentNode) {
        List<Vector2> path = new List<Vector2>();
        AStarNode current = currentNode;
        while (current != null) {
                path.Add(current.getNodePosition());
                current = current.GetParent();
            }
        path.Reverse();
        return path;
    }
    public static List<Vector2> startPathFiding(Vector2 start, Vector2 end, string type, Room room, int[,] map) {
        int maxIterations = 1000;
        if (type == "Enemy") {
            maxIterations = 100;
        }
        int iterations = 0;
        List<AStarNode> openList = new List<AStarNode>();
        List<AStarNode> closedList = new List<AStarNode>();
        start = new Vector2(Mathf.RoundToInt(start.x),Mathf.RoundToInt(start.y));
        end = new Vector2(Mathf.RoundToInt(end.x),Mathf.RoundToInt(end.y));
        AStarNode starterNode = new AStarNode(null, start);
        openList.Add(starterNode);

        while (openList.Count > 0) {
            iterations++;
            AStarNode currentNode = openList[0];
            int currentIndex = 0;

            if (iterations > maxIterations) {
                return returnPath(currentNode);
            }

            foreach(AStarNode node in openList) {
                if (node.GetCost() < currentNode.GetCost()) {
                    currentNode = node;
                    currentIndex = openList.IndexOf(node);
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.getNodePosition().Equals(end)) {
                return returnPath(currentNode);
            }

            List<AStarNode> children = new List<AStarNode>();
            Vector2[] listOfNeighbours = new Vector2[]{new Vector2(1,0), new Vector2(0,1), new Vector2(-1,0), new Vector2(0,-1)};
            if (type == "Enemy") {
                listOfNeighbours = new Vector2[]{new Vector2(1,0), new Vector2(0,1), new Vector2(-1,0), new Vector2(0,-1)
                ,new Vector2(-1,1), new Vector2(1,1), new Vector2(1,-1), new Vector2(-1,-1)};
            }
            bool continue_while = false;
            foreach (Vector2 childrenPos in listOfNeighbours) {
                
                Vector2 nodePosition = currentNode.getNodePosition() + childrenPos;
                if(type.Equals("Enemy")) {
                    foreach(GameObject Obs in room.GetObjectsInRoom()) {
                        if((Vector2) Obs.transform.position == nodePosition) {
                            continue_while = true;
                            break;
                        }
                    }
                    if (map[Mathf.RoundToInt(nodePosition.x), Mathf.RoundToInt(nodePosition.y)] != 1) {
                        continue_while =  true;
                    }
                    if(continue_while) {
                        continue;
                    }
                    continue_while = false;
                }
                AStarNode newNode = new AStarNode(currentNode, nodePosition);
                children.Add(newNode);
            }

            
            foreach(AStarNode child in children) {
                foreach(AStarNode closeChild in closedList) {
                    if (child == closeChild) {
                        continue_while = true;
                        break;
                    }
                }
                if(continue_while) {
                    continue;
                }
                continue_while = false;

                child.SetDistance(currentNode.GetDistance() + 1);
                child.SetHeuristic(end);
                child.SetCost(child.GetDistance() + child.GetHeuristic());

                foreach(AStarNode openChild in openList) {
                    if(openChild.getNodePosition().y == child.getNodePosition().y && openChild.getNodePosition().x == child.getNodePosition().x && child.GetDistance() >= openChild.GetDistance()) {
                        continue_while = true;
                        break;
                    }
                }
                if(continue_while) {
                    continue;
                }
                openList.Add(child);
            }
        }
        return new List<Vector2>();
    }
}