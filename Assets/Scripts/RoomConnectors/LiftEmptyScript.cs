using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftEmptyScript : MonoBehaviour
{
    //
    //  The parent ship this static
    //  prefab (LiftEmpty) is a part of.
    //
    [SerializeField] protected GameObject owningShip;

    //
    //  The lift that is attached
    //  to this static prefab.
    //
    [SerializeField] protected GameObject lift;

    protected bool found;

    //
    //  Start is called before
    //  the initial frame update.
    //
    void Start()
    {
        found = false;
    }

    //
    //  Update is called
    //  once per frame.
    //
    void Update()
    {
        if (!found)
        {
            findRooms();

            found = true;
        }
    }

    protected void findRooms()
    {
        GameObject[]     shipRooms  = owningShip.GetComponent<ShipScript>().getRoomList();

        foreach (GameObject room in shipRooms)
        {
            float roomXAxis     = room.transform.position.x;
            float prefabXAxis   = gameObject.transform.position.x;

            float roomYAxis     = room.transform.position.y;
            float prefabYAxis   = gameObject.transform.position.y;

            if (
                Utils.getDistance(roomYAxis, prefabYAxis) > 0.50f ||
                Utils.getDistance(roomXAxis, prefabXAxis) > 3.00f
           )
                continue;

            if (lift.GetComponent<LiftScript>().getRoomById(room.GetComponent<RoomScript>().getId()) == null)
            {
                lift.GetComponent<LiftScript>().attachRoom(room);

                room.GetComponent<RoomScript>().attachLift(lift);
            }
        }
    }
}