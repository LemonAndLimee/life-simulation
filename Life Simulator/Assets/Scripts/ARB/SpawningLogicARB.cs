using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//spawns food every day
//destroys food at end of each day
//allows editing of food amounts

public class SpawningLogicARB : MonoBehaviour
{ 
    public DayCycleARB dayScript;

    public int foodNumber;

    public GameObject foodPrefab;
    public GameObject currentObject;

    public Text foodText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        foodText.text = foodNumber.ToString() + " Food/Day";
    }

    public void Spawn()
    {
        for (int i = 1; i <= foodNumber; i++)
        {
            currentObject = Instantiate(foodPrefab);
            Vector3 pos = new Vector3(Random.Range(-40f, 40f), 1f, Random.Range(-40f, 40f));
            currentObject.transform.position = pos;
        }
    }

    public void Destroy()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject food in objects)
        {
            Destroy(food);
        }
    }

    public void AddFood()
    {
        foodNumber += 5;
    }
    public void MinusFood()
    {
        foodNumber -= 5;
    }
}
