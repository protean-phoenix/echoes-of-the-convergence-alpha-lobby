using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarRoomScript : WeaponRoomScript
{ 
    // Start is called before the first frame update
    void Start()
    {
        initRoom();
    }

    // Update is called once per frame
    void Update()
    {
        reload_timer += Time.deltaTime * assigned_power / max_hp;
        if (reload_timer >= reload && target != null)
        {
            reload_timer = 0;
            fireWeapon(target);
        }
    }

    public void fireWeapon(GameObject target)
    {
        if (NetworkManager.started)
        {
            GameObject craft_obj = GameObject.Instantiate(projectile, gameObject.transform, false);
            CraftScript craft = craft_obj.GetComponent<CraftScript>();
            craft.setTargetShip(target.GetComponent<RoomScript>().getOwningShip());
            craft.setParentShip(owningShip);
            craft.initCraft(owningShip, gameObject);
        }
    }
}
