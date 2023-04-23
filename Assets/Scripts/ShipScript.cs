using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private float flight_height;
    [SerializeField] private float flight_width;
    [SerializeField] private float shield_height;
    [SerializeField] private float shield_width;
    [SerializeField] private GameObject shield_prefab;
    private GameObject ship_shield;
    [SerializeField] private int max_hp;
    private float hp;
    [SerializeField] private GameObject health_bar;
    [SerializeField] private int max_shield;
    private float shield_hp;
    [SerializeField] private GameObject shield_bar;

    private static GameObject[] ship_list;
    private GameObject[] room_list;
    private List<GameObject> missiles_inflight;
    private List<GameObject> craft_inflight;
    // Start is called before the first frame update
    void Start()
    {
        registerSelf();
        hp = max_hp;
        health_bar.GetComponent<StatBarScript>().renderBar(max_hp);
        shield_hp = max_shield;
        shield_bar.GetComponent<StatBarScript>().renderBar(max_shield);
        missiles_inflight = new List<GameObject>();
        craft_inflight = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shield_hp > 0 && ship_shield == null)
        {
            ship_shield = GameObject.Instantiate(shield_prefab, gameObject.transform);
            ship_shield.transform.localScale = new Vector2(shield_width, shield_height);
            if(id == 1)//if this is player 2, switch the layer of the shield to player2shield
            {
                ship_shield.layer = 7;
            }
        }else if(shield_hp <= 0 && ship_shield != null)
        {
            GameObject.Destroy(ship_shield);
        }
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

    public void registerCraft(GameObject craft) {
        craft_inflight.Add(craft);
    }

    public void registerMissile(GameObject missile)
    {
        missiles_inflight.Add(missile);
    }

    public void deregisterCraft(GameObject craft)
    {
        craft_inflight.Remove(craft);
    }

    public void deregisterMissile(GameObject missile)
    {
        missiles_inflight.Remove(missile);
    }

    public List<GameObject> getMissileList()
    {
        return missiles_inflight;
    }

    public List<GameObject> getCraftList()
    {
        return craft_inflight;
    }

    public void TakeHullDamage(float damage)
    {
        hp -= MathF.Abs(damage);
        health_bar.GetComponent<StatBarScript>().adjustBar(-Mathf.Abs(damage));
    }
    
    public void TakeShieldDamage(float damage)
    {
        shield_hp -= MathF.Abs(damage);
        shield_bar.GetComponent<StatBarScript>().adjustBar(-Mathf.Abs(damage));
    }

    public int getId()
    {
        return id;
    }

    public static GameObject getShipById(int id)
    {
        return ship_list[id];
    }

    public static GameObject getEnemyShip(GameObject ship)
    {
        int id = ship.GetComponent<ShipScript>().getId();
        if(id == 0)
        {
            return ship_list[1];
        }else if (id == 1)
        {
            return ship_list[0];
        }
        return null;
    }

    public GameObject getRoomById(int id)
    {
        return room_list[id];
    }

    public float getFlightHeight()
    {
        return flight_height;
    }

    public float getFlightWidth()
    {
        return flight_width;
    }
}
