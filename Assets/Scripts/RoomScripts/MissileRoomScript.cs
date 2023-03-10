using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileRoomScript : RoomScript
{
    [SerializeField]
    private GameObject temp_missile_target;

    [SerializeField]
    private GameObject missile_projectile;

    [SerializeField] private float reload_time = 2;
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
            fireWeapon(temp_missile_target);
            reload_timer = 0;
        }
        
    }
    public void fireWeapon(GameObject target)
    {
        GameObject missile = GameObject.Instantiate(missile_projectile, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
        float angle_to = Utils.getAngleToInRadians(gameObject.transform.position, target.transform.position) * 180 / Mathf.PI;
        missile.transform.eulerAngles = new Vector3(0, 0, angle_to);
        
        missile.GetComponent<NonNetRocket>().SetAngle(angle_to);
        missile.GetComponent<NonNetRocket>().SetTarget(temp_missile_target);
    }
}
