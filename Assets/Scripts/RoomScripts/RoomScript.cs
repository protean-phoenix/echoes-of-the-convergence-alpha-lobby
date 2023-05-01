using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    [SerializeField] protected GameObject   owningShip;

    [SerializeField] protected int          id;
    [SerializeField] protected int          max_hp;
                     protected float        hp;

                     protected GameObject[] room_health_display;
    [SerializeField] protected GameObject   room_health_bar;

    //
    //  The crew that are in this room.
    //
    [SerializeField] protected GameObject[] crewList;

    //
    //  The lift attached to this room.
    //
    [SerializeField] protected GameObject   lift;

    //
    //  Assigned power to this room.
    //
    [SerializeField] protected int          assigned_power;

    //
    //  Start is called before
    //  the first frame update.
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

    //
    //  Initializes the room and
    //  places it "onto" the parent ship.
    //
    protected void initRoom()
    {
        owningShip.GetComponent<ShipScript>().addRoomToShip(this.gameObject);

        hp = max_hp;

        room_health_display = new GameObject[max_hp];

        RenderHealthBars();
    }

    public int getId()
    {
        return id;
    }

    public float getHp()
    {
        return hp;
    }

    public GameObject[] getCrewList()
    {
        return crewList;
    }

    public void addCrew(GameObject crew)
    {
        List<GameObject> temporary = new List<GameObject>(crewList);

        temporary.Add(crew);

        crewList = temporary.ToArray(); 
    }

    public void removeCrewById(int id)
    {
        List<GameObject> temporary = new List<GameObject>(crewList);

        crewList = temporary.FindAll(c => c.GetComponent<CrewScript>().getId() != id).ToArray();
    }

    //
    //  Renders in place the health bars
    //  on the top right of the room.
    //
    protected void RenderHealthBars()
    {
        float width = gameObject.GetComponent<SpriteRenderer>().sprite.rect.width;
        float height = gameObject.GetComponent<SpriteRenderer>().sprite.rect.height;

        width /= 50;
        height /= 50;

        for(int i = 0; i < max_hp; i++) {
            GameObject bar = GameObject.Instantiate(room_health_bar, gameObject.transform, false);

            bar.transform.position = new Vector2(
                bar.transform.position.x + width - (.15f * (1 + i)), 
                bar.transform.position.y + height - .22f
            );
            bar.GetComponent<SpriteRenderer>().color = Color.green;

            room_health_display[i] = bar;
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= Mathf.Abs(damage);
        float overflow;
        if (hp < 0)
        {
            //
            //  Save the "dip" into the negatives;
            //  we need this to assign hull damage.
            //
            overflow = hp;

            //
            //  Ensure that HP does not dip
            //  into negatives, otherwise
            //  bugs will occur.
            //
            hp = 0;

            owningShip.GetComponent<ShipScript>().TakeHullDamage(overflow);
        }

        int hp_floor = Mathf.FloorToInt(hp);

        for(int i = max_hp - 1; i >= hp_floor; i--)
        {
            room_health_display[i].GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void setAssignedPower(int power)
    {
        assigned_power = power;
    }

    public int getAssignedPower()
    {
        return assigned_power;
    }

    public GameObject getOwningShip()
    {
        return owningShip;
    }

    public GameObject getLift()
    {
        return lift;
    }

    public void attachLift(GameObject newLift)
    {
        lift = newLift;
    }
}
