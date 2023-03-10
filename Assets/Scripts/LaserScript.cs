using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField] private float residual = 1;
    private float res_timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        res_timer += Time.deltaTime;
        if(res_timer >= residual)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    public GameObject fireLaser(GameObject origin, GameObject target)
    {
        float target_x = target.gameObject.transform.position.x;
        float target_y = target.gameObject.transform.position.y;

        float origin_x = origin.transform.position.x;
        float origin_y = origin.transform.position.y;

        float mid_x = (target_x + origin_x) / 2;
        float mid_y = (target_y + origin_y) / 2;

        float diff_x = origin_x - target_x;
        float diff_y = origin_y - target_y;

        float scale = Mathf.Sqrt(Mathf.Pow(diff_x, 2) + Mathf.Pow(diff_y, 2));

        GameObject laser = GameObject.Instantiate(this.gameObject, new Vector3(mid_x, mid_y), Quaternion.identity);

        laser.transform.localScale = new Vector3(scale, 1, 1);
        laser.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan(diff_y / diff_x) * 180 / Mathf.PI);
        
        return laser;
    }
}
