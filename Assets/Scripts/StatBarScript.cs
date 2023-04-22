using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBarScript : MonoBehaviour
{
    [SerializeField] private GameObject unit; //the basic block of this stat bar
    private float bar_amount;
    private float desired_width;
    private float desired_height;
    private GameObject[] blocks; //the individual blocks of the stat bar


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void renderBar(int amount)
    {
        blocks = new GameObject[amount];
        bar_amount = amount;
        for(int i = 0; i < amount; i++)
        {
            
            desired_width = gameObject.GetComponent<RectTransform>().sizeDelta.x / amount;
            desired_height = gameObject.GetComponent<RectTransform>().sizeDelta.y;
            GameObject block = GameObject.Instantiate(unit, gameObject.transform, false);
            block.transform.localPosition = new Vector2(desired_width * (i + .5f), 0);
            block.transform.localScale = new Vector2(desired_width*0.9f, desired_height);
            blocks[i] = block;
        }
    }

    //Always decreases the bar
    public void adjustBar(float adjustment)
    {
        bar_amount += adjustment;
        int amount_floor = Mathf.FloorToInt(bar_amount);
        if(bar_amount < 0 )
        {
            bar_amount = 0;
        }
        if(amount_floor < 0)
        {
            amount_floor = 0;
        }
        for (int i = blocks.Length - 1; i > amount_floor; i--)
        {
            if(blocks[i] != null)
            {
                GameObject.Destroy(blocks[i]);
            }
        }
        float remainder = bar_amount - amount_floor;
        if(remainder <= 0.01f)
        {
            if(blocks[amount_floor] != null) { 
                GameObject.Destroy(blocks[amount_floor]);
            }
        }
        else
        {
            GameObject frac_block = blocks[amount_floor];
            frac_block.transform.localScale = new Vector2(desired_width * 0.9f * remainder, desired_height);
            frac_block.transform.localPosition = new Vector2(desired_width * (amount_floor + 0.5f) - ((1-remainder) * desired_width * 0.9f / 2), 0);
        }
    }
}
