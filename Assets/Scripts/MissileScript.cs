using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        byte[] b_ax = BitConverter.GetBytes(x);
        byte[] b_ay = BitConverter.GetBytes(y);
        byte[] b_comb = new byte[b_ax.Length + b_ay.Length];
        b_ax.CopyTo(b_comb, 0);
        b_ay.CopyTo(b_comb, b_ax.Length);
        NetworkManager.Enqueue(b_comb);
    }
}
