using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacteriaLogic : MonoBehaviour
{
    public BacteriaSpawn spawnScript;

    public float speed;
    public Vector3 direction;

    public Vector3 vel;
    public bool detectedFood;

    public double foodCount;

    public float survivalChance;
    public float replicateChance;

    public int resistance;
    public int mutationChance;

    public bool doesSurvive;
    public bool doesReplicate;


    public float timer;

    public bool isTouchingWall;
    public bool isXplus;
    public bool isXminus;
    public bool isZplus;
    public bool isZminus;


    public Material currentMat;


    public bool isRunning;
    public bool stillRunning;

    public int range;
    public SphereCollider sensor;


    public float runTimer;

    // Start is called before the first frame update
    void Start()
    {
        sensor = GetComponentInChildren<SphereCollider>();
        sensor.radius = range / 2;

        spawnScript = GameObject.Find("GameManager").GetComponent<BacteriaSpawn>();

        currentMat = GetComponent<Renderer>().material;

        direction = (new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f))).normalized;

        foodCount = 1.0;

        mutationChance = 50;

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x >= 50 || transform.position.x <= -50 || transform.position.z >= 50 || transform.position.z <= -50)
        {
            Destroy(gameObject);
        }



        timer += Time.deltaTime;

        int limit = Random.Range(1, 5);

        runTimer += Time.deltaTime;

        if (isTouchingWall == true)
        {
            if (timer >= 0.2f)
            {
                if (isXplus == true)
                {
                    direction = (new Vector3(Random.Range(0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f))).normalized;
                }
                else if (isXminus == true)
                {
                    direction = (new Vector3(Random.Range(-1.0f, 0f), 0.0f, Random.Range(-1.0f, 1.0f))).normalized;
                }

                else if (isZplus == true)
                {
                    direction = (new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(0f, 1.0f))).normalized;
                }
                else if (isZminus == true)
                {
                    direction = (new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 0f))).normalized;
                }

                timer = 0;
            }
        }
        else
        {
            if (timer >= limit)
            {
                direction = (new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f))).normalized;
                timer = 0;
            }
        }

        if (isRunning == true)
        {
            vel = transform.forward * -1;
        }
        else
        {
            if (detectedFood == false)
            {
                vel = direction;

                Vector3 rot = transform.eulerAngles;
                rot.x = 0f;
                rot.y = 0f;
                rot.z = 0f;
                transform.eulerAngles = rot;
            }
            else if (detectedFood == true)
            {
                vel = transform.forward;
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
        else if (collider.transform.tag == "Food")
        {
            foodCount += 1;
            Destroy(collider.gameObject);
            detectedFood = false;
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
        else if (collider.transform.tag == "Food")
        {
            detectedFood = false;
        }
    }



    public void FoodDetected(Vector3 targetPos)
    {
        transform.LookAt(targetPos);
        Vector3 rot = transform.eulerAngles;
        rot.x = 0f;
        rot.z = 0f;
        transform.eulerAngles = rot;

        detectedFood = true;
    }

    public void EndDay(bool isFirst, bool isCourse)
    {
        if (isFirst == false)
        {
            if (isCourse == true)
            {
                survivalChance = resistance;

                int rand = Random.Range(0, 101);
                if (rand > survivalChance)
                {
                    //Debug.Log("die1");
                    spawnScript.currentDestroyed = true;
                    DestroyImmediate(gameObject);

                }
                else
                {
                    //Debug.Log("survive1");

                    survivalChance = (float)foodCount * 100;

                    int ran = Random.Range(0, 101);
                    if (ran <= survivalChance)
                    {
                        doesSurvive = true;
                        //Debug.Log("survive2");

                        replicateChance = ((float)foodCount - 1) * 100;
                        if (replicateChance >= 0)
                        {
                            int rando = Random.Range(0, 101);
                            if (rando <= replicateChance)
                            {
                                doesReplicate = true;
                                Replicate(resistance);
                            }
                            else
                            {
                                doesReplicate = false;
                            }
                        }

                        foodCount = 0;
                    }
                    else
                    {
                        //Debug.Log("die2");
                        doesSurvive = false;
                        spawnScript.currentDestroyed = true;
                        DestroyImmediate(gameObject);
                    }
                }
            }
            else
            {
                survivalChance = (float)foodCount * 100;

                int ran = Random.Range(0, 101);
                if (ran <= survivalChance)
                {
                    doesSurvive = true;
                    //Debug.Log("survive2");

                    replicateChance = ((float)foodCount - 1) * 100;
                    if (replicateChance >= 0)
                    {
                        int rando = Random.Range(0, 101);
                        if (rando <= replicateChance)
                        {
                            doesReplicate = true;
                            Replicate(resistance);
                        }
                        else
                        {
                            doesReplicate = false;
                        }
                    }

                    foodCount = 0;
                }
                else
                {
                    //Debug.Log("die2");
                    doesSurvive = false;
                    spawnScript.currentDestroyed = true;
                    DestroyImmediate(gameObject);
                }
            }
            
        }

        survivalChance = 0;
        replicateChance = 0;
    }

    public void Replicate(int parentRes)
    {
        int ran = Random.Range(0, 101);
        int newRes = parentRes;
        if (ran <= mutationChance)
        {
            ran = Random.Range(-10, 11);
            newRes += ran;
            if (newRes <= 0)
            {
                newRes = 0;
            }
            else if (newRes >= 100)
            {
                newRes = 100;
            }
        }

        GameObject clone = Instantiate(gameObject);
        clone.transform.position = transform.position;

        BacteriaLogic bacteriaScript = clone.GetComponent<BacteriaLogic>();
        bacteriaScript.speed = speed;
        bacteriaScript.range = range;
        bacteriaScript.resistance = newRes;
    }
}
