using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileRoomScript : WeaponRoomScript
{ 
    // Start is called before the first frame update
    void Start()
    {
        initRoom();
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
        if (NetworkManager.started)
        {
            projectile.GetComponent<MissileScript>().fireMissile(gameObject, target);
        }
    }
}
