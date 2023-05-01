using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftScript : MonoBehaviour
{
    //
    //  The identifier of this lift
    //  carriage.
    //
    [SerializeField] protected int id;

    //
    //  The parent ship this lift
    //  is a part of.
    //
    [SerializeField] protected GameObject owningShip;

    //
    //  The rooms this lift
    //  connects vertically.
    //
    //  Sorted from top-most
    //  room at index 0, to
    //  bottom-most room at index N - 1.
    //
    [SerializeField] protected GameObject[] connectingRooms;

    //
    //  The current room this lift
    //  is on. (For rendering the sprite)
    //
    [SerializeField] protected GameObject currentRoom;

    //
    //  Crew that are riding
    //  this lift.
    //
    [SerializeField] protected List<GameObject> crewList;

    //
    //  The capacity (maximum amount of crew)
    //  this lift can hold at once.
    //
    [SerializeField] protected int capacity;

    //
    //  The rooms this elevator needs
    //  to pick up passengers from,
    //  or rooms to drop them off on.
    //
    [SerializeField] protected GameObject[] roomsToVisit;

    //
    //  Rooms that this elevator can
    //  stop at to pick up crew whilst
    //  it's on its way to the original
    //  room that it needs to visit.
    //
    [SerializeField] protected List<GameObject> roomsOnTheWay;

    //
    //  Whether or not this lift
    //  has currently stopped to
    //  board crew.
    //
    [SerializeField] protected bool stop;

    //
    //  Start is called before
    //  the initial frame update.
    //
    void Start()
    {
        currentRoom = null;
        capacity = 4;
        stop = false;

        roomsOnTheWay = new List<GameObject>();
        crewList      = new List<GameObject>();
    }

    //
    //  Update is called
    //  once per frame.
    //
    void Update()
    {
        //if (roomsToVisit != null && roomsToVisit.Length > 0)
        //{
        //    goToRoom(roomsToVisit[0]);
        //}

        ShipScript shipComp = owningShip.GetComponent<ShipScript>();

        if (shipComp.getLiftById(id) == null)
        {
            shipComp.addLiftToShip(gameObject);
        }

        if (currentRoom == null)
            findCurrentRoom();

        if (roomsToVisit != null && roomsToVisit.Length > 0 && capacity > crewList.Count)
            goToRoom(roomsToVisit[0]);
    }

    public void findCurrentRoom()
    {
        foreach (GameObject room in owningShip.GetComponent<ShipScript>().getRoomList())
        {
            if (
                Utils.getDistance(gameObject.transform.position.y, room.transform.position.y) <= 0.50f &&
                Utils.getDistance(gameObject.transform.position.x, room.transform.position.x) <= 3.00f
            )
            {
                currentRoom = room;

                break;
            }
        }
    }

    public void callElevator(GameObject crew)
    {
        List<GameObject> temporary = new List<GameObject>(roomsToVisit);

        if (
            temporary.Find(r =>
                r.GetComponent<RoomScript>().getId() ==
                crew.GetComponent<CrewScript>().getCurrentRoom().GetComponent<RoomScript>().getId()
            ) != null
        )
            return;

        temporary.Add(crew.GetComponent<CrewScript>().getCurrentRoom());

        roomsToVisit = temporary.ToArray();
    }

    public void goToRoom(GameObject room)
    {
        if (roomsOnTheWay == null)
            roomsOnTheWay = new List<GameObject>();
        
        //
        //  If the room where we need to
        //  go isn't in our `connectingRooms`
        //  list, it's unreachable for this lift.
        //
        //  This is a preliminary check to
        //  (hopefully) avoid any game-breaking bugs.
        //
        if (
            Array.FindAll(connectingRooms,
                r => r.GetComponent<RoomScript>().getId() == room.GetComponent<RoomScript>().getId()
            ).Length < 1
        )
            return;

        if (Utils.getDistance(gameObject.transform.position.y, room.transform.position.y) < 0.01f)
        {
            currentRoom = room;

            return;
        }

        float delta = 0.002f;

        if (room.transform.position.y < gameObject.transform.position.y)
            delta *= -1.0f;

        if (Utils.getDistance(gameObject.transform.position.y, room.transform.position.y) >= 2.00f && !stop)
        {
            List<GameObject> temporaryWay    = new List<GameObject>(roomsOnTheWay);
            List<GameObject> temporaryVisits = new List<GameObject>(roomsToVisit);
            List<int> indexForRemovals = new List<int>();

            //
            //  If we already hadn't
            //  looked for any rooms
            //  we may need to stop at
            //  (that are on the way to
            //  the target location), do so.
            //
            if (roomsOnTheWay.Count < 1)
            {

                for (int Index = 0; Index < roomsToVisit.Length; Index++)
                {
                    //
                    //  Ensure there are actual
                    //  crewmates in the room
                    //  we may need to visit.
                    //
                    if (
                        roomsToVisit[Index].GetComponent<RoomScript>().getCrewList().Length > 0 &&
                        roomsToVisit[Index].GetComponent<RoomScript>().getId() != room.GetComponent<RoomScript>().getId()
                    )
                    {
                        if (
                            (room.transform.position.y > 0 && room.transform.position.y > roomsToVisit[Index].transform.position.y &&
                            gameObject.transform.position.y < room.transform.position.y) ||

                            (room.transform.position.y < 0 && room.transform.position.y < roomsToVisit[Index].transform.position.y &&
                            gameObject.transform.position.y > room.transform.position.y)
                        )
                        {
                            if (roomsOnTheWay.Find(r => r.GetComponent<RoomScript>().getId() != roomsToVisit[Index].GetComponent<RoomScript>().getId()) == null)
                            {
                                roomsOnTheWay.Add(roomsToVisit[Index]);

                                indexForRemovals.Add(Index);
                            }
                        }
                    }

                    else
                        indexForRemovals.Add(Index);
                }

                if (indexForRemovals.Count > 0)
                    temporaryVisits.RemoveAll(t => indexForRemovals.Contains(temporaryVisits.IndexOf(t)));

                roomsToVisit = temporaryVisits.ToArray();
                GameObject[] temp = roomsOnTheWay.ToArray();

                if (roomsToVisit.Length > 1)
                    sortRooms(roomsToVisit);

                if (temp.Length > 1)
                    sortRooms(temp);

                roomsOnTheWay = new List<GameObject>(temp);
            }

            if (
                roomsOnTheWay.Count > 0 &&
                Utils.getAbsoluteDifference(gameObject.transform.position.y, roomsOnTheWay[0].transform.position.y) < 0.01f
            )
            {
                delta = 0.0f;

                stop = true;

                currentRoom = roomsOnTheWay[0];

                Debug.Log(currentRoom.GetComponent<RoomScript>());

                roomsOnTheWay.RemoveAt(0);

                temporaryWay.FindAll(r => r.GetComponent<RoomScript>().getId() != currentRoom.GetComponent<RoomScript>().getId());

                roomsToVisit = temporaryWay.ToArray();

                List<GameObject> crewsInRoom = new List<GameObject>(
                    currentRoom
                        .GetComponent<RoomScript>()
                        .getCrewList()
                );

                if (crewsInRoom.Count > 0)
                {
                    crewsInRoom[0]
                        .GetComponent<CrewScript>()
                        .boardLift(gameObject);

                    Debug.Log(crewsInRoom.Count);
                    Debug.Log(crewsInRoom[0].GetComponent<CrewScript>().isInRoom(gameObject));

                    if (crewsInRoom[0].GetComponent<CrewScript>().isInRoom(gameObject))
                    {
                        crewList.Add(crewsInRoom[0]);
                        crewsInRoom.RemoveAt(0);
                    }
                }

                else
                    stop = false;
            }
            
        }

        if (!stop)
            gameObject.transform.position = new Vector2 (
                gameObject.transform.position.x,
                (float)Math.Round(gameObject.transform.position.y + delta, 3)
            );
    }

    public void attachRoom(GameObject room)
    {
        List<GameObject> temporary = new List<GameObject>(connectingRooms);

        temporary.Add(room);

        connectingRooms = temporary.ToArray();
    }

    public GameObject getRoomById(int id)
    {
        List<GameObject> temporary = new List<GameObject>(connectingRooms);

        return temporary.Find(r => r != null && r.GetComponent<RoomScript>().getId() == id);
    }

    
    public GameObject[] getConnectingRooms()
    {
        return connectingRooms;
    }

    //
    //  Sorts rooms depending
    //  on their Y coordinate.
    //
    //  Uses a quick-sort algorithm
    //  of time complexity O(N * log N).
    //
    public void sortRooms(GameObject[] rooms)
    {
        quickSort(rooms, 0, rooms.Length);
    }

    //
    //  Swaps two elements
    //  inside of an array.
    //
    private void swap<T>(
        T[] rooms,
        int i,
        int j
    )
    {
        T temporary = rooms[i];

        rooms[i] = rooms[j];
        rooms[j] = temporary;
    }

    //
    //  Sort lower elements left of pivot
    //  and higher elements right of pivot.
    //
    //  In our case, it's rooms that are besides,
    //  under the room and above the pivot room.
    //
    private int partition(
        GameObject[] rooms,
        int low,
        int high
    )
    {
        //
        //  Choose the last element
        //  of the array as the pivot.
        //
        GameObject pivot = rooms[high];

        //
        //  Index of smaller element and
        //  indicates the right position
        //  of pivot found so far.
        //
        int i = (low - 1);

        for (int j = low; j <= high - 1; j++)
        {
            if (rooms[j].transform.position.y > pivot.transform.position.y)
            {
                //
                //  Increment index of lower element.
                //
                //  In this case, a room above it.
                //
                i++;

                swap(rooms, i, j);
            }
        }

        swap(rooms, i + 1, high);

        return (i + 1);
    }

    //
    //  Quick sort algorithm.
    //
    //  -> `rooms` — An array of room objects.
    //  -> `low`   — Start index.
    //  -> `high`  — End index.
    //
    private void quickSort(
        GameObject[] rooms,
        int low,
        int high
    )
    {
        if (low < high)
        {
            int pi = partition(rooms, low, high);

            //
            //  Divide and conquer approach;
            //  separately sort elements
            //  before and after partition index.
            //
            quickSort(rooms, low, pi - 1);
            quickSort(rooms, pi + 1, high);
        }
    }

    public int getId()
    {
        return id;
    }
}