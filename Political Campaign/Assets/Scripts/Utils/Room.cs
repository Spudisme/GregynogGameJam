using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    enum Orientation {
        Up,
        Down,
        Left,
        Right
    }

    private int roomType;
    private int posX;
    private int posY;
    private int width;
    private int height;
    private List<Vector2> edges = new List<Vector2>();
    private List<Vector2> objectSpawnables = new List<Vector2>();
    private List<GameObject> spawnableObjects = new List<GameObject>();
    private List<GameObject> objectsInRoom = new List<GameObject>();
    private List<GameObject> enemiesInRoom = new List<GameObject>();
    private List<Block> roomAdditions = new List<Block>();
    private Vector2 doorEntrance;
    private Vector2 roomCenter;
    private Vector2 doorExit;

    public Room(int x, int y, int width, int height) {
        this.posX = x;
        this.posY = y;
        this.width = width;
        this.height = height;
        this.roomCenter = new Vector2(posX + width/2, posY + height/2);
        this.roomType = Random.Range(0,4);
        this.roomAdditions = GetRoomAdditions();
        SetEdge();
        objectSpawnables = GetSpawnablePoints();
    }

    public List<GameObject> GetObjectsInRoom() {
        return objectsInRoom;
    } 

    public List<GameObject> GetEnemiesInRoom() {
        return enemiesInRoom;
    }

    public void AddEnemy(GameObject enemy) {
        enemiesInRoom.Add(enemy);
    }

    public void AddSpawnableObject(GameObject obj) {
        spawnableObjects.Add(obj);
    }

    public List<GameObject> GetSpawnableObjects() {
        return spawnableObjects;
    }

    public List<Vector2> GetSpawnablePoints2() {
        return objectSpawnables;
    }
    private List<Vector2> GetSpawnablePoints() {
        List<Vector2> returnList = new List<Vector2>();
        for (int i = 1; i < width - 1; i++) {
            for (int j = 1; j < height - 1; j++) {
                bool blocked = false;
                foreach(Block block in roomAdditions) {
                    if (block.GetX() == i + posX && block.GetY() == j + posY) {
                        blocked = true;
                    }
                }
                if (!blocked) {
                    returnList.Add(new Vector2(i + posX, j + posY));
                }
            }
        }
        return returnList;
    }


    public void GenerateSpawnRoom() {
        roomAdditions = new List<Block>();
    }

    public List<Block> GetRoomAdditions() {
        List<Block> returnList = new List<Block>();
        if (roomType == 2 && width > 4 && height > 4) {
            for (int i = 3; i < width - 3; i++) {
                for (int j = 3; j < height - 3; j++) {
                    if (j == height - 4) {
                        returnList.Add(new Block(i + posX, j + posY, 7));
                    }
                    else {
                        returnList.Add(new Block(i + posX, j + posY, 8));
                    }
                }
            }
        }
        if (roomType == 3 && width > 4 && height > 4) {
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    if (((i == 3 && j == 3) || (i == width - 4 && j == 3) || (i == 3 && j == height - 4) || (i == width - 4 && j == height - 4))) {
                        returnList.Add(new Block(i + posX, j + posY, 2));
                    }
                }
            }
        }
        return returnList;
    }

    public List<Block> GetRoomTypeChanges() {
        return roomAdditions;
    }

    private void SetEdge() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if ((!(i == 0 && j == 0)
                && !(i == width - 1 && j == 0))) {
                    if (i == 0 || i == width - 1 || j == 0 || j == height - 1) {
                        edges.Add(new Vector2(i + posX, j + posY));
                    }
                }
            }
        }
    } 

    public List<Block> ListOfBlocks() {
        List<Block> returnList = new List<Block>();
        for (int i = 1; i < width - 1; i++) {
            for (int j = 1; j < height - 1; j++) {
                returnList.Add(new Block(i + posX, j + posY, 1));
            }
        }
        foreach(Vector2 edge in edges) {
            int tempBlockId;
            if (edge.x == posX) {
                tempBlockId = 3;
            } else if (edge.x == posX + width - 1) {
                tempBlockId = 4;
            } else if (edge.y == posY) {
                tempBlockId = 5;
            } else {
                tempBlockId = 2;
            }
            returnList.Add(new Block((int) edge.x,(int) edge.y, tempBlockId));
        }
        return returnList;
    }

    public Vector2 GetDoorEnt() {
        return doorEntrance;
    }

    public Vector2 GetDoorExt() {
        return doorExit;
    }
 
    public int GetX() {
        return posX;
    }
    public int GetY() {
        return posY;
    }
    public int GetWidth() {
        return width;
    }
    public int GetHeight() {
        return height;
    }
    public Vector2 GetCentre() {
        return roomCenter;
    }
    public List<Vector2> GetEdges() {
        return edges;
    }
}
