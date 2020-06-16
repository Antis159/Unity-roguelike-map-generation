using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class GenerateButBruh2 : MonoBehaviour
{
    public GameObject room1;
    public GameObject room2H;
    public GameObject room2C;
    public GameObject room3;
    public GameObject room4;

    public int doorCount;
    int hFix;
    int doorPosCount;

    public Queue<Transform> availableDoors = new Queue<Transform>();
    public Queue<int> availableDoorsRot = new Queue<int>();
    public GameObject spawnRoom(int doorBudget)
    {
        if (doorBudget == 0) return null;

        GameObject[] rooms = new[] { room1, room2H, room2C, room3, room4 };

        if (doorBudget >= 2) doorBudget++;
        return Instantiate(rooms[Random.Range(0, Mathf.Min(doorBudget, rooms.Length))]);
    }

    public int countDoors(GameObject room)
    {
        return room.GetComponentsInChildren<Transform>().Where(gameObject => gameObject.name.Contains("door")).Count();
    }

    public Transform[] getDoors(GameObject room)
    {
        return room.GetComponentsInChildren<Transform>().Where(gameObject => gameObject.name.Contains("door")).ToArray();
    }
    public void destroyDoors()
    {
        List<Vector3> doorPos = new List<Vector3>();
        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(gameObject => gameObject.name == "door");
        //GameObject [] doors = new GameObject[objects.Count()];
        //doors = objects.ToArray();
        foreach (var door in objects)
        {
            if (doorPos.Contains(door.transform.position))
            {
                doorPos.Remove(door.transform.position);
            }
            else doorPos.Add(door.transform.position);
        }
        //foreach (var x in doorPos)
        //{
        //    Debug.Log(x.ToString());
        //}
        foreach (var door in objects)
        {
            if (doorPos.Contains(door.transform.position))
            {
                Debug.Log("Door Destroyed");
                DestroyImmediate(door);
            }
        }
    }
    public int getDoorRot2(Vector3 rotation, int count)
    {
        if (rotation.y == -180) return (2 + count) % 4;
        if (rotation.y == -90) return (3 + count) % 4;
        if (rotation.y == 90) return (1 + count) % 4;
        return (0 + count) % 4;
        
    }
    public Vector3 rotToRotation(int rot)
    {
        if (rot == 1) return new Vector3(0, -90, 0);
        if (rot == 2) return new Vector3 (0, 0, 0);
        if (rot == 3) return new Vector3(0, 90, 0);
        return new Vector3 (0, -180, 0);
    }
    public Vector3 rotVector(Vector3 roomRot)
    {
        if (roomRot.y == -180) return new Vector3(0, 0, 5);
        if (roomRot.y == 90) return new Vector3(-5, 0, 0);
        if (roomRot.y == 0) return new Vector3(0, 0, -5);
        return new Vector3(5, 0, 0);
    }
    void Start()
    {
        int currentDoorCount = 0;
        List<Vector3> roomPositions = new List<Vector3>();
        for (int i = 0; i < 10000 && doorCount > currentDoorCount; i++)
        {
            Transform[] doors;
            int doorBudget = doorCount - currentDoorCount;
            GameObject spawnedRoom = spawnRoom(doorBudget);
            if (i > 0)
            {
                Vector3 roomRotation = rotToRotation(availableDoorsRot.Peek());
                Vector3 doorPosition = availableDoors.Peek().position + rotVector(roomRotation);
                doorPosition.y = 0;
                spawnedRoom.transform.rotation = Quaternion.Euler(roomRotation);
                spawnedRoom.transform.position = doorPosition;
                if (Mathf.Abs(spawnedRoom.transform.position.x) < 1) doorPosition.x = 0;
                if (Mathf.Abs(spawnedRoom.transform.position.z) < 1) doorPosition.z = 0;
                spawnedRoom.transform.position = doorPosition;
                doors = getDoors(spawnedRoom);
                if (roomPositions.Contains(doorPosition))
                {
                    Debug.Log("Destroyed");
                    availableDoors.Dequeue();
                    availableDoorsRot.Dequeue();
                    DestroyImmediate(spawnedRoom);
                    if (availableDoors.Count <= 0) break;
                    continue;
                }
                else
                {
                    availableDoors.Dequeue();
                    availableDoorsRot.Dequeue();
                    for (int j = 0; j < doors.Length; j++)
                    {
                        availableDoors.Enqueue(doors[j]);
                        availableDoorsRot.Enqueue(getDoorRot2(roomRotation,j+hFix));
                        if (spawnedRoom.name.Contains("Room2 H")) hFix++;
                    }
                }
            }
            else
            {
                doors = getDoors(spawnedRoom);
                Vector3 v = spawnedRoom.transform.rotation.eulerAngles;
                for (int j = 0; j < doors.Length; j++)
                {
                    availableDoors.Enqueue(doors[j]);
                    availableDoorsRot.Enqueue(getDoorRot2(v,j+hFix));
                    if (spawnedRoom.name.Contains("Room2 H")) hFix++;
                }
                Debug.Log("Start");
            }
            hFix = 0;
            currentDoorCount += countDoors(spawnedRoom);
            if (spawnedRoom != null) roomPositions.Add(spawnedRoom.transform.position);
            if (availableDoors.Count == 0) break;
        }
        //destroyDoors();
        Debug.Log("Complete");
    }

}
