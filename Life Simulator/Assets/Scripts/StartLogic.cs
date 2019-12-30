using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartLogic : MonoBehaviour
{
    public int initialSpeed;
    public int food;
    public int blobs;
    public int range;
    public bool stamina;

    public Text speedText;
    public Text foodText;
    public Text blobsText;
    public Text rangeText;
    public Toggle staminaToggle;

    public bool speedEntered;
    public bool foodEntered;
    public bool blobsEntered;
    public bool rangeEntered;
    

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (speedEntered == true)
        {
            initialSpeed = Convert.ToInt32(speedText.text);
        }
        if (foodEntered == true)
        {
            food = Convert.ToInt32(foodText.text);
        }
        if (blobsEntered == true)
        {
            blobs = Convert.ToInt32(blobsText.text);
        }
        if (rangeEntered == true)
        {
            range = Convert.ToInt32(rangeText.text);
        }

        stamina = staminaToggle.isOn;
    }

    public void EnterSpeed()
    {
        speedEntered = true;
    }
    public void EnterFood()
    {
        foodEntered = true;
    }
    public void EnterBlobs()
    {
        blobsEntered = true;
    }
    public void EnterRange()
    {
        rangeEntered = true;
    }

    public void Begin()
    {
        if (speedEntered == true && foodEntered == true && blobsEntered == true && rangeEntered == true)
        {
            SceneManager.LoadScene("Evo");
        }
    }

}
