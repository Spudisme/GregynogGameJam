using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphEdge
{
    float distance;
    GraphNode node1;
    GraphNode node2;

    public GraphEdge(GraphNode node1, GraphNode node2) {
        this.node1 = node1;
        this.node2 = node2;
        this.distance = Vector2.Distance(node1.GetPosition(), node2.GetPosition());
    }

    public bool IsTheSame(GraphEdge edge)
    {
        return (this.node1.GetPosition().Equals(edge.GetNode1().GetPosition()) && this.node2.GetPosition().Equals(edge.GetNode2().GetPosition()) 
            || (this.node1.GetPosition().Equals(edge.GetNode2().GetPosition()) && this.node2.GetPosition().Equals(edge.GetNode1().GetPosition())));
    }

    public GraphNode GetNode1() {
        return node1;
    }

     public GraphNode GetNode2() {
        return node2;
    }

    public float GetDistance() {
        return distance;
    }

    public GraphNode GetOtherNode(GraphNode node) {
        if (node == node1) { 
            return node2;
        } else if (node == node2) {
            return node1;
        }
        return null;
    }

    public void AddConnections() {
        node1.AddConnection(this);
        node2.AddConnection(this);
    }
}
