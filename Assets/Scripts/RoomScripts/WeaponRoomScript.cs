using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRoomScript : RoomScript
{
    [SerializeField] protected GameObject   target;
    [SerializeField] protected GameObject   projectile;
    [SerializeField] protected float        reload;
                     protected float        reload_timer;

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

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public GameObject getTarget()
    {
        return target;
    }
}
