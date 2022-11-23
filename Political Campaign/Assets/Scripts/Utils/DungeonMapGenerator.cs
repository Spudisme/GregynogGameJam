using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMapGenerator : MonoBehaviour
{
    public int xSize = 10;
    public static List<Room> roomsOnMap = new List<Room>();
    public int ySize = 10;
    int[,] map;
    
    public GameObject blockFloor;
    public GameObject blockWall;
    public GameObject blockWallOtherSide;
    public GameObject blockWallOnFloor;
    public GameObject blockWallOnFloorOnFloor;
    public GameObject spawnSurround;
    public GameObject player;
    public GameObject crate;
    public GameObject rock;
    public GameObject ratling1;
    private const int MAX_LEAFS = 6;
    private Room spawnRoom;
    private List<Leaf> leafs = new List<Leaf>();
    private Triangulation nodeMap = new Triangulation();
    private List<DEdge> MSTnodes;
    private List<Block> listOfBlocksOnMap = new List<Block>();
    private List<GameObject> ListOfRoomObjects = new List<GameObject>();
    void Start()
    {
        map = new int[xSize,ySize];
        Leaf rootLeaf = new Leaf(0, 0, xSize, ySize);
        leafs.Add(rootLeaf);
        bool split = true;
        while (split) {
            split = false;
            foreach(Leaf l in leafs.ToArray()) {
                if (l.GetLeftChild() == null && l.GetRightChild() == null) {
                    if(l.GetWidth() > MAX_LEAFS || l.GetHeight() > MAX_LEAFS || Random.Range(1,101) > 25 ) {
                        if (l.SplitLeaf()) {
                            leafs.Add(l.GetLeftChild());
                            leafs.Add(l.GetRightChild());
                            split = true;
                        }
                    }
                }
            }
        }
        rootLeaf.GenerateRoom();
        SetSpawnRoom();
        spawnRoom.GenerateSpawnRoom();
        Instantiate(spawnSurround, new Vector3(spawnRoom.GetCentre().x, spawnRoom.GetCentre().y, -.1f), Quaternion.identity);

        List<Vector2> roomNodes = new List<Vector2>();
        foreach(Room room in roomsOnMap) {
            roomNodes.Add(room.GetCentre());
            room.AddSpawnableObject(crate);
            room.AddSpawnableObject(rock);
            room.AddSpawnableObject(ratling1);
            foreach(GameObject gobject in room.GetSpawnableObjects()) {
                int amount = ((room.GetWidth() / 4) * (room.GetHeight() / 4));
                for (int i = 0; i < amount; i++) {
                    Vector2 spawnSpot = room.GetSpawnablePoints2()[Random.Range(0,room.GetSpawnablePoints2().Count)];
                    room.GetSpawnablePoints2().Remove(spawnSpot);
                    GameObject createdObject = Instantiate(gobject, new Vector3(spawnSpot.x, spawnSpot.y, -0.5f), Quaternion.identity);
                    if (gobject.layer == LayerMask.NameToLayer("Enemy")) {
                        room.AddEnemy(createdObject);
                        createdObject.GetComponent<EnemyMovement>().init(this,map,room);
                        createdObject.GetComponent<EnemyHealth>().init(room);
                    } else {
                        room.GetObjectsInRoom().Add(createdObject);
                        createdObject.GetComponent<Obstical>().SetRoom(room);
                    }
                    
                }
            }
            
        }
        
        nodeMap.triangulate(xSize, ySize, roomNodes, spawnRoom.GetCentre());

        listOfBlocksOnMap = GetAllBlocksOnLevel();
        ApplyGrid();

        GenerateHallways();
        ApplyGrid();
        foreach(Room room in roomsOnMap) {
            foreach(Block block in room.GetRoomTypeChanges()) {
                listOfBlocksOnMap.Add(block);
            }
        }
        ApplyGrid();
        GenerateRoomObject();

        ApplySides();

        for(int x = 0; x < map.GetLength(0); x++) {
            for(int y = 0; y < map.GetLength(1); y++) {
                switch (map[x, y]) {
                    case 1:
                        Instantiate(blockFloor, new Vector3(x, y, 0), Quaternion.identity, this.transform);
                        break;
                    case 2:
                        Instantiate(blockWall, new Vector3(x, y, -0.5f),Quaternion.identity, this.transform);
                        break;
                    case 3:
                        Instantiate(blockWallOtherSide, new Vector3(x, y, -0.5f),Quaternion.Euler(0,0, -180), this.transform);
                        break;
                    case 4:
                        Instantiate(blockWallOtherSide, new Vector3(x, y, -0.5f), Quaternion.identity, this.transform);
                        break;
                    case 5:
                        Instantiate(blockWallOtherSide, new Vector3(x, y, -0.5f),Quaternion.Euler(0,0, -90), this.transform);
                        break;
                    case 7:
                        Instantiate(blockWallOnFloor, new Vector3(x, y, 0.5f),Quaternion.identity, this.transform);
                        break;
                    case 8:
                        Instantiate(blockWallOnFloorOnFloor, new Vector3(x, y, 0.5f),Quaternion.identity, this.transform);
                        break;
                }
            }
        }
    }

    private void GenerateRatKingRoom() {

    }

    private void ApplySides() {
        for(int x = 0; x < map.GetLength(0); x++) {
            for(int y = 0; y < map.GetLength(1); y++) {
                if (map[x,y] == 0) {
                    if (x >= 2 && x <= xSize - 2 && y >= 2 && y <= ySize - 2) {
                        if (map[x,y - 1] == 1){
                            map[x,y] = 2;
                        } else if (map[x,y + 1] == 1){
                            map[x,y] = 5;
                        } else if (map[x - 1, y] == 1){
                            map[x,y] = 4;
                        } else if (map[x + 1, y] == 1){
                            map[x,y] = 3;
                        }
                    }
                }
            }
        }
    }

    private void ApplyGrid() {
        foreach(Block block in listOfBlocksOnMap) {
            map[block.GetX(),block.GetY()] = block.GetBlockType();
        }
    }

    private void GenerateRoomObject() {
        foreach(Room room in roomsOnMap) {
            GameObject roomObj = new GameObject("Room (" + room.GetX() + "," + room.GetY() + ")");
            roomObj.transform.position = room.GetCentre();
            BoxCollider2D roomCol = roomObj.AddComponent<BoxCollider2D>();
            roomCol.isTrigger = true;
            roomCol.size = new Vector2(room.GetWidth(), room.GetHeight());
            roomObj.AddComponent<RoomActivation>().init(this, room);
        }
    }

    private void GenerateHallways() {
        foreach(GraphEdge graphEdge in nodeMap.GetMSTEdges()) {
            foreach(Vector2 floorPos in AStarPathfinding
            .startPathFiding(graphEdge.GetNode2().GetPosition(),
                             graphEdge.GetNode1().GetPosition(),
                             "Generation", null, map)) {
                listOfBlocksOnMap.Add(new Block((int) floorPos.x,(int) floorPos.y, 1));
            }
        }
    }

    private void SetSpawnRoom() {
        spawnRoom = roomsOnMap[Random.Range(0, roomsOnMap.Count)];
        Instantiate(player, new Vector3(spawnRoom.GetCentre().x, spawnRoom.GetCentre().y, -1), Quaternion.identity);
    }

    public List<Block> GetAllBlocksOnLevel() {
        List<Block> returnList = new List<Block>();
        foreach(Room room in roomsOnMap) {
            returnList.AddRange(room.ListOfBlocks());
        }
        return returnList;
    }

    public List<DTriangle> GetDTriangles() {
        return nodeMap.GetTriangles();
    }

    public int[,] GetMap() {
        return map;
    }

    private void OnDrawGizmos() {
        foreach(GraphEdge graphEdge in nodeMap.GetMSTEdges()) {
            Gizmos.DrawLine(graphEdge.GetNode1().GetPosition(), graphEdge.GetNode2().GetPosition());
        }
        
        /*foreach(DTriangle triangle in nodeMap.GetTriangles()) {
            Vector3 p1 = new Vector3(triangle.getPoint1().x, triangle.getPoint1().y, 0);
            Vector3 p2 = new Vector3(triangle.getPoint2().x, triangle.getPoint2().y, 0);
            Vector3 p3 = new Vector3(triangle.getPoint3().x, triangle.getPoint3().y, 0);
            Gizmos.DrawSphere(p1, 1);
            Gizmos.DrawSphere(p2, 1);
            Gizmos.DrawSphere(p3, 1);
            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p1);
        }*/
    }
}
