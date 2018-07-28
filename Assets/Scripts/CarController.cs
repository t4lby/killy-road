using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : SortedObject {

    public GameController gameController;
    public Sprite[] images;

    public float smoothTime;
    public LightController trafficLight;
    public float initialBasePoint = -0.5f;
    public Vector3 startPos;
    public Vector3 waitPoint;
    public Vector3 endPoint;
    public float initialAcceleration = 0.1f;
    public float accelerationDiff = 0.05f;
    public float maxCarSpeed = 1f;
    public bool goingRight;
    public int killCount;

    public Vector3 velocity;
    private const float SMALL = 0.5f;
    private float acceleration;
    private SpriteRenderer sr;
    private bool comboTextDisplayed;


    void Start () {
        basePoint = initialBasePoint;
        transform.position = startPos;
        acceleration = Random.Range(initialAcceleration - accelerationDiff, initialAcceleration + accelerationDiff);
        killCount = 0;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = images[Random.Range(0, images.Length)];
        comboTextDisplayed = false;
	}
	
	void Update () {
        if ((goingRight ? transform.position.x < waitPoint.x + 0.5f : transform.position.x > waitPoint.x - 0.5f) && trafficLight.vehicleState == LightController.State.red)
        {
            transform.position = Vector3.SmoothDamp(transform.position, waitPoint, ref velocity, smoothTime);
        }




        if (trafficLight.vehicleState == LightController.State.green || (goingRight ? transform.position.x > waitPoint.x : transform.position.x < waitPoint.x))
        {
            if (velocity.magnitude < maxCarSpeed)
                velocity += acceleration*(endPoint - transform.position).normalized;
            transform.position += velocity*Time.deltaTime;
        }
        basePoint = initialBasePoint;

        if (killCount >= gameController.comboThreshold && !comboTextDisplayed)
        {
            gameController.comboMultiplier += 1;
            gameController.DisplayComboText();
            comboTextDisplayed = true;
        }

        if ((transform.position - endPoint).magnitude < SMALL)
        {
            gameController.destroySprite(this);
            if (goingRight)
            {
                gameController.nextCarTime1 = Time.time + gameController.randomCarTime(gameController.carSpawnTime, gameController.carSpawnVariance);
                gameController.spawnNextCar1 = true;
            }
            else
            {
                gameController.nextCarTime2 = Time.time + gameController.randomCarTime(gameController.carSpawnTime, gameController.carSpawnVariance);
                gameController.spawnNextCar2 = true;
            }  
        }
    }
}
