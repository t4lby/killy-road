using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : SortedObject {

    public GameController gameController;

    public float smoothTime;
    public LightController trafficLight;
    public float initialBasePoint = -0.5f;
    public Vector3 startPos;
    public Vector3 waitPoint;
    public Vector3 endPoint;
    public float initialAcceleration = 0.1f;
    public float accelerationDiff = 0.05f;
    public bool goingRight;
    public bool collided;

    public Vector3 velocity;
    private const float SMALL = 0.5f;
    private float acceleration;


    void Start () {
        basePoint = initialBasePoint;
        transform.position = startPos;
        acceleration = Random.Range(initialAcceleration - accelerationDiff, initialAcceleration + accelerationDiff);
        collided = false;
	}
	
	void Update () {
        if ((goingRight ? transform.position.x < waitPoint.x + 0.5f : transform.position.x > waitPoint.x - 0.5f) && trafficLight.vehicleState == LightController.State.red)
        {
            transform.position = Vector3.SmoothDamp(transform.position, waitPoint, ref velocity, smoothTime);
        }




        if (trafficLight.vehicleState == LightController.State.green || (goingRight ? transform.position.x > waitPoint.x : transform.position.x < waitPoint.x))
        {
            velocity += acceleration*(endPoint - transform.position).normalized;
            transform.position += velocity*Time.deltaTime;
        }
        basePoint = initialBasePoint;

        if ((transform.position - endPoint).magnitude < SMALL)
        {
            gameController.destroySprite(this);
            if (goingRight)
                gameController.spawnNextCar = true;
            else
                gameController.spawnNextCar2 = true;
        }
    }
}
