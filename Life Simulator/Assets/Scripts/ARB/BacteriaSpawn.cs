using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//spawns in the initial batch
//monitors stats

public class BacteriaSpawn : MonoBehaviour
{
    public DayCycleARB dayScript;

    public GameObject blobPrefab;
    public GameObject currentObject;


    public int blobNumber;
    public int speed;
    public int range;

    public int x;
    public int z;

    public Text originalBlobText;
    public Text blobText;

    public Text speedText;
    public Text rangeText;

    public float minResistance;
    public float maxResistance;
    public float totalResistance;
    public float averageResistance;

    public Text resText;
    public Text minResText;
    public Text maxResText;

    public int daysLeft;
    public bool courseOn;
    public int courseDays;


    public int blobCount;

    public bool currentDestroyed;

    // Start is called before the first frame update
    void Start()
    {

        courseOn = false;
        currentDestroyed = false;
        
        for (int i = 1; i <= blobNumber; i++)
        {

            currentObject = Instantiate(blobPrefab);

            BacteriaLogic blobScript = currentObject.GetComponent<BacteriaLogic>();
            blobScript.speed = speed;
            blobScript.range = (int)range;
            int rand = Random.Range((int)minResistance, (int)maxResistance);
            blobScript.resistance = rand;

            int ran = Random.Range(0, 2);
            if (ran == 0)
            {
                ran = Random.Range(0, 2);
                if (ran == 0)
                {
                    x = 45;
                    z = Random.Range(45, -46);
                }
                else
                {
                    x = -45;
                    z = Random.Range(45, -46);
                }
            }
            else
            {
                ran = Random.Range(0, 2);
                if (ran == 0)
                {
                    z = 45;
                    x = Random.Range(45, -46);
                }
                else
                {
                    z = -45;
                    x = Random.Range(45, -46);
                }
            }

            currentObject.transform.position = new Vector3(x, 2.5f, z);
        }

        originalBlobText.text = "Original Count: " + blobNumber.ToString();
        speedText.text = "Speed: " + speed.ToString();
        rangeText.text = "Range: " + range.ToString();

        

    }

    // Update is called once per frame
    void Update()
    {
        minResistance = 101;
        maxResistance = 0;
        
        blobCount = 0;
        GameObject[] blobs = GameObject.FindGameObjectsWithTag("Blob");
        foreach (GameObject blob in blobs)
        {
            
            blobCount += 1;
            BacteriaLogic blobscript = blob.GetComponent<BacteriaLogic>();
            totalResistance += blobscript.resistance;

            
            if (blobscript.resistance < minResistance)
            {
                minResistance = blobscript.resistance;
            }
            else if (blobscript.resistance > maxResistance)
            {
                maxResistance = blobscript.resistance;
                
            }

            if (dayScript.isDay == false)
            {
                blobscript.detectedFood = false;
            }
        }
        blobText.text = blobCount.ToString() + " Blobs";
        averageResistance = totalResistance / blobCount;
        
        totalResistance = 0;
        resText.text = "Resistance: " + averageResistance.ToString();
        minResText.text = "Min Resistance: " + minResistance.ToString();
        maxResText.text = "Max Resistance: " + maxResistance.ToString();
        
        if (daysLeft <= 0)
        {
            courseOn = false;
        }

       }

    public void EndDay(bool isTrue)
    {
        GameObject[] blobs = GameObject.FindGameObjectsWithTag("Blob");
        foreach (GameObject blob in blobs)
        {
            BacteriaLogic blobscript = blob.GetComponent<BacteriaLogic>();

            if (isTrue == true)
            {
                //early days
                blobscript.EndDay(true, courseOn);
            }
            else
            {
                //later
                //Debug.Log("before");
                blobscript.EndDay(false, courseOn);
                //Debug.Log("ended");
            }

            //Debug.Log(blobscript.enabled);

        }
        if (courseOn == true)
        {
            Debug.Log("wave");
            daysLeft -= 1;
            Debug.Log(daysLeft);
        }


    }

    public void StartCourse(int days) {

        courseOn = true;
        daysLeft = days;

    }

    public void TriggerCourse()
    {
        StartCourse(courseDays);
    }



}
