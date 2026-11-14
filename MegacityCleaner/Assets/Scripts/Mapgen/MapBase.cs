using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapBase : MonoBehaviour{
    public GameObject welcomeArea;
    public MapData mapData;

    public List<GeneratedRoom> roomsGenerated = new List<GeneratedRoom>();

    public static MapBase instance;
    public bool isMapDone;
    
    void Awake(){
        if(instance == null){
            instance = this;
            return;
        }
        Destroy(this);
    }

    [ContextMenu("ForceGen")]
    public void ForceGenMap(){
        StartCoroutine(GenerateMap());
    }

    public IEnumerator GenerateMap(){
        yield return new WaitForSeconds(0.1f);
        int credits = Mathf.RoundToInt(Random.Range(mapData.mapRoomCredits.x, mapData.mapRoomCredits.y));
        int reserveCredits = 100;

        RoomOpening[] openings = FindObjectsOfType<RoomOpening>();

        for(int i = 0; i < credits; i++){
            openings = FindObjectsOfType<RoomOpening>();
            List<RoomOpening> validOpenings = new List<RoomOpening>();

            foreach(var op in openings){
                if(!op.didGen){
                    validOpenings.Add(op);
                }
            }

            int selected = Random.Range(0, validOpenings.Count);
            int selectedRoom = Random.Range(0, mapData.mapTiles.Count);

            if(validOpenings.Count > 0){
                validOpenings[selected].didGen = true;
                Room temp = Instantiate(mapData.mapTiles[selectedRoom].tile, validOpenings[selected].transform.position, validOpenings[selected].transform.rotation).GetComponent<Room>();

                bool didKeep = true;

                Collider[] cols = Physics.OverlapBox(temp.roomCenter.position, (temp.roomSize - new Vector3(0.2f, 0.2f, 0.2f)) / 2, temp.roomCenter.rotation);
                foreach(var col in cols){
                    if(col.gameObject.tag == "MapRoom"){
                        if(!temp.IsGeoMine(col.gameObject) || IsInPreventZone(col.transform.position)){
                            Destroy(temp.gameObject);
                            didKeep = false;

                            if(reserveCredits > 0){
                                credits++;                        
                            }else{
                                reserveCredits--;
                            }

                            break;
                        }
                    }
                }

                if(didKeep){
                    roomsGenerated.Add(new GeneratedRoom(selectedRoom, temp.transform.position, temp.transform.rotation));
                }

                yield return new WaitForSeconds(0.1f);            
            }
        }
        
        yield return new WaitForSeconds(0.1f);
        CheckDoors();
        yield return new WaitForSeconds(0.1f);
        CleanDoormats();

        isMapDone = true;
    }

    bool IsInPreventZone(Vector3 pos){
        MapPreventZone[] zones = FindObjectsOfType<MapPreventZone>();
        foreach(var zone in zones){
            if(zone.IsInZone(pos)){
                return true;
            }
        }

        return false;
    }
    
    public void CheckDoors(){
        RoomOpening[] openings = FindObjectsOfType<RoomOpening>();
        foreach(var op in openings){
            op.CheckDoor();
        }
    }

    public void CleanDoormats(){
        Doormat[] mats = FindObjectsOfType<Doormat>();
        foreach(var dm in mats){
            Destroy(dm.gameObject);
        }
    }

    public void SpawnRoom(GeneratedRoom roomData){
        Instantiate(mapData.mapTiles[roomData.id].tile, roomData.pos, roomData.rot);
    }
}

[System.Serializable]
public class GeneratedRoom{
    public int id;
    public Vector3 pos;
    public Quaternion rot;

    public GeneratedRoom(int roomID, Vector3 roomPos, Quaternion roomRot){
        id = roomID;
        pos = roomPos;
        rot = roomRot;
    }
}