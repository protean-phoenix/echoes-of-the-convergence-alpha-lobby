using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    protected float angle;
    protected int id;
    protected static int count = 0;
    [SerializeField] protected float velocity;
    protected GameObject destination;
    [SerializeField] protected float sys_damage;
    protected GameObject source_ship;
    protected float timer;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void initMissile(GameObject target, GameObject ship)
    {        
        id = count;
        count++;

        destination = target;
        source_ship = ship;

        timer = Utils.getDistanceTo(gameObject.transform.position, destination.transform.position) / velocity;

        source_ship.GetComponent<ShipScript>().registerMissile(this.gameObject);

        this.angle = Utils.getAngleToInRadians(gameObject.transform.position, destination.transform.position);

        //byte serialization to send to client
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        byte[] b_ox = BitConverter.GetBytes(x);
        byte[] b_oy = BitConverter.GetBytes(y);
        x = target.transform.position.x;
        y = target.transform.position.y;
        byte[] b_dx = BitConverter.GetBytes(x);
        byte[] b_dy = BitConverter.GetBytes(y);
        byte[] packet = new byte[19];
        packet[0] = 0;
        packet[1] = 1;
        packet[2] = (byte)id;
        b_ox.CopyTo(packet, 3);
        b_oy.CopyTo(packet, 7);
        b_dx.CopyTo(packet, 11);
        b_dy.CopyTo(packet, 15);
        NetworkManager.broadcast(packet);
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

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            if (destination.GetComponent<RoomScript>().getOwningShip().GetComponent<ShipScript>().rollDodge())
            {
                GameObject text = GameObject.Instantiate(GameObject.Find("EffectsManager").GetComponent<EffectsManagerScript>().effects[0]);
                text.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            }
            else
            { 
                destination.GetComponent<RoomScript>().TakeDamage(sys_damage);
            }
            destroy();
        }
    }

    public void fireMissile(GameObject origin, GameObject target)
    {
        float origin_x = origin.transform.position.x;
        float origin_y = origin.transform.position.y;

        GameObject missile = GameObject.Instantiate(this.gameObject, new Vector3(origin_x, origin_y), Quaternion.identity);
       
        angle = Utils.getAngleToInRadians(origin.transform.position, target.transform.position);
        missile.GetComponent<MissileScript>().initMissile(target, origin.GetComponent<MissileRoomScript>().getOwningShip());
        missile.transform.eulerAngles = new Vector3(0, 0, angle * 180 / Mathf.PI);
    }

    public float getSysDamage()
    {
        return sys_damage;
    }
}
