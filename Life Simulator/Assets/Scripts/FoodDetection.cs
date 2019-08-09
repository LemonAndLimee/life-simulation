using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider collider)
    {
        if (collider.transform.tag == "BlobChild")
        {
            BlobLogic blobScript = collider.transform.parent.GetComponent<BlobLogic>();
            blobScript.FoodDetected(transform.position);
        }
    }
}


