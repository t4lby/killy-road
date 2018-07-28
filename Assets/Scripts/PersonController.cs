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
        fading,
        fixing,
        completedFix
    }


    public LightController pedestrianLight;
    public GameController gameController;

    public Sprite[] aliveImages;
    public Sprite[] deadImages;

    public GameObject bloodSplat;
    public GameObject bloodPile;
    public float splatSpeed = 10f;
    public float pileSpeed = 0.7f;

    public Vector3[] startPoints; //0 - top left, 1- top right, 2- bottom right, 3- bottom left
    public Vector3 crossNear = new Vector3(-2.75f,-0.5f,0f);
    public Vector3 crossFar = new Vector3(2f, 2.5f, 0f);
    public float waitRadius = 0.6f;
    public State state;
    public float initialWalkSpeed = 1f;
    public float walkSpeedDiff = 0.3f;
    public float wobbleAmount = 3f;
    public float wobbleSpeed = 15f;
    public float initialBasePoint = -0.7f;
    public float deadBasePoint = -0.2f;
    public float fallSpeed = 0.7f;
    public float fallRotationSpeed = 300f;
    public float fadeRate = 0.3f;
    public float idleBob = 0.1f;
    public int startSide;

    protected const float SMALL = 0.05f;
    protected Vector3 destination;
    protected Vector3 carVelocity;
    protected Vector3 deathVector;
    protected SpriteRenderer sr;
    protected float opacity;
    protected float idleDiff;
    protected float walkSpeed;

    private int imgIndex;


	// Use this for initialization
	protected void Start () {
        /*on start choose:
         * RANDOM SIDE
         * RANDOM END
         * RANDOM position near crossing
         *
        */
        

        state = (State) startSide;
        int lr = Random.Range(0, 2);
        transform.position = startPoints[state == State.sidewalkNear ? lr + 2 : lr];
        if (state == State.sidewalkNear)
            destination = randomInCircle(crossNear, waitRadius);
        else
            destination = randomInCircle(crossFar, waitRadius);

        basePoint = initialBasePoint;

        sr = GetComponent<SpriteRenderer>();
        imgIndex = Random.Range(0, aliveImages.Length);
        sr.sprite = aliveImages[imgIndex];
        opacity = 1f;
        idleDiff = Random.Range(0f, 3f);
        walkSpeed = Random.Range(initialWalkSpeed - walkSpeedDiff, initialWalkSpeed + walkSpeedDiff);
    }
	
	

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "car")
        {
            CarController controller = collision.gameObject.GetComponent<CarController>();
            carVelocity = controller.velocity;
            deathVector = (transform.position - collision.transform.position).normalized * carVelocity.magnitude;
            if (state != State.Dead && state != State.fading)
            {
                if (controller.killCount == 0)
                {
                    gameController.crashes += 1;
                    gameController.crashCountText.text = "" + gameController.crashes;
                }
                controller.killCount += 1;
                state = State.Dead;
                sr.sprite = deadImages[imgIndex];
            }

            

            bloodSplat.transform.localScale = new Vector3(0,0,1);
            bloodSplat.SetActive(true);

            basePoint = deadBasePoint;
        }
    }

    protected bool Falling()
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

    protected bool MoveTo(Vector3 point, float speed)
    {
        if ((transform.position - point).magnitude < SMALL)
            return true;
        transform.position += (point - transform.position).normalized * speed * Time.deltaTime;
        tilt();
        return false;
    }

    protected void tilt()
    {
        Vector3 angles = new Vector3(0, 0, Mathf.Sin(Time.time* wobbleSpeed) * wobbleAmount);
        transform.eulerAngles = angles;
    }

    protected Vector3 randomInCircle(Vector3 centre, float radius)
    {
        float randAngle = Random.Range(0f, 360f);
        Vector3 direction = new Vector3(Mathf.Sin(randAngle), Mathf.Cos(randAngle), 0);
        return centre + direction * radius;
    }
    
    protected void Fade()
    {
        if (opacity < 0f)
        {
            bloodPile.SetActive(false);
            gameController.destroySprite(this);
        }
            

        bloodPile.transform.localScale += (new Vector3(1, 1, 0)) * pileSpeed * Time.deltaTime;
        opacity -= fadeRate * Time.deltaTime;
        sr.color = new Color(1f, 1f, 1f, opacity);    
        
    }

    protected void Idle()
    {
        transform.eulerAngles = Vector3.zero;
        float cycleTime = 3f;
        float c = (Time.time+idleDiff) % cycleTime;
        if (c < 0.5f)
            transform.position = destination + new Vector3(0, idleBob*Mathf.Sin(c*10), 0);

    }
}
