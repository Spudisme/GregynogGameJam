using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf
{
    private const int MIN_LEAF = 13;
    private int y;
    private int x;
    private int width;
    private int height;
    private Leaf leftChild;
    private Leaf rightChild;
    private Room room;

    public Leaf(int x, int y, int width, int height) {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    public bool SplitLeaf() {
        if (leftChild != null || rightChild != null) {
            return false;
        }

        bool splitH = Random.Range(1,101) > 50;
        if (width > height && width / height >= 1.25) {
            splitH = false;
        } else if (height > width && height / width >= 1.25) {
            splitH = true;
        }

        int max = (splitH ? height : width) - MIN_LEAF;
        if (max <= MIN_LEAF) {
            return false;
        }

        int split = Random.Range(MIN_LEAF, max);
        if (splitH) {
            leftChild = new Leaf(x, y, width, split);
            rightChild = new Leaf(x, y + split, width, height - split);
        }
        else {
            leftChild = new Leaf(x, y, split, height);
            rightChild = new Leaf(x + split, y, width  - split, height);
        }
        return true;
    }

    public void GenerateRoom() {
        if (leftChild != null || rightChild != null) {
            if (leftChild != null) {
                leftChild.GenerateRoom();
            }
            if (rightChild != null) {
                rightChild.GenerateRoom();
            }
        } else {
            Vector2 roomSize = new Vector2(Random.Range(8, width - 2), Random.Range(6, height - 2) );
            Vector2 roomPos = new Vector2(Random.Range(1, width - roomSize.x - 1), Random.Range(1, height - roomSize.y - 1));
            room = new Room(x + (int) roomPos.x, y +  (int) roomPos.y, (int) roomSize.x, (int) roomSize.y);
            DungeonMapGenerator.roomsOnMap.Add(room);
        }
    }

    public Leaf GetLeftChild() {
        return leftChild;
    }

    public Leaf GetRightChild() {
        return rightChild;
    }
    public int GetWidth() {
        return width;
    }
    public int GetHeight() {
        return height;
    }
    public int GetX() {
        return x;
    }
    public int GetY() {
        return y;
    }

    public Room GetRoom() {
        return room;
    }
}
