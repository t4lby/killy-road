using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullTokenController : SortedObject {
    public enum Type
    {
        normal,
        token
    }

    public GameController gameController;
    public float spinSpeed;
    public float bob;
    public float initialBase;
    public Vector3 scoreLocation;
    public float smoothTime;
    public Sprite token;
    public Sprite skull;

    private Type type;
    private Vector3 destination;
    private float idleDiff;
    private const float SMALL = 0.2f;
    private Vector3 velocity;
    private bool clicked;
    private SpriteRenderer sr;

    private void OnMouseDown()
    {
        spinSpeed *= 20;
        clicked = true;
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        destination = transform.position;
        idleDiff = Random.Range(0f, 3f);
        basePoint = initialBase;
        type = Type.normal;
        sr.sprite = skull;
        clicked = true;
    }

    // Update is called once per frame
    void Update () {

        if (clicked)
        {
            Spin();
            if (MoveToScore())
            {
                gameController.killCount += 1;
                gameController.tokenCount += 1;
                gameController.killCountText.text = "" + gameController.killCount;
                gameController.destroySprite(this);
            }
                
        }
        else
        {
            Spin();
            Bob();
        }
            
	}

    private void Spin()
    {
        Vector3 angles = transform.eulerAngles;
        angles.y += spinSpeed * Time.deltaTime;
        transform.eulerAngles = angles;
    }

    private void Bob()
    {
        float cycleTime = 3f;
        float c = (Time.time + idleDiff) % cycleTime;
        if (c < 0.5f)
            transform.position = destination + new Vector3(0, bob * Mathf.Sin(c * 10), 0);
    }

    private bool MoveToScore()
    {
        if ((transform.position - scoreLocation).magnitude < SMALL)
            return true;
        transform.position = Vector3.SmoothDamp(transform.position, scoreLocation, ref velocity, smoothTime);
        return false;
    }
}
