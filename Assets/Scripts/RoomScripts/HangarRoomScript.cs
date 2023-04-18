using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarRoomScript : WeaponRoomScript
{ 
    // Start is called before the first frame update
    void Start()
    {
        owningShip.GetComponent<ShipScript>().addRoomToShip(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
