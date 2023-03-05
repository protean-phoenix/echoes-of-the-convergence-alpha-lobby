using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonNetRocket : MonoBehaviour
{
    [SerializeField] private float angle;
    [SerializeField] private float velocity;
    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        velocity = 5;
    }
    // Update is called once per frame
    void Update()
    {
         gameObject.transform.position += new Vector3(velocity * Mathf.Cos(angle/180 * Mathf.PI) * Time.deltaTime, velocity * Mathf.Sin(angle/180 * Mathf.PI) * Time.deltaTime);

        float target_x = target.gameObject.transform.position.x;
        float target_y = target.gameObject.transform.position.y;

        float origin_x = gameObject.transform.position.x;
        float origin_y = gameObject.transform.position.y;

        float diff_x = target_x - origin_x;
        float diff_y = target_y - origin_y;
        
        float TheDist = Mathf.Sqrt(Mathf.Pow(diff_x, 2) + Mathf.Pow(diff_y, 2));
        if(TheDist <= 0.1){
            GameObject.Destroy(this.gameObject);
        }
    }

    public float GetAngle(){
        return angle;
    }
    public void SetAngle(float angle){
        this.angle = angle;
    }

    public GameObject GetTarget(){
        return target;
    }
    public void SetTarget(GameObject target){
        this.target = target;
    }
}
