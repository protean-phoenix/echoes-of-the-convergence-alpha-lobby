using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorRoomScript : RoomScript
{
    //
    //  The capacity (max current power)
    //  this reactor can use to power rooms.
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
        capacity = max_hp * 2;

        initRoom();
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
            capacity = Mathf.FloorToInt(hp) * 2;
    }

    //
    //  Commented out for the time
    //  being. After further design
    //  choices and AI implementations,
    //  this method will need to be updated
    //  or relocated.
    //

    //public void reroutePowerToRoomById(
    //    int initialRoomId,
    //    int newRoomId,
    //    int power
    //)
    //{
    //    //
    //    //  TODO: Implement logic for
    //    //        rerouting power from
    //    //        room A, to room B.
    //    //
    //}

    public int getCapacity()
    {
        return capacity;
    }
}