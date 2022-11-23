using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSTNode
{
    Vector2 position;

    public MSTNode(MSTNode parent, float weight, Vector2 position) {
        this.position = position;
    }
}
