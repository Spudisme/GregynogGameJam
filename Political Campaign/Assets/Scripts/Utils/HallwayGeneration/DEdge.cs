using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEdge 
{
    Vector2 point1;
    Vector2 point2;
    float distance;

    public DEdge(Vector2 point1, Vector2 point2) {
        this.point1 = point1;
        this.point2 = point2;
        distance = Vector2.Distance(point1, point2);
    }

    public bool IsTheSame(DEdge edge)
    {
        return (this.point1.Equals(edge.point1) && this.point2.Equals(edge.point2) 
        || (this.point1.Equals(edge.point2) && this.point2.Equals(edge.point1)));
    }

    public Vector2 GetPoint1() {
        return point1;
    }

    public Vector2 GetPoint2() {
        return point2;
    }

    public float GetDistance() {
        return distance;
    }
}

