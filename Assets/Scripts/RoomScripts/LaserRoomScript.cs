using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRoomScript : TargetedRoomScript
{
    // Start is called before the first frame update
    void Start()
    {
        
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
