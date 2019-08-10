﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobLogic : MonoBehaviour
{
	public BlobSpawn spawnScript;

	public float speed;
	public Vector3 direction;

	public Vector3 vel;
	public bool detectedFood;

	public double foodCount;

	public float survivalChance;
	public float replicateChance;

	public bool doesSurvive;
	public bool doesReplicate;

	public int mutationChance;

	public float timer;

	public bool isTouchingWall;
	public bool isXplus;
	public bool isXminus;
	public bool isZplus;
	public bool isZminus;

	public float foodPercentage;

	public Material currentMat;

	public float initialSpeed;
	public float initialRange;

    public float immunity;
    public float initialImmunity;

    public bool isInfected;
    public float diseaseTime;
    public float lethality;
    public float infectivity;
    public float diseaseTimer;


	public int range;
	public SphereCollider sensor;

    public Color startColor;

	// Start is called before the first frame update
	void Start()
	{

        sensor = GetComponentInChildren<SphereCollider>();
		sensor.radius = range / 2;

		spawnScript = GameObject.Find("GameManager").GetComponent<BlobSpawn>();
		initialSpeed = spawnScript.initialSpeed;
		initialRange = spawnScript.initialRange;

		currentMat = GetComponent<Renderer>().material;

		direction = (new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f))).normalized;

		foodCount = 1.0;

		startColor = new Color(1f, 0f, 0f, 1f);

		if (speed <= 0)
		{
			startColor.r = 0f;
		}
		else if (speed <= initialSpeed)
		{
			startColor.r = (speed / initialSpeed);
			if (range <= initialRange)
			{
				startColor.g = 1 - (range / initialRange);
			}
			else if (range > initialRange)
			{
				startColor.b = (range / initialRange) - 1;
			}
		}
		else if (speed <= (initialSpeed * 2) - 2)
		{
			startColor.g = 1 - (initialSpeed / speed);
			startColor.b = 1 - (initialSpeed / speed);
			if (range <= initialRange)
			{
				startColor.g = 1 - (range / initialRange);
			}
			else if (range > initialRange)
			{
				startColor.b = (range / initialRange) - 1;
			}

		}
		else if (speed > (initialSpeed * 2) - 2)
		{
			startColor.g = 1 - (initialSpeed / ((initialSpeed * 2) - 2));
			startColor.b = 1 - (initialSpeed / ((initialSpeed * 2) - 2));

			if (range <= initialRange)
			{
				startColor.g = 1 - (range / initialRange);
			}
			else if (range > initialRange)
			{
				startColor.b = (range / initialRange) - 1;
			}
		}

		currentMat.color = startColor;


        
        diseaseTimer = 0f;

        foodPercentage = 1 / (speed / initialSpeed);
    }

	// Update is called once per frame
	void Update()
	{
        diseaseTime = (1f / (immunity)) * 100f;

        if (isInfected == true)
        {
            diseaseTimer += Time.deltaTime;

            Color diseaseColor = new Color(0f, 0f, 0f, 1f);
            currentMat.color = diseaseColor;

            if (diseaseTimer >= diseaseTime)
            {
                DiseaseVerdict((int)immunity, (int)lethality);
                diseaseTimer = 0;
            }
        }
        else
        {
            currentMat.color = startColor;
        }

		timer += Time.deltaTime;

		int limit = Random.Range(1, 5);

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
			vel = transform.forward;
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
			foodCount += (1 * foodPercentage);
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "BlobChild")
        {
            if (isInfected == true)
            {
                Debug.Log("infect");
                BlobLogic blobScript = other.transform.parent.GetComponent<BlobLogic>();
                blobScript.Infect((int)infectivity, (int)lethality, false);
            }
        }
    }

    public void Infect(int infect, int lethal, bool isPatientZero)
    {
        int ran = Random.Range(0, 101);
        if (ran >= (immunity))
        {
            ran = Random.Range(0, 101);
            if (ran <= infect || isPatientZero == true)
            {
                isInfected = true;

                lethality = lethal;
                infectivity = infect;
            }
            else
            {
                isInfected = false;
            }
        }
        
    }

    public void DiseaseVerdict(int imm, int leth)
    {
        int ran = Random.Range(0, 101);
        if (ran >= (imm))
        {
            ran = Random.Range(0, 101);
            if (ran <= lethality)
            {
                Destroy(gameObject);
            }
            else
            {
                isInfected = false;
            }
        }
        else
        {
            isInfected = false;
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

	public void EndDay(bool isFirst)
	{
		if (isFirst == false)
        {
            survivalChance = (float)foodCount * 100;
            int ran = Random.Range(0, 101);
            if (ran <= survivalChance)
            {
                doesSurvive = true;

            }
            else
            {
                doesSurvive = false;
                Destroy(gameObject);
            }

            replicateChance = ((float)foodCount - 1) * 100;
            if (replicateChance >= 0)
            {
                ran = Random.Range(0, 101);
                if (ran <= replicateChance)
                {
                    doesReplicate = true;
                    Replicate(speed, range, immunity);
                }
                else
                {
                    doesReplicate = false;
                }
            }

            foodCount = 0;
        }

        survivalChance = 0;
        replicateChance = 0;
	}

	public void Replicate(float parentSpeed, int parentRange, float parentImmunity)
	{
		int ran = Random.Range(0, 101);
		float newSpeed = parentSpeed;
		if (ran <= mutationChance)
		{
			ran = Random.Range(-5, 6);
			newSpeed += ran;
			if (newSpeed <= 0)
			{
				newSpeed = 1;
			}
		}

		ran = Random.Range(0, 101);
		int newRange = parentRange;
		if (ran <= mutationChance)
		{
			ran = Random.Range(-3, 4);
			newRange += ran;
			if (newRange <= 0)
			{
				newRange = 1;
			}
		}

        ran = Random.Range(0, 101);
        float newImmunity = parentImmunity;
        if (ran <= mutationChance)
        {
            ran = Random.Range(-8, 9);
            newImmunity += ran;
            if (newImmunity <= 0)
            {
                newImmunity = 1;
            }
            else if (newImmunity >= 100)
            {
                newImmunity = 100;
            }
        }

        GameObject clone = Instantiate(gameObject);
		clone.transform.position = transform.position;

		BlobLogic blobScript = clone.GetComponent<BlobLogic>();
		blobScript.speed = newSpeed;
		blobScript.range = newRange;
        blobScript.immunity = newImmunity;
	}
}