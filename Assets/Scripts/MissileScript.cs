using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    [SerializeField] private float angle;
    [SerializeField] private float velocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += new Vector3(velocity * Mathf.Cos(angle) * Time.deltaTime, velocity * Mathf.Sin(angle) * Time.deltaTime);
    }
}
