using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRoomScript : RoomScript
{
    [SerializeField]
    private GameObject temp_target;

    [SerializeField]
    private GameObject projectile;

    [SerializeField] private float reload_time = 10;
    private float reload_timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        reload_timer += Time.deltaTime;
        if(reload_timer >= reload_time)
        {
            fireWeapon(temp_target);
            reload_timer = 0;
        }
    }

    public void fireWeapon(GameObject target)
    {
        projectile.GetComponent<LaserScript>().fireLaser(this.gameObject, target);
    }
}
