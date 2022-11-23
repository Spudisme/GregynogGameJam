using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTriangle
{
    Vector2 point1;
    Vector2 point2;
    Vector2 point3;
    DEdge oneToTwo;
    DEdge twoToThree;
    DEdge threeToFour;
    Vector2 circumCenter;
    float radius;

    public DTriangle(Vector2 v1, Vector2 v2, Vector2 v3) {
        this.point1 = v1;
        this.point2 = v2;
        this.point3 = v3;
        this.circumCenter = CalculateCircleCenter();
        this.radius = workOutRadius();
        this.oneToTwo = new DEdge(point1, point2);
        this.twoToThree = new DEdge(point2, point3);
        this.threeToFour = new DEdge(point3, point1);
        
        /* Mathf.Sqrt(((((point1.x * point1.x) - (point3.x * point3.x)) + ((point1.y * point1.y) - (point3.y * point3.y))) * //
                                    ((point2.x * point1.x) - (point3.x * point3.x)) + ((point2.y * point2.y) - (point3.y * point3.y)) *
                                    ((point1.x * point1.x) - (point2.x * point2.x)) + ((point1.y * point1.y) - (point2.y * point2.y))) /
                                    (point1.x * (point2.y - point3.y)) + (point2.x * (point3.y - point1.y)) + (point3.x * (point1.y - point2.y))); */
    }

    private Vector2 CalculateCircleCenter() {
        float d = 2 * (point1.x * (point2.y - point3.y) + point2.x * (point3.y - point1.y) + point3.x * (point1.y - point2.y));
        float ux = (((point1.x * point1.x) + (point1.y * point1.y)) * (point2.y - point3.y) + 
        ((point2.x * point2.x) + (point2.y * point2.y)) * (point3.y - point1.y) + 
        ((point3.x * point3.x) + (point3.y * point3.y)) * (point1.y - point2.y)) / d;
        float uy = (((point1.x * point1.x) + (point1.y * point1.y)) * (point3.x - point2.x) + 
        ((point2.x * point2.x) + (point2.y * point2.y)) * (point1.x - point3.x) + 
        ((point3.x * point3.x) + (point3.y * point3.y)) * (point2.x - point1.x)) / d;
        return new Vector2(ux, uy);
    }

    private float workOutRadius() {
        float a = GetLength(point1, point2);
        float b = GetLength(point2, point3);
        float c = GetLength(point3, point1);
        float s = (a + b + c) / 2;
        float area = Mathf.Sqrt(s*(s - a) * (s - b) * (s - c));
        return (a * b * c) / (4 * area);
    }

    private float GetLength(Vector2 point1, Vector2 point2) {
        return Vector2.Distance(point1, point2);
    }

    public Vector2 GetCircumCenter() {
        return circumCenter;
    }

    public bool InCircumCirc(Vector2 vector) {
        return Vector2.Distance(circumCenter, vector) <= radius;
    }

    public List<DEdge> GetEdges() {
        return new List<DEdge>{oneToTwo, twoToThree, threeToFour};
    }

    public float GetRadius() {
        return radius;
    }
    public Vector2 getPoint1() {
        return point1;
    }
    public Vector2 getPoint2() {
        return point2;
    }
    public Vector2 getPoint3() {
        return point3;
    }
}
