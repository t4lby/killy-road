using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LightController : SortedObject {

    public enum State
    {
        green,
        amber,
        red
    }

    public State vehicleState;
    public State pedestrianState;
    public float initialBasePoint = -1.25f; //point of ground (for sorting) relative to center.

    public GameObject greenLight;
    public GameObject yellowLight;
    public GameObject redLight;
    public GameObject greenMan;
    public GameObject redMan;

    private GameObject[] lights;


    // Use this for initialization
    void Start () {
        
        basePoint = initialBasePoint;

        lights = new GameObject[5];
        lights[0] = greenLight;
        lights[1] = yellowLight;
        lights[2] = redLight;
        lights[3] = greenMan;
        lights[4] = redMan;

        lightsOff();
        menOff();

        vehicleState = State.red;
        pedestrianState = State.red;
        redLight.SetActive(true);
        redMan.SetActive(true);

    }

    private void lightsOff()
    {
        for (int i = 0; i < 3; i++)
        {
            lights[i].SetActive(false);
        }
    }
    private void menOff()
    {
        for (int i = 3; i < 5; i++)
        {
            lights[i].SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
        {
            vehicleState = State.green;
            lightsOff();
            greenLight.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            vehicleState = State.amber;
            lightsOff();
            yellowLight.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            vehicleState = State.red;
            lightsOff();
            redLight.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            pedestrianState = State.green;
            menOff();
            greenMan.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            pedestrianState = State.red;
            menOff();
            redMan.SetActive(true);
        }
    }
}
