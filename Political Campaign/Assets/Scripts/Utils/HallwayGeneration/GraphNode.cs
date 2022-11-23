using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode
{
    Vector2 position;
    List<GraphEdge> connections = new List<GraphEdge>();
    public GraphNode(Vector2 position) {
        this.position = position;
    }

    public Vector2 GetPosition() {
        return position;
    }

    public List<GraphEdge> GetConnections() {
        return connections;
    }
    public void AddConnection(GraphEdge connection) {
        connections.Add(connection);
    }
}
