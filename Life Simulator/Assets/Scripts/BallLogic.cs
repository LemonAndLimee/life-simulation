﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLogic : MonoBehaviour
{

    public float speed;
    public Vector3 direction;

    public Vector3 vel;


    public float timer;

    public bool isTouchingWall;
    public bool isXplus;
    public bool isXminus;
    public bool isZplus;
    public bool isZminus;


    public bool detectedFood;
    public GameObject target;
    public bool hasTarget;

    // Start is called before the first frame update
    void Start()
    {

        speed = 20f;

        direction = (new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f))).normalized;
    }

    // Update is called once per frame
    void Update()
    {


        timer += Time.deltaTime;

        int limit = Random.Range(1, 5);



        if (detectedFood == false)
        {
            vel = direction;

            Vector3 rot = transform.eulerAngles;
            rot.x = 0f;
            rot.y = 0f;
            rot.z = 0f;
            transform.eulerAngles = rot;
        }
        else
        {
            if (target != null)
            {
                BlobLogic blobScript = target.GetComponent<BlobLogic>();
                blobScript.Run(gameObject);

                transform.LookAt(target.transform.position);
                Vector3 rot = transform.eulerAngles;
                rot.x = 0f;
                rot.z = 0f;
                transform.eulerAngles = rot;

                vel = transform.forward;
            }
            else
            {
                detectedFood = false;
                hasTarget = false;

                vel = direction;

                Vector3 rot = transform.eulerAngles;
                rot.x = 0f;
                rot.y = 0f;
                rot.z = 0f;
                transform.eulerAngles = rot;
            }
        }


        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = vel * speed;
    }

    public void OnCollisionEnter(Collision collider)
    {
        if (collider.transform.tag == "Wall")
        {
            detectedFood = false;
            isTouchingWall = true;
            if (collider.transform.name == "WallLeft")
            {
                direction.x *= -1;
                isXplus = true;
            }
            else if (collider.transform.name == "WallRight")
            {
                direction.x *= -1;
                isXminus = true;
            }
            else if (collider.transform.name == "WallTop")
            {
                direction.z *= -1;
                isZminus = true;
            }
            else if (collider.transform.name == "WallBottom")
            {
                direction.z *= -1;
                isZplus = true;
            }
        }
        else if (collider.transform.tag == "Blob")
        {
            Destroy(collider.gameObject);

        }
    }
    public void OnCollisionExit(Collision collider)
    {
        if (collider.transform.tag == "Wall")
        {
            isTouchingWall = false;
            isXplus = false;
            isXminus = false;
            isZplus = false;
            isZminus = false;
        }
        else if (collider.transform.tag == "Blob")
        {
            detectedFood = false;
            target = null;
            hasTarget = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "BlobChild")
        {
            BlobLogic blobScript = other.transform.parent.GetComponent<BlobLogic>();
            //blobScript.Run(gameObject);
        }
        if (other.transform.tag == "Food")
        {
            FoodDetection foodScript = other.transform.GetComponent<FoodDetection>();
            foodScript.UnSafe();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "BlobChild")
        {
            BlobLogic blobScript = other.transform.parent.GetComponent<BlobLogic>();
            blobScript.Safe();
        }
        if (other.transform.tag == "Food")
        {
            FoodDetection foodScript = other.transform.GetComponent<FoodDetection>();
            foodScript.Safe();
        }
    }

    public void FoodDetected(GameObject tar)
    {

        detectedFood = true;

        if (hasTarget == false)
        {
            target = tar;
            hasTarget = true;
        }
    }
    public void FoodEscaped()
    {
        detectedFood = false;
        hasTarget = false;
    }


}