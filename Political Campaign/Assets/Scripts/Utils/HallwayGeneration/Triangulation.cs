using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangulation
{
    Vector2 spawnRoom;
    List<DTriangle> triangles;
    List<DEdge> edgeList;
    List<Vector2> roomNodes;
    List<GraphNode> graphNodes = new List<GraphNode>();
    List<GraphEdge> graphEdges = new List<GraphEdge>();
    IDictionary<int, GraphNode> mstGraphNodes = new Dictionary<int, GraphNode>();
    List<GraphNode> vistedGraphNodes = new List<GraphNode>();
    List<GraphEdge> graphEdgesToKeep = new List<GraphEdge>();
    
    private DTriangle createSuperTriangle(int xSize, int ySize) {
        // This assumes that the start point of the left corner is 0,0
        // Creates a right angle triangle that encompasses all other vertices
        return new DTriangle(new Vector2(0,0), new Vector2(0, ySize*2), new Vector2(xSize*2, 0));
    }

    public void triangulate(int xSize, int ySize, List<Vector2> roomNodes, Vector2 spawnRoom){
        triangles = new List<DTriangle>();
        DTriangle superTriangle = createSuperTriangle(xSize, ySize);
        triangles.Add(superTriangle);
        this.roomNodes = roomNodes;
        foreach(Vector2 roomCenter in this.roomNodes) {
            addVertex(roomCenter);
        }

        foreach (DTriangle triangle in triangles.ToArray()) {
            if ((SameCoord(triangle.getPoint1(), superTriangle.getPoint1()) || SameCoord(triangle.getPoint1(), superTriangle.getPoint2()) || SameCoord(triangle.getPoint1(), superTriangle.getPoint3()) ||
                  SameCoord(triangle.getPoint2(), superTriangle.getPoint1()) || SameCoord(triangle.getPoint2(), superTriangle.getPoint2()) || SameCoord(triangle.getPoint2(), superTriangle.getPoint3()) ||
                  SameCoord(triangle.getPoint3(), superTriangle.getPoint1()) || SameCoord(triangle.getPoint3(), superTriangle.getPoint2()) || SameCoord(triangle.getPoint3(), superTriangle.getPoint3()))) {
                    triangles.Remove(triangle);
            }
        }

        EstablishGraphNodes();
    }

    private bool SameCoord(Vector2 point1, Vector2 point2) {
        if (point1.x == point2.x && point1.y == point2.y) {
            return true;
        }
        return false;
    }

    private void EstablishGraphNodes() {

        foreach(Vector2 roomCenter in this.roomNodes) {
            graphNodes.Add(new GraphNode(roomCenter));
        }

        foreach(DTriangle triangle in triangles) {
            
            GraphNode node1 = FindGraphNode(triangle.getPoint1().x, triangle.getPoint1().y);
            GraphNode node2 = FindGraphNode(triangle.getPoint2().x, triangle.getPoint2().y);
            GraphNode node3 = FindGraphNode(triangle.getPoint3().x, triangle.getPoint3().y);

            GraphEdge edge1 = new GraphEdge(node1, node2);
            GraphEdge edge2 = new GraphEdge(node2, node3);
            GraphEdge edge3 = new GraphEdge(node3, node1);

            if (!CheckIfExists(edge1)) {
                graphEdges.Add(edge1);
            }
            if (!CheckIfExists(edge2)) {
                graphEdges.Add(edge2);
            }
            if (!CheckIfExists(edge3)) {
                graphEdges.Add(edge3);
            }
        }

        foreach(GraphEdge graphEdge in graphEdges) {
            graphEdge.AddConnections();
        }

        SetUpMST();
    }

    private bool CheckIfExists(GraphEdge graphEdge1) {
        foreach(GraphEdge graphEdge in graphEdges) {
            if ((graphEdge.GetNode1() == graphEdge1.GetNode1() && graphEdge.GetNode2() == graphEdge1.GetNode2()) || 
            (graphEdge.GetNode1() == graphEdge1.GetNode2() && graphEdge.GetNode2() == graphEdge1.GetNode1())) {
                return true;
            }
        }
        return false;
    }

    public List<GraphEdge> GetMSTEdges() {
        return graphEdgesToKeep;
    }

    private (GraphNode, GraphEdge) GetMinNode() {
        float weight = float.MaxValue;
        GraphEdge minGE = null;
        GraphNode minGN = null;
        foreach(GraphNode gn in vistedGraphNodes) {
            foreach(GraphEdge ge in gn.GetConnections()) {
                if (ge.GetDistance() < weight && !vistedGraphNodes.Contains(ge.GetOtherNode(gn))) {
                    weight = ge.GetDistance();
                    minGE = ge;
                    minGN = gn;
                }
            }
        }
        return (minGN, minGE);
    }

    private void SetUpMST() {
        GraphNode start = graphNodes[Random.Range(0, graphNodes.Count)];
        vistedGraphNodes.Add(start);
        while (vistedGraphNodes.Count != graphNodes.Count) {
            var minNodeValues = GetMinNode();
            GraphNode nextNode = minNodeValues.Item1;
            GraphEdge nextEdge = minNodeValues.Item2;
            if (nextEdge != null) {
                vistedGraphNodes.Add(nextEdge.GetOtherNode(nextNode));
                graphEdgesToKeep.Add(nextEdge);
            }
            
        }

        foreach(GraphEdge ge in graphEdges) {
            if (Random.Range(1, 101) > 92 && !graphEdgesToKeep.Contains(ge)) {
                graphEdgesToKeep.Add(ge);
            }
        }
    }

    private GraphNode FindGraphNode(float x, float y) {
        foreach(GraphNode graphNode in graphNodes) {
            if (graphNode.GetPosition().x == x && graphNode.GetPosition().y == y) {
                return graphNode;
            }
        }
        return null;
    }

    private void addVertex(Vector2 vert) {
        edgeList = new List<DEdge>();
        List<DTriangle> badTriangles = new List<DTriangle>();
        foreach (DTriangle triangle in triangles.ToArray()) {
            if (triangle.InCircumCirc(vert)) {
                badTriangles.Add(triangle);
            }
        }

        foreach(DTriangle triangle in badTriangles) {
            edgeList.Add(new DEdge(triangle.getPoint1(), triangle.getPoint2()));
            edgeList.Add(new DEdge(triangle.getPoint2(), triangle.getPoint3()));
            edgeList.Add(new DEdge(triangle.getPoint3(), triangle.getPoint1()));
            triangles.Remove(triangle);
        }

        edgeList = GetUniqueEdges(edgeList);

        foreach (DEdge edge in edgeList) {
            triangles.Add(new DTriangle(edge.GetPoint1(), edge.GetPoint2(), vert));
        }
    }

    public List<DEdge> GetUniqueEdges(List<DEdge> listOfEdges) {
        List<DEdge> uniqueEdges = new List<DEdge>();
        for (int i = 0; i < listOfEdges.Count; i++) {
            bool isUnique = true;
            for (int j = 0; j < listOfEdges.Count; j++) {
                if (i != j && listOfEdges[i].IsTheSame(listOfEdges[j])) {
                    isUnique = false;
                    break;
                }
            }

            if (isUnique) {
                uniqueEdges.Add(listOfEdges[i]);
            }
        }

        return uniqueEdges;
    }

    public List<DTriangle> GetTriangles() {
        return triangles;
    }

    public List<DEdge> GetEdges() {
        return edgeList;
    }

    public List<GraphNode> GetGraphNodes() {
        return graphNodes;
    }

    public List<GraphEdge> GetGraphEdges() {
        return graphEdges;
    }
}
