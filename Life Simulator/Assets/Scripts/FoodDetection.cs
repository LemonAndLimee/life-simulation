using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDetection : MonoBehaviour
{

    public bool safe;

    // Start is called before the first frame update
    void Start()
    {
        safe = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider collider)
    {
        if (collider.transform.tag == "BlobChild" && safe == true)
        {
            BlobLogic blobScript = collider.transform.parent.GetComponent<BlobLogic>();
            blobScript.FoodDetected(transform.position);
        }
    }

    public void UnSafe()
    {
        safe = false;

    }
    public void Safe()
    {
        safe = true;
    }
}


