using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    AStarNode parentNode;
    Vector2 position;
    float distance;
    float heuristic;
    float cost;

    public AStarNode(AStarNode parent, Vector2 position) {
        this.parentNode = parent;
        this.position = position;
        this.distance = 0;
        this.heuristic = 0;
        this.cost = 0;
    }

    public void SetHeuristic(Vector2 end) {
        heuristic = Vector2.Distance(position, end);
    }

    public float GetCost() {
        return cost;
    }
    public void SetCost(float newCost) {
        cost = newCost;
    }
    public float GetHeuristic() {
        return heuristic;
    }
    public void SetDistance(float newDistance) {
        distance = newDistance;
    }
    public float GetDistance() {
        return distance;
    }
    public AStarNode GetParent() {
        return parentNode;
    }
    public Vector2 getNodePosition() {
        return position;
    }
}
