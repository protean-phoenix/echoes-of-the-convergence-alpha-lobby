using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorScript : MonoBehaviour
{
	//
	//	The parent ship this corridor
	//	is a part of.
	//
	[SerializeField] protected GameObject owningShip;

	//
	//	The two rooms this corridor
	//	is connecting.
	//
	[SerializeField] protected (GameObject, GameObject) connectedRooms;


	//
	//	Start is called before
	//	the initial frame update.
	//
	void Start()
	{

	}

	//
	//	Update is called
	//	once per frame.
	//
	void Update()
	{
		
	}
}

