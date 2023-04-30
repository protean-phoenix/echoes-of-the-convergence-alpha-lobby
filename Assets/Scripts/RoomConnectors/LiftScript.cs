using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftScript : MonoBehaviour
{
    //
    //  The rooms this lift
    //  connects vertically.
    //
    //  Sorted from top-most
    //  room at index 0, to
    //  bottom-most room at index N - 1.
    //
    protected GameObject[] connectingRooms;

    //
    //  The current room this lift
    //  is on. (For rendering the sprite)
    //
    protected GameObject currentRoom;

    //
    //  Start is called before
    //  the initial frame update.
    //
    void Start()
    {

    }

    //
    //  Update is called
    //  once per frame.
    //
    void Update()
    {

    }

    public GameObject[] getConnectingRooms()
    {
        return connectingRooms;
    }

    public void indexNewRoom(GameObject room)
    {

    }
}