using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCycleARB : MonoBehaviour
{
    public SpawningLogicARB spawnScript;
    public BacteriaSpawn bacSpawnScript;

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
                bacSpawnScript.EndDay(true);
            }
            else
            {
                bacSpawnScript.EndDay(false);
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
