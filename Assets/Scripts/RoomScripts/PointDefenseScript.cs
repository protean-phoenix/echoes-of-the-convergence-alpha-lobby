using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointDefenseScript : RoomScript
{
    [SerializeField] private float reload;
    [SerializeField] private GameObject projectile;
    private float reload_timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        initRoom();
    }

    // Update is called once per frame
    void Update()
    {
        reload_timer += Time.deltaTime * assigned_power / max_hp;

        if (reload_timer >= reload )
        {
            GameObject target = null;
            target = findClosestTarget();
            if(target != null) { 
                reload_timer = 0;
                fireWeapon(target);
            }
        }
    }

    public void fireWeapon(GameObject target)
    {
        if (NetworkManager.started)
        {
            projectile.GetComponent<PointDefenseLaserScript>().fireLaser(gameObject, target);
        }
    }

    public GameObject findClosestTarget()
    {
        GameObject target = null;
        GameObject enemy_ship = ShipScript.getEnemyShip(owningShip);
        List<GameObject> enemy_missiles = enemy_ship.GetComponent<ShipScript>().getMissileList();
        foreach (GameObject obj in enemy_missiles)
        {
            if (target == null || Utils.getDistanceTo(gameObject.transform.position, obj.transform.position) < Utils.getDistanceTo(gameObject.transform.position, target.transform.position))
            {
                target = obj;
            }
        }
        List<GameObject> enemy_craft = enemy_ship.GetComponent<ShipScript>().getCraftList();
        foreach (GameObject obj in enemy_craft)
        {
            if (target == null || Utils.getDistanceTo(gameObject.transform.position, obj.transform.position) < Utils.getDistanceTo(gameObject.transform.position, target.transform.position))
            {
                target = obj;
            }
        }
        return target;
    }
}
