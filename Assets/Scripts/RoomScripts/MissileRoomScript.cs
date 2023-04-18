using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileRoomScript : WeaponRoomScript
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
            //fireWeapon(target);
            reload_timer = 0;
        }
        
    }
    public void fireWeapon(GameObject target)
    {
        GameObject missile = GameObject.Instantiate(projectile, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
        float angle_to = Utils.getAngleToInRadians(gameObject.transform.position, target.transform.position) * 180 / Mathf.PI;
        missile.transform.eulerAngles = new Vector3(0, 0, angle_to);
        
        missile.GetComponent<NonNetRocket>().SetAngle(angle_to);
        missile.GetComponent<NonNetRocket>().SetTarget(target);
    }
}
