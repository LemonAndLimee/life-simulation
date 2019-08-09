using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DiseaseLogic : MonoBehaviour
{
    public BlobSpawn spawnScript;

    public int infectivity;
    public int lethality;

    public Text infectivityText;
    public Text lethalityText;

    public bool infectivityEntered;
    public bool lethalityEntered;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (infectivityEntered == true)
        {
            infectivity = Convert.ToInt32(infectivityText.text);
        }
        if (lethalityEntered == true)
        {
            lethality = Convert.ToInt32(lethalityText.text);
        }
    }

    public void EnterInfectivity()
    {
        infectivityEntered = true;
    }
    public void EnterLethality()
    {
        lethalityEntered = true;
    }

    public void AddDisease()
    {
        
        spawnScript.AddDisease(infectivity, lethality);
    }
}
