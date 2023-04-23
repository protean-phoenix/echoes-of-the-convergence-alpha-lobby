using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlightStatus
{
    on_approach = 0,
    encircling = 1
}

public class CraftScript : MonoBehaviour
{
    [SerializeField] protected float speed;
    protected float angle;
    protected GameObject target_ship;
    protected GameObject host_room;
    protected GameObject parent_ship;
    [SerializeField] protected float reload_time;
    [SerializeField] protected GameObject projectile;
    protected float reload_timer = 0;
    protected Vector3 start_pos;
    protected FlightStatus status;
    protected float flight_timer = 0;
    protected float perim;
    [SerializeField] protected float sys_damage;
    [SerializeField] protected float shl_damage;
    protected GameObject source_ship;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initCraft(GameObject ship, GameObject room)
    {
        source_ship = ship;
        source_ship.GetComponent<ShipScript>().registerCraft(this.gameObject);

        host_room = room;

        status = FlightStatus.on_approach;
        float start_y = target_ship.transform.position.y + target_ship.GetComponent<ShipScript>().getFlightHeight();
        start_pos = new Vector3(target_ship.transform.position.x, start_y);

        angle = Utils.getAngleToInRadians(gameObject.transform.position, start_pos);
        gameObject.transform.eulerAngles = new Vector3(0, 0, angle * 180 / Mathf.PI);

        perim = Utils.getEllipsePerimeter(target_ship.transform.position.x, target_ship.transform.position.y);
    }

    public void destroy() {
        source_ship.GetComponent<ShipScript>().deregisterCraft(this.gameObject);
        GameObject.Destroy(gameObject);
    }

    protected void updateCraft()
    {
        if (status == FlightStatus.on_approach)
        {
            gameObject.transform.position += new Vector3(speed * Mathf.Cos(angle) * Time.deltaTime, speed * Mathf.Sin(angle) * Time.deltaTime);

            if (Utils.getDistanceTo(gameObject.transform.position, start_pos) <= 0.1)
            {
                status = FlightStatus.encircling;
            }
        }
        else if (status == FlightStatus.encircling)
        {
            flight_timer += Time.deltaTime;
            float factor = flight_timer * speed * Mathf.PI / perim;

            //calculate the x and y position of the craft and set it to those coordinates
            float x_pos = target_ship.GetComponent<ShipScript>().getFlightWidth() * Mathf.Sin(factor) + target_ship.transform.position.x;
            float y_pos = target_ship.GetComponent<ShipScript>().getFlightHeight() * Mathf.Cos(factor) + target_ship.transform.position.y;
            gameObject.transform.position = new Vector3(x_pos, y_pos);

            //calculate the angle to the target ship and set it perpendicular
            float angle_to = Utils.getAngleToInRadians(gameObject.transform.position, target_ship.transform.position) * 180 / Mathf.PI;
            int flip = 1;
            if (source_ship.GetComponent<ShipScript>().getId() == 1) flip = - 1;
            gameObject.transform.eulerAngles = new Vector3(0, 0, angle_to + (90 * flip));
            
            reload_timer += Time.deltaTime;
            if(reload_timer >= reload_time)
            {
                GameObject target_room = host_room.GetComponent<HangarRoomScript>().getTarget();
                projectile.GetComponent<LaserScript>().fireLaser(gameObject, target_room);
                reload_timer = 0;
            }
        }
    }

    public GameObject getTargetShip()
    {
        return target_ship;
    }

    public void setTargetShip(GameObject ship)
    {
        this.target_ship = ship;
    }

    public GameObject getHostRoom()
    {
        return host_room;
    }

    public GameObject getParentShip()
    {
        return parent_ship;
    }

    public void setParentShip(GameObject ship)
    {
        parent_ship = ship;
    }

    public float getSysDamage()
    {
        return sys_damage;
    }

    public float getShlDamage()
    {
        return shl_damage;
    }
}
