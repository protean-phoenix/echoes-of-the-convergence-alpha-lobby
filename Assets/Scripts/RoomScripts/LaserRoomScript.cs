using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRoomScript : WeaponRoomScript
{
    // Start is called before the first frame update
    void Start()
    {
        owningShip.GetComponent<ShipScript>().addRoomToShip(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        reload_timer += Time.deltaTime;
        if(reload_timer >= reload && target != null)
        {
            fireWeapon(target);
            reload_timer = 0;
        }
    }

    public void fireWeapon(GameObject target)
    {
        if (NetworkManager.started) { 
            projectile.GetComponent<LaserScript>().fireLaser(this.gameObject, target);
        }
    }
}
