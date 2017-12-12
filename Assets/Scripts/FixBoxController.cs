using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixBoxController : SortedObject {


    public float fixedNess; //0 to 1
    public Vector3 localWaitPoint;
    public float fixSpeed = 0.05f;
    public SpriteRenderer yellowLight;
    public SpriteRenderer redLight;
    public LightController lights;

    private Color off;

	
	void Start () {
        off = new Color(1f, 1f, 1f, 0f);
        yellowLight.color = off;
        redLight.color = off;
	}
	
	void Update () {
		if (fixedNess >= 1)
        {
            redLight.color = off;
            yellowLight.color = new Color(1f, 1f, 1f, Mathf.Abs(Mathf.Sin(4*Time.time)));
            lights.hasControl = false;
        }
        else
        {
            yellowLight.color = off;
            redLight.color = new Color(1f, 1f, 1f, Mathf.Abs(Mathf.Sin(4*Time.time)));
        }
    }
}
