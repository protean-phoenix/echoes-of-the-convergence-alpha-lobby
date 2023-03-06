using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileRoomScript : MonoBehaviour
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
        float target_x = target.gameObject.transform.position.x;
        float target_y = target.gameObject.transform.position.y;

        float origin_x = gameObject.transform.position.x;
        float origin_y = gameObject.transform.position.y;

        float diff_x = target_x - origin_x;
        float diff_y = target_y - origin_y;

        GameObject missile = GameObject.Instantiate(missile_projectile, new Vector3(origin_x, origin_y), Quaternion.identity);
        missile.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan(diff_y / diff_x) * 180 / Mathf.PI);
        
        missile.GetComponent<NonNetRocket>().SetAngle((Mathf.Atan(diff_y / diff_x) * 180 / Mathf.PI));
        missile.GetComponent<NonNetRocket>().SetTarget(temp_missile_target);
    }
}
