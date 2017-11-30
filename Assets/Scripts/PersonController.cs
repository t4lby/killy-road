using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonController : SortedObject {
    public enum State
    {
        sidewalkNear,
        sidewalkFar,
        waitingNear,
        waitingFar,
        crossingNear,
        crossingFar,
        crossedNear,
        crossedFar,
        Dead,
        fading
    }

    public LightController pedestrianLight;
    public GameController gameController;

    public GameObject bloodSplat;
    public GameObject bloodPile;
    public float splatSpeed;
    public float pileSpeed;

    public Vector3[] startPoints; //0 - top left, 1- top right, 2- bottom right, 3- bottom left
    public Vector3 crossNear;
    public Vector3 crossFar;
    public float waitRadius;
    public State state;
    public float walkSpeed;
    public float wobbleAmount;
    public float wobbleSpeed;
    public float initialBasePoint = -0.7f;
    public float fallSpeed;
    public float fallRotationSpeed;
    public float fadeRate;
    public float idleBob;

    private const float SMALL = 0.01f;
    private Vector3 destination;
    private Vector3 carVelocity;
    private Vector3 deathVector;
    private SpriteRenderer sr;
    private float opacity;
    private float idleDiff;


	// Use this for initialization
	void Start () {
        /*on start choose:
         * RANDOM SIDE
         * RANDOM END
         * RANDOM position near crossing
         *
        */
        state = (State) Random.Range(0, 2);
        int lr = Random.Range(0, 2);
        transform.position = startPoints[state == State.sidewalkNear ? lr + 2 : lr];
        if (state == State.sidewalkNear)
            destination = randomInCircle(crossNear, waitRadius);
        else
            destination = randomInCircle(crossFar, waitRadius);

        basePoint = initialBasePoint;

        sr = GetComponent<SpriteRenderer>();
        opacity = 1f;
        idleDiff = Random.Range(0f, 3f);
    }
	
	
	void Update () {

        switch (state)
        {
            case State.sidewalkNear:
                if (MoveTo(destination, walkSpeed))
                    state = State.waitingNear;
                break;
            case State.sidewalkFar:
                if (MoveTo(destination, walkSpeed))
                    state = State.waitingFar;
                break;
            case State.waitingNear:
                Idle();
                if (pedestrianLight.pedestrianState == LightController.State.green)
                {
                    destination = randomInCircle(crossFar, waitRadius);
                    state = State.crossingFar;
                }
                break;
            case State.waitingFar:
                Idle();
                if (pedestrianLight.pedestrianState == LightController.State.green)
                {
                    destination = randomInCircle(crossNear, waitRadius);
                    state = State.crossingNear;
                }
                break;
            case State.crossingFar:
                if (MoveTo(destination, walkSpeed))
                {
                    destination = startPoints[Random.Range(0, 2)];
                    state = State.crossedFar;
                }
                break;
            case State.crossingNear:
                if (MoveTo(destination, walkSpeed))
                {
                    destination = startPoints[Random.Range(2, 4)];
                    state = State.crossedNear;
                }
                break;
            case State.crossedFar:
                if (MoveTo(destination, walkSpeed))
                {
                    state = State.fading;
                }
                break;
            case State.crossedNear:
                if (MoveTo(destination, walkSpeed))
                {
                    state = State.fading;
                }
                break;
            case State.Dead:
                if (!Falling())
                {
                    bloodSplat.SetActive(false);
                    state = State.fading;
                    bloodPile.transform.localScale = new Vector3(0.3f, 0.3f, 1);
                    bloodPile.SetActive(true);
                }  
                break;
            case State.fading:
                Fade();
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "car")
        {
            state = State.Dead;
            carVelocity = collision.gameObject.GetComponent<CarController>().velocity;
            deathVector = (transform.position - collision.transform.position).normalized * carVelocity.magnitude;

            bloodSplat.transform.localScale = new Vector3(0,0,1);
            bloodSplat.SetActive(true);
        }
    }

    private bool Falling()
    {
        Vector3 angles = transform.eulerAngles;
        //Debug.Log(Vector3.Cross(carVelocity, deathVector).normalized);
        if (Vector3.Cross(carVelocity, deathVector).normalized == Vector3.forward)
        {
            if ((angles.z > 180 ? 360 - angles.z : angles.z) > 90)
            {
                return false;

            }
            angles.z -= fallRotationSpeed * Time.deltaTime;
        }
        else
        {
            if ((angles.z > 180 ? 360 - angles.z : angles.z) > 90)
            {
                return false;

            }
            angles.z += fallRotationSpeed * Time.deltaTime;
        }
        bloodSplat.transform.localScale += (new Vector3(1, 1, 0))*splatSpeed*Time.deltaTime;
        transform.position += deathVector * Time.deltaTime * fallSpeed;
        transform.eulerAngles = angles;
        return true;
    }

    private bool MoveTo(Vector3 point, float speed)
    {
        if ((transform.position - point).magnitude < SMALL)
            return true;
        transform.position += (point - transform.position).normalized * speed * Time.deltaTime;
        tilt();
        return false;
    }

    private void tilt()
    {
        Vector3 angles = new Vector3(0, 0, Mathf.Sin(Time.time* wobbleSpeed) * wobbleAmount);
        transform.eulerAngles = angles;
    }

    private Vector3 randomInCircle(Vector3 centre, float radius)
    {
        float randAngle = Random.Range(0f, 360f);
        Vector3 direction = new Vector3(Mathf.Sin(randAngle), Mathf.Cos(randAngle), 0);
        return centre + direction * radius;
    }

    private float dot(Vector3 v1, Vector3 v2)
    {
        return (v1.x * v2.x + v1.y * v2.y * v1.z * v2.z);
    }
    
    private void Fade()
    {
        if (opacity < 0f)
            gameController.destroySprite(this);

        bloodPile.transform.localScale += (new Vector3(1, 1, 0)) * pileSpeed * Time.deltaTime;
        opacity -= fadeRate * Time.deltaTime;
        sr.color = new Color(1f, 1f, 1f, opacity);    
        
    }

    private void Idle()
    {
        transform.eulerAngles = Vector3.zero;
        float cycleTime = 3f;
        float c = (Time.time+idleDiff) % cycleTime;
        if (c < 0.5f)
            transform.position = destination + new Vector3(0, idleBob*Mathf.Sin(c*10), 0);

    }
}
