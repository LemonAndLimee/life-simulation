using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCycle : MonoBehaviour
{
    public SpawningLogic spawnScript;
    public BlobSpawn blobSpawnScript;

    public int days;
    public Text dayText;

    public float dayTime;
    public float waitTime;

    public float timer;

    public bool isDay;

    // Start is called before the first frame update
    void Start()
    {
        days = 0;
        isDay = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        dayText.text = days.ToString() + " Day/s";

        if (timer >= dayTime && isDay == true)
        {
            days += 1;
            timer = 0;
            isDay = false;
            spawnScript.Destroy();
            if (days <= 1)
            {
                blobSpawnScript.EndDay(true);
            }
            else
            {
                blobSpawnScript.EndDay(false);
            }
        }
        else if (timer >= waitTime && isDay == false)
        {
            timer = 0;
            isDay = true;
            spawnScript.Spawn();
        }
    }
}
