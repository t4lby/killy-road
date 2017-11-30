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
    public float acceleration;

    public Vector3 velocity;
    private const float SMALL = 0.5f;

    void Start () {
        basePoint = initialBasePoint;
        transform.position = startPos;
	}
	
	void Update () {
        if (transform.position.x < waitPoint.x + 0.5f && trafficLight.vehicleState == LightController.State.red)
        {
            transform.position = Vector3.SmoothDamp(transform.position, waitPoint, ref velocity, smoothTime);
        }




        if (trafficLight.vehicleState == LightController.State.green || transform.position.x > waitPoint.x)
        {
            velocity += acceleration*(endPoint - transform.position).normalized;
            transform.position += velocity*Time.deltaTime;
        }
        basePoint = initialBasePoint;

        if ((transform.position - endPoint).magnitude < SMALL)
        {

            gameController.destroySprite(this);
            gameController.spawnNextCar = true;
        }
    }
}
