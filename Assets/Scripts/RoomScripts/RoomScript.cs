using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    [SerializeField] protected GameObject owningShip;
    [SerializeField] protected int id;
    [SerializeField] protected int max_hp;
    protected float hp;
    protected GameObject[] room_health_display;
    [SerializeField] protected GameObject room_health_bar;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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

    //renders in place the health bars on the top right of the room
    protected void RenderHealthBars()
    {
        float width = gameObject.GetComponent<SpriteRenderer>().sprite.rect.width;
        float height = gameObject.GetComponent<SpriteRenderer>().sprite.rect.height;
        width /= 50;
        height /= 50;
        for(int i = 0; i < max_hp; i++) {
            GameObject bar = GameObject.Instantiate(room_health_bar, gameObject.transform, false);
            bar.transform.position = new Vector2(bar.transform.position.x + width - (.15f * (1 + i)), bar.transform.position.y + height - .22f);
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
            overflow = hp; //save the dip into the negatives. We need this to assign hull damage
            hp = 0; //ensure that hp does not dip into negatives, otherwise bugs will occur
            owningShip.GetComponent<ShipScript>().TakeHullDamage(overflow);
        }
        int hp_floor = Mathf.FloorToInt(hp);
        for(int i = max_hp - 1; i >= hp_floor; i--)
        {
            room_health_display[i].GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public GameObject getOwningShip()
    {
        return owningShip;
    }
}
