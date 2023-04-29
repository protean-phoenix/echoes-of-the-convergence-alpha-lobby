using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineScript : RoomScript
{
    [SerializeField] private float max_dodge;
    private float dodge;

    //
    //  Start is called before
    //  the first frame update.
    //
    void Start()
    {
        initRoom();

        dodge = max_dodge;
        owningShip.GetComponent<ShipScript>().recalculateDodge();
    }

    //
    //  Update is called
    //  once per frame.
    //
    void Update()
    {
        
    }

    public float getDodge()
    {
        return dodge;
    }

    public new void TakeDamage(float damage)
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

        for (int i = max_hp - 1; i >= hp_floor; i--)
        {
            room_health_display[i].GetComponent<SpriteRenderer>().color = Color.red;
        }

        dodge = max_dodge * max_hp / hp_floor;
        owningShip.GetComponent<ShipScript>().recalculateDodge();
    }
}
