using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
    [SerializeField] private int        id;

    [SerializeField] private float      flight_height;
    [SerializeField] private float      flight_width;

    [SerializeField] private float      shield_height;
    [SerializeField] private float      shield_width;
    [SerializeField] private GameObject shield_prefab;
                     private GameObject ship_shield;
    [SerializeField] private GameObject shield_bar;
                     private float      shield_hp;
    [SerializeField] private int        max_shield;

    [SerializeField] private int        max_hp;
                     private float      hp;
    [SerializeField] private GameObject health_bar;
                     private float      dodge;

    [SerializeField] private int        accumulative_power;

    private GameObject[]                crew_list;
    private static GameObject[]         ship_list;
    private GameObject[]                room_list;
    private List<GameObject>            missiles_inflight;
    private List<GameObject>            craft_inflight;

    protected List<GameObject>          hangars;
    protected List<GameObject>          missiles;
    protected List<GameObject>          lasers;
    protected List<GameObject>          reactors;
    protected List<GameObject>          engines;
    protected List<GameObject>          point_defenses;
    protected List<GameObject>          shields;

    //
    //  Start is called before
    //  the first frame update.
    //
    void Start()
    {
        registerSelf();

        hp = max_hp;
        health_bar.GetComponent<StatBarScript>().renderBar(max_hp);

        shield_hp = max_shield;
        shield_bar.GetComponent<StatBarScript>().renderBar(max_shield);

        missiles_inflight   = new List<GameObject>();
        craft_inflight      = new List<GameObject>();
        
        //
        //  Instantiate all the room lists.
        //
        hangars         = new List<GameObject>();
        missiles        = new List<GameObject>();
        lasers          = new List<GameObject>();
        engines         = new List<GameObject>();
        reactors        = new List<GameObject>();
        engines         = new List<GameObject>();
        point_defenses  = new List<GameObject>();
        shields         = new List<GameObject>();
    }

    //
    //  Update is called
    //  once per frame.
    //
    void Update()
    {
        if(shield_hp > 0 && ship_shield == null)
        {
            ship_shield = GameObject.Instantiate(shield_prefab, gameObject.transform);
            ship_shield.transform.localScale = new Vector2(shield_width, shield_height);

            //
            //  If this is Player 2,
            //  switch the layer of the
            //  shield to Player2Shield.
            //
            if(id == 1)
                ship_shield.layer = 7;
        }

        else if(shield_hp <= 0 && ship_shield != null)
            GameObject.Destroy(ship_shield);
    }

    public void incrementPowerOfRoom(GameObject room)
    {
        RoomScript selected = room.GetComponent<RoomScript>();

        //
        //  A room can only draw power
        //  proportional to its current HP.
        //
        if (
            Mathf.FloorToInt(selected.getHp()) < ( (selected.getAssignedPower() / 2) + 1 )
        )
            return;

        selected.setAssignedPower(selected.getAssignedPower() + 2);
    }

    public void registerSelf()
    {
        if(ship_list == null)
            ship_list = new GameObject[1];

        if(ship_list.Length <= id)
        {
            GameObject[] longer_ship_list = new GameObject[id + 1];
            Array.Copy(ship_list, longer_ship_list, ship_list.Length);
            ship_list = longer_ship_list;
        }

        ship_list[id] = gameObject;
    }

    public void addCrewToShip(GameObject crew)
    {
        int local_id = crew.GetComponent<CrewScript>().getId();

        if (crew_list == null)
            crew_list = new GameObject[1];

        if (crew_list.Length <= local_id)
        {
            GameObject[] extended_list = new GameObject[local_id + 1];

            Array.Copy(crew_list, extended_list, crew_list.Length);

            crew_list = extended_list;
        }

        crew_list[local_id] = crew;
    }

    public void addRoomToShip(GameObject room)
    {
        int local_id = room.GetComponent<RoomScript>().getId();

        if (room_list == null)
            room_list = new GameObject[1];

        if(room_list.Length <= local_id)
        {
            GameObject[] longer_room_list = new GameObject[local_id + 1];
            Array.Copy(room_list, longer_room_list, room_list.Length);
            room_list = longer_room_list;
        }

        room_list[local_id] = room;

        indexRoomUponAddition(room);
    }

    //
    //  Whenever a new Room is 
    //  added to the Ship, we want
    //  to place it inside of its respective
    //  "filtered" list.
    //
    //  TODO: Possibly research a method of
    //        better handling this? Without
    //        chaining conditionals, that is.
    //
    public void indexRoomUponAddition(GameObject room)
    {
        //
        //  If the room doesn't exist,
        //  well... I don't know what to say.
        //
        if (room == null)
            return;

        if (room.GetComponent<LaserRoomScript>() != null)
        {
            if (lasers == null)
                lasers = new List<GameObject>();

            lasers.Add(room);
        }

        else if (room.GetComponent<HangarRoomScript>() != null)
        {
            if (hangars == null)
                hangars = new List<GameObject>();

            hangars.Add(room);
        }

        else if (room.GetComponent<MissileRoomScript>() != null)
        {
            if (missiles == null)
                missiles = new List<GameObject>();

            missiles.Add(room);
        }

        else if (room.GetComponent<EngineScript>() != null)
        {
            if (engines == null)
                engines = new List<GameObject>();

            engines.Add(room);
        }

        else if (room.GetComponent<ShieldScript>() != null)
        {
            if (shields == null)
                shields = new List<GameObject>();

            shields.Add(room);
        }

        else if (room.GetComponent<ReactorRoomScript>() != null)
        {
            if (reactors == null)
                reactors = new List<GameObject>();

            reactors.Add(room);

            accumulative_power += room.GetComponent<ReactorRoomScript>().getCapacity();
        }

        else if (room.GetComponent<PointDefenseScript>() != null)
        {
            if (point_defenses == null)
                point_defenses = new List<GameObject>();

            point_defenses.Add(
                room
            );
        }

        else
            return;
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

    public void recalculateDodge()
    {
        dodge = 0;
        foreach(GameObject room in room_list)
        {
            if(room != null && room.GetComponent<EngineScript>() != null)
            {
                dodge += (1 - dodge) * room.GetComponent<EngineScript>().getDodge();
            }
        }
    }

    public bool rollDodge()
    {
        float roll = (float)(new System.Random().NextDouble());
        if (roll <= dodge)
        {
            return true;
        }
        else
        {
            return false;
        }
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

    public GameObject[] getCrewList()
    {
        return crew_list;
    }

    public GameObject getCrewById(int id)
    {
        return crew_list[id];
    }

    public GameObject[] getRoomList()
    {
        return room_list;
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
    
    public List<GameObject> getLasersList()
    {
        return lasers;
    }

    public List<GameObject> getReactorsList()
    {
        return reactors;
    }

    public List<GameObject> getHangarsList()
    {
        return hangars;
    }

    public List<GameObject> getMissilesList()
    {
        return missiles;
    }

    public List<GameObject> getEnginesList()
    {
        return engines;
    }

    public List<GameObject> getPointDefensesList()
    {
        return point_defenses;
    }

    public List<GameObject> geShieldsList()
    {
        return shields;
    }
}
