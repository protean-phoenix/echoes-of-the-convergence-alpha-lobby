using System;
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

        int shieldLayer = 0;
        if(origin.GetComponent<LaserRoomScript>().getOwningShip().GetComponent<ShipScript>().getId() == 0)
        {
            shieldLayer = 7;
        }else if (origin.GetComponent<LaserRoomScript>().getOwningShip().GetComponent<ShipScript>().getId() == 1)
        {
            shieldLayer = 6;
        }

            RaycastHit2D hit = Physics2D.Raycast(origin.transform.position, ((Vector2)(target.transform.position - origin.transform.position)).normalized, Mathf.Infinity, 1 << shieldLayer);
        if (hit.collider != null) //this means that an enemy shield was detected in the path of the laser
        {
            target_x = hit.point.x;
            target_y = hit.point.y;
            //Enemy Ship takes shield damage
            target.GetComponent<RoomScript>().getOwningShip().GetComponent<ShipScript>().TakeShieldDamage(origin.GetComponent<LaserRoomScript>().getShlDamage());
        }
        else
        {
            //Room takes system damage
            target.GetComponent<RoomScript>().TakeDamage(origin.GetComponent<LaserRoomScript>().getSysDamage());
        }

        float origin_x = origin.transform.position.x;
        float origin_y = origin.transform.position.y;

        float mid_x = (target_x + origin_x) / 2;
        float mid_y = (target_y + origin_y) / 2;

        float diff_x = origin_x - target_x;
        float diff_y = origin_y - target_y;

        //scale is length of the laser
        float scale = Mathf.Sqrt(Mathf.Pow(diff_x, 2) + Mathf.Pow(diff_y, 2));
        float angle = Mathf.Atan2(diff_y, diff_x) * 180 / Mathf.PI;

        GameObject laser = GameObject.Instantiate(this.gameObject, new Vector3(mid_x, mid_y), Quaternion.identity);

        laser.transform.localScale = new Vector3(scale, 1, 1);
        laser.transform.eulerAngles = new Vector3(0, 0, angle);

        

        byte[] packet = new byte[18];
        packet[0] = 0;
        packet[1] = 0;
        Array.Copy(BitConverter.GetBytes(mid_x), 0, packet, 2, 4);
        Array.Copy(BitConverter.GetBytes(mid_y), 0, packet, 6, 4);
        Array.Copy(BitConverter.GetBytes(scale), 0, packet, 10, 4);
        Array.Copy(BitConverter.GetBytes(angle), 0, packet, 14, 4);
        NetworkManager.broadcast(packet);

        return laser;
    }
}
