using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrewType
{
    Scientist,
    Pilot,
    Engineer,
    Gunnery
}

public class CrewScript : MonoBehaviour
{
    //
    //  The parent ship this crew
    //  has currently boarded.
    //
    [SerializeField] protected GameObject owningShip;

    //
    //  The identifier of this crew.
    //
    [SerializeField] protected int id;

    //
    //  This crew's respective
    //  "specialist" points. They go
    //  up to a maximum of 20 pts.
    //
    //  -> SCIPoints - Science points   (Scientist; applies stats to Laser, Reactor and Shield)
    //  -> PLTPoints - Pilot points     (Pilot;     applies stats to Hangar)
    //  -> ENGPoints - Engineer points  (Engineer;  applies stats to Engine)
    //  -> WPNPoints - Weapon points    (Gunnery;   applies stats to Missile)
    //
    [SerializeField] protected int SCIPoints;
    [SerializeField] protected int PLTPoints;
    [SerializeField] protected int ENGPoints;
    [SerializeField] protected int WPNPoints;

    [SerializeField] protected CrewType         crewType;
    [SerializeField] protected GameObject       appointedRoom;

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

    public void initCrew()
    {
        SCIPoints = 10;
        PLTPoints = 10;
        ENGPoints = 10;
        WPNPoints = 10;

        switch(crewType)
        {
            case CrewType.Engineer:
                ENGPoints = 20;
                break;

            case CrewType.Scientist:
                SCIPoints = 20;
                break;

            case CrewType.Pilot:
                PLTPoints = 20;
                break;

            case CrewType.Gunnery:
                WPNPoints = 20;
                break;

            //
            //  What?
            //
            default:
                break;
        }
    }

    //
    //  A path-finding algorithm for
    //  (re-)locating the crew.
    //
    //  It tries to find the shortest
    //  path to the destination room,
    //  whilst taking into consideration
    //  whether or not rooms are connected
    //  with lifts, corridors or other rooms.
    //
    //  If none of the rooms that provide
    //  a path to the destination don't have a lift,
    //  and the crew must move vertically
    //  to reach a room, then the destination is unreachable.
    //
    public void updateCrewLocation()
    {
        Vector2 currentPosition = gameObject.transform.position;
        Vector2 wantedLocation  = appointedRoom.transform.position;
        float   locationHeight  = appointedRoom.GetComponent<SpriteRenderer>().sprite.rect.height;

        //
        //  Instead of placing the crew
        //  in the center (X, Y)
        //  position of the room, place them
        //  near the bottom (floor in the sprite).
        //
        wantedLocation = new Vector2(
            wantedLocation.x,
            wantedLocation.y - (float)Math.Round(locationHeight / 76, 3)
        );

        //
        //  TODO:   Linearize this part.
        //          Currently, it grows exponentially,
        //          meaning that some movements are
        //          very rapid, whilst others are
        //          very slow, depending on the distance
        //          between the current position and the
        //          location's position.
        //
        //          Unknown why it's exponential growth.
        //
        if (!appointedRoom.transform.position.Equals(gameObject.transform.position))
        {
            if (!Mathf.Approximately(currentPosition.y, wantedLocation.y) || !Mathf.Approximately(currentPosition.x, wantedLocation.x))
            {
                currentPosition = gameObject.transform.position;

                float delta = 0.0f;

                if (Math.Abs(currentPosition.y - wantedLocation.y) > 0.01f)
                {
                    if (wantedLocation.y > currentPosition.y)
                        delta += Math.Abs(wantedLocation.y) / 500f;

                    else
                        delta -= Math.Abs(wantedLocation.y) / 500f;

                    gameObject.transform.position = new Vector2(currentPosition.x, (float)Math.Round(currentPosition.y + delta, 3));
                }

                else if (Math.Abs(currentPosition.x - wantedLocation.x) > 0.01f)
                {
                    if (wantedLocation.x > currentPosition.x)
                        delta += Math.Abs(wantedLocation.x) / 500f;

                    else
                        delta -= Math.Abs(wantedLocation.x) / 500f;

                    gameObject.transform.position = new Vector2((float)Math.Round(currentPosition.x + delta, 3), currentPosition.y);
                }
            }
        }
    }

    public bool isInAssignedRoom()
    {
        Vector2 currentLocation  = gameObject.transform.position;
        Vector2 assignedLocation = appointedRoom.transform.position;
        float locationHeight     = appointedRoom.GetComponent<SpriteRenderer>().sprite.rect.height;

        //
        //  Have to remember that
        //  there's an offset from
        //  the Y point.
        //
        assignedLocation = new Vector2 (
            assignedLocation.x,
            assignedLocation.y - (float)Math.Round(locationHeight / 76, 3)
        );

        return (
            (Math.Abs(currentLocation.x - assignedLocation.x) < 0.01f) &&
            (Math.Abs(currentLocation.y - assignedLocation.y) < 0.01f)
        );
    }

    public void assignRoom(GameObject room)
    {
        //
        //  If the `room` object is
        //  null, then we can't do anything.
        //
        //  But we know something's wrong.
        //
        if (room == null)
            return;

        appointedRoom = room;
    }

    public int getId()
    {
        return id;
    }
}