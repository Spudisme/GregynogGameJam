using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    int posX;
    int posY;
    int blockID;
    public Block(int x, int y, int blockID) {
        this.posX = x;
        this.posY = y;
        this.blockID = blockID;
    }

    public int GetBlockType() {
        return blockID;
    }
    public int GetX() {
        return posX;
    }
    public int GetY() {
        return posY;
    }
}
