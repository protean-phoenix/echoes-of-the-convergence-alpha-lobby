using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorRoomScript : RoomScript
{
    //
    //  The currently assigned power
    //  to all rooms.
    //
    //  This value is affected whenever
    //  the ReactorRoom takes any damage.
    //
    [SerializeField] protected int capacity;

    //
    //  Start is called before
    //  the first frame update.
    //
    void Start()
    {
        initRoom();

        capacity = max_hp;
    }

    //
    //  Update is called once
    //  every frame.
    //
    void Update()
    {
        //
        //  When the ReactorRoom takes
        //  a point of damage, its capacity
        //  of generated power also decreases.
        //
        if (hp < capacity)
            capacity = (int)Math.Floor(hp);
    }

    public int getCapacity()
    {
        return capacity;
    }
}