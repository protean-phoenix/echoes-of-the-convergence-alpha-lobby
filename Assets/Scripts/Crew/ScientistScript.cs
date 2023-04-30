using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistScript : CrewScript
{
    //
    //  Start is called before
    //  the initial frame update.
    //
    void Start()
    {
        crewType = CrewType.Scientist;

        initCrew();
    }

    //
    //  Update is called
    //  once per frame.
    //
    void Update()
    {
        //
        //  This is for testing
        //  purposes only!
        //
        if (appointedRoom == null)
        {
            List<GameObject> available_reactors = owningShip.GetComponent<ShipScript>().getReactorsList();

            if (available_reactors.Count > 0)
                assignRoom(available_reactors[0]);
        }

        if (appointedRoom != null && !isInAssignedRoom())
            updateCrewLocation();
    }
}