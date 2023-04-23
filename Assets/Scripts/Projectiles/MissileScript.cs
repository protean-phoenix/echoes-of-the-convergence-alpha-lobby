using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    protected float angle;
    [SerializeField] protected float velocity;
    protected GameObject destination;
    [SerializeField] protected float sys_damage;
    protected GameObject source_ship;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void initMissile(GameObject target, GameObject ship, float angle)
    {
        destination = target;
        source_ship = ship;

        source_ship.GetComponent<ShipScript>().registerMissile(this.gameObject);

        this.angle = angle;
    }

    // Update is called once per frame
    void Update()
    {
        updateMissile();
    }

    public void destroy()
    {
        source_ship.GetComponent<ShipScript>().deregisterMissile(this.gameObject);
        GameObject.Destroy(this.gameObject);
    }

    protected void updateMissile()
    {
        gameObject.transform.position += new Vector3(velocity * Mathf.Cos(angle) * Time.deltaTime, velocity * Mathf.Sin(angle) * Time.deltaTime);

        if(Utils.getDistanceTo(gameObject.transform.position, destination.transform.position) <= 0.05f)
        {
            destination.GetComponent<RoomScript>().TakeDamage(sys_damage);
            destroy();
        }

        //byte serialization to send to client
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        byte[] b_ax = BitConverter.GetBytes(x);
        byte[] b_ay = BitConverter.GetBytes(y);
        byte[] b_comb = new byte[b_ax.Length + b_ay.Length];
        b_ax.CopyTo(b_comb, 0);
        b_ay.CopyTo(b_comb, b_ax.Length);
    }

    public void fireMissile(GameObject origin, GameObject target)
    {
        float origin_x = origin.transform.position.x;
        float origin_y = origin.transform.position.y;

        GameObject missile = GameObject.Instantiate(this.gameObject, new Vector3(origin_x, origin_y), Quaternion.identity);
       
        angle = Utils.getAngleToInRadians(origin.transform.position, target.transform.position);
        missile.GetComponent<MissileScript>().initMissile(target, origin.GetComponent<MissileRoomScript>().getOwningShip(), angle);
        missile.transform.eulerAngles = new Vector3(0, 0, angle * 180 / Mathf.PI);
    }

    public float getSysDamage()
    {
        return sys_damage;
    }
}
