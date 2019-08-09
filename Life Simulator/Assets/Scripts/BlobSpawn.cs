using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlobSpawn : MonoBehaviour
{
    public DayCycle dayScript;

    public GameObject blobPrefab;
    public GameObject currentObject;

    public int blobNumber;

    public int x;
    public int z;

    public Text originalBlobText;
    public Text blobText;

    public Text speedText;
    public Text initialSpeedText;
    public float totalSpeed;
    public float averageSpeed;

    public float minSpeed;
    public float maxSpeed;
    public Text minSpeedText;
    public Text maxSpeedText;

    public float initialSpeed;

    public float initialRange;
    public float minRange;
    public float maxRange;
    public float averageRange;
    public float totalRange;

    public Text initialRangeText;
    public Text rangeText;
    public Text minRangeText;
    public Text maxRangeText;

    public float initialImmunity;
    public float minImmunity;
    public float maxImmunity;
    public float averageImmunity;
    public float totalImmunity;

    public Text initialImmunityText;
    public Text minImmunityText;
    public Text maxImmunityText;
    public Text immunityText;

    public float totalAcquired;
    public float averageAcquired;

    public Text acquiredImmunityText;

    public int blobCount;

    // Start is called before the first frame update
    void Start()
    {
        StartLogic startScript = GameObject.Find("StartManager").GetComponent<StartLogic>();
        blobNumber = startScript.blobs;
        initialSpeed = (float)startScript.initialSpeed;
        initialRange = startScript.range;
        initialImmunity = startScript.immunity;

        for (int i = 1; i <= blobNumber; i++)
        {

            currentObject = Instantiate(blobPrefab);

            BlobLogic blobScript = currentObject.GetComponent<BlobLogic>();
            blobScript.speed = initialSpeed;
            blobScript.range = (int)initialRange;
            blobScript.immunity = initialImmunity;

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

        initialSpeedText.text = "Initial Speed: " + initialSpeed.ToString();

        initialRangeText.text = "Initial Detection Range: " + initialRange.ToString();

        initialImmunityText.text = "Initial Immunity: " + initialImmunity.ToString();

        averageSpeed = initialSpeed;

        averageRange = initialRange;

        averageImmunity = initialImmunity;
    }

    // Update is called once per frame
    void Update()
    {
        minSpeed = averageSpeed;
        maxSpeed = averageSpeed;

        minRange = averageRange;
        maxRange = averageRange;

        minImmunity = averageImmunity;
        maxImmunity = averageImmunity;

        blobCount = 0;
        GameObject[] blobs = GameObject.FindGameObjectsWithTag("Blob");
        foreach(GameObject blob in blobs)
        {
            blobCount += 1;
            BlobLogic blobscript = blob.GetComponent<BlobLogic>();
            totalSpeed += blobscript.speed;
            totalRange += blobscript.range;
            totalImmunity += blobscript.immunity;
            totalAcquired += blobscript.acquiredImmunity;

            if (blobscript.speed < minSpeed)
            {
                minSpeed = blobscript.speed;
            }
            else if (blobscript.speed > maxSpeed)
            {
                maxSpeed = blobscript.speed;
            }

            if ((float)blobscript.range < (float)minRange)
            {
                minRange = (float)blobscript.range;
            }
            else if ((float)blobscript.range > (float)maxRange)
            {
                maxRange = (float)blobscript.range;
            }

            if (blobscript.immunity < minImmunity)
            {
                minImmunity = blobscript.immunity;
            }
            else if (blobscript.immunity > maxImmunity)
            {
                maxImmunity = blobscript.immunity;
            }

            if (dayScript.isDay == false)
            {
                blobscript.detectedFood = false;
            }
        }
        blobText.text = blobCount.ToString() + " Blobs";


        averageSpeed = totalSpeed / blobCount;
        totalSpeed = 0;
        speedText.text = "Average Speed: " + averageSpeed.ToString();

        minSpeedText.text = "Minimum Speed: " + minSpeed.ToString();
        maxSpeedText.text = "Maximum Speed: " + maxSpeed.ToString();


        averageRange = (float)totalRange / (float)blobCount;
        totalRange = 0;
        rangeText.text = "Average Detection Range: " + averageRange.ToString();

        minRangeText.text = "Minimum Detection Range: " + minRange.ToString();
        maxRangeText.text = "Maximum Detection Range: " + maxRange.ToString();

        averageImmunity = totalImmunity / blobCount;
        totalImmunity = 0;
        immunityText.text = "Average Immunity: " + averageImmunity.ToString() + "%";

        minImmunityText.text = "Minimum Immunity: " + minImmunity.ToString() + "%";
        maxImmunityText.text = "Maximum Immunity: " + maxImmunity.ToString() + "%";

        averageAcquired = totalAcquired / blobCount;
        acquiredImmunityText.text = "Average Acquired Immunity: " + averageAcquired.ToString() + "%";
        
        
    }

    public void EndDay(bool isTrue)
    {
        GameObject[] blobs = GameObject.FindGameObjectsWithTag("Blob");
        foreach (GameObject blob in blobs)
        {
            BlobLogic blobscript = blob.GetComponent<BlobLogic>();
            if (isTrue == true)
            {
                blobscript.EndDay(true);
            }
            else
            {
                blobscript.EndDay(false);
            }
        }
    }

    public void AddDisease(int infect, int lethal)
    {
        GameObject[] blobs = GameObject.FindGameObjectsWithTag("Blob");
        int index = 0;
        foreach (GameObject blob in blobs)
        {
            if (index == Random.Range(0, (blobCount)))
            {
                BlobLogic blobScript = blob.GetComponent<BlobLogic>();
                
                blobScript.Infect(infect, lethal, true);

            }
            index += 1;
        }
    }
}
