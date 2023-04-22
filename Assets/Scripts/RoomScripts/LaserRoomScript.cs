using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRoomScript : WeaponRoomScript
{
    [SerializeField] private float sys_damage; //damage this laser room does to rooms
    [SerializeField] private float shl_damage; //damage this laser room does to shields
    // Start is called before the first frame update
    void Start()
    {
        initRoom();
    }

    // Update is called once per frame
    void Update()
    {
        reload_timer += (Time.deltaTime * hp / max_hp);
        if(reload_timer >= reload && target != null)
        {
            reload_timer = 0;
            fireWeapon(target);
        }
    }

    public void fireWeapon(GameObject target)
    {
        if (NetworkManager.started) { 
            projectile.GetComponent<LaserScript>().fireLaser(gameObject, target);
        }
    }

    public float getSysDamage()
    {
        return sys_damage;
    }

    public float getShlDamage()
    {
        return shl_damage;
    }
}
