using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
    private List<GameObject> rooms;
    [SerializeField] private float height;
    [SerializeField] private float width;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addRoomToShip(GameObject room)
    {
        if (rooms == null)
        {
            rooms = new List<GameObject>();
        }
        rooms.Add(room);
        Debug.Log("room added " + rooms.Count);
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
