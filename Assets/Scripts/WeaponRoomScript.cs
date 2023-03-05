using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRoomScript : MonoBehaviour
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
        float target_x = target.gameObject.transform.position.x;
        float target_y = target.gameObject.transform.position.y;

        float origin_x = gameObject.transform.position.x;
        float origin_y = gameObject.transform.position.y;

        float mid_x = (target_x + origin_x) / 2;
        float mid_y = (target_y + origin_y) / 2;

        //this is inverted, flip the origin and target around 
        float diff_x = origin_x - target_x;
        float diff_y = origin_y - target_y;

        float scale = Mathf.Sqrt(Mathf.Pow(diff_x, 2) + Mathf.Pow(diff_y, 2));

        GameObject laser = GameObject.Instantiate(projectile, new Vector3(mid_x, mid_y), Quaternion.identity);

        laser.transform.localScale = new Vector3(scale, 1, 1);
        laser.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan(diff_y / diff_x) * 180 / Mathf.PI);
    }
}
