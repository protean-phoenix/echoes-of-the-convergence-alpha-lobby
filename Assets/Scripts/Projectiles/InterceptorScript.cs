using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlightStatus
{
    on_approach = 0,
    encircling = 1
}

public class InterceptorScript : MonoBehaviour
{
    [SerializeField] private float speed;
    private float angle;
    [SerializeField] private GameObject target_ship;
    [SerializeField] private GameObject target_room;
    [SerializeField] private float reload_time;
    [SerializeField] private GameObject projectile;
    private float reload_timer = 0;
    private Vector3 start_pos;
    private FlightStatus status;
    private float flight_timer = 0;
    private float perim;
    
    // Start is called before the first frame update
    void Start()
    {
        status = FlightStatus.on_approach;
        float start_y = target_ship.transform.position.y + target_ship.GetComponent<ShipScript>().getFlightHeight();
        start_pos = new Vector3(target_ship.transform.position.x, start_y);

        angle = Utils.getAngleToInRadians(gameObject.transform.position, start_pos);
        gameObject.transform.eulerAngles = new Vector3(0, 0, angle * 180 / Mathf.PI);

        perim = Utils.getEllipsePerimeter(target_ship.transform.position.x, target_ship.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(status == FlightStatus.on_approach) { 
            gameObject.transform.position += new Vector3(speed * Mathf.Cos(angle) * Time.deltaTime, speed * Mathf.Sin(angle) * Time.deltaTime);

            if (Utils.getDistanceTo(gameObject.transform.position, start_pos) <= 0.1)
            {
                status = FlightStatus.encircling;
            }
        }
        else if (status == FlightStatus.encircling)
        {
            flight_timer += Time.deltaTime;
            float factor = flight_timer * speed * Mathf.PI * 2 / perim;
            float x_pos = target_ship.GetComponent<ShipScript>().getFlightWidth() * Mathf.Sin(factor) + target_ship.transform.position.x;
            float y_pos = target_ship.GetComponent<ShipScript>().getFlightHeight() * Mathf.Cos(factor) + target_ship.transform.position.y;
            gameObject.transform.position = new Vector3(x_pos, y_pos);

            float angle_to = Utils.getAngleToInRadians(gameObject.transform.position, target_ship.transform.position) * 180 / Mathf.PI;
            gameObject.transform.eulerAngles = new Vector3(0, 0, angle_to + 90);
        }
    }
}
