using System;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private float height;
    [SerializeField] private float width;

    private static GameObject[] ship_list;
    private GameObject[] room_list;
    // Start is called before the first frame update
    void Start()
    {
        registerSelf();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void registerSelf()
    {
        if(ship_list == null)
        {
            ship_list = new GameObject[1];
        }
        if(ship_list.Length <= id)
        {
            GameObject[] longer_ship_list = new GameObject[id + 1];
            Array.Copy(ship_list, longer_ship_list, ship_list.Length);
            ship_list = longer_ship_list;
        }
        ship_list[id] = gameObject;
    }

    public void addRoomToShip(GameObject room)
    {
        int local_id = room.GetComponent<RoomScript>().getId();
        if (room_list == null)
        {
            room_list = new GameObject[1];
        }
        if(room_list.Length <= local_id)
        {
            GameObject[] longer_room_list = new GameObject[local_id + 1];
            Array.Copy(room_list, longer_room_list, room_list.Length);
            room_list = longer_room_list;
        }
        room_list[local_id] = room;
    }

    public static GameObject getShipById(int id)
    {
        return ship_list[id];
    }

    public GameObject getRoomById(int id)
    {
        return room_list[id];
    }

    public float getHeight()
    {
        return height;
    }

    public float getWidth()
    {
        return width;
    }
}
