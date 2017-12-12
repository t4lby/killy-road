using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class LightController : SortedObject {

    public enum State
    {
        green,
        red
    }

    public State vehicleState;
    public State pedestrianState;
    public float initialBasePoint = -1.25f; //point of ground (for sorting) relative to center.
    public bool hasControl;

    public GameObject greenLight;
    public GameObject yellowLight;
    public GameObject redLight;
    public GameObject greenMan;
    public GameObject redMan;
    public float transitionTime;
    public float bigCycleTime;
    public float smallCycleTime;
    public float interruptionTime;

    private GameObject[] lights;
    private float nextTransition;
    private bool transitioning;
    private bool interrupted;
    private float interruptionEnd;


    // Use this for initialization
    void Start () {
        hasControl = true;

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

        transitioning = false;

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
        //KEY INPUT DISABLED
		/*if (Input.GetKeyDown(KeyCode.A))
        {
            greenClicked();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            redClicked();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            greenManClicked();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            redManClicked();
        }*/

        if (!interrupted)
        {
            NormalCycle();
        }
        else
        {
            if (Time.time > interruptionEnd)
            {
                interrupted = false;
            }
        }



        if (transitioning && Time.time > nextTransition)
        {
            lightsOff();
            switch (vehicleState)
            {
                case State.red:
                    redLight.SetActive(true);
                    break;
                case State.green:
                    greenLight.SetActive(true);
                    break;
            }
            transitioning = false;
            
        }
    }


    public void greenClicked()
    {
        if (hasControl)
        {
            interrupted = true;
            interruptionEnd = Time.time + interruptionTime;
            greenChange();
        }
    }

    public void redClicked()
    {   
        if (hasControl)
        {
            interrupted = true;
            interruptionEnd = Time.time + interruptionTime;
            redChange();
        }
    }

    public void greenManClicked()
    {
        if (hasControl)
        {
            interrupted = true;
            interruptionEnd = Time.time + interruptionTime;
            greenManChange();
        }
    }
    public void redManClicked()
    {
        if (hasControl)
        {
            interrupted = true;
            interruptionEnd = Time.time + interruptionTime;
            redManChange();
        }
    }

    private void greenChange()
    {
        if (vehicleState != State.green)
        {
            vehicleState = State.green;
            lightsOff();
            yellowLight.SetActive(true);
            nextTransition = Time.time + transitionTime;
            transitioning = true;
        }
    }

    private void redChange()
    {
        if(vehicleState != State.red)
        {
            vehicleState = State.red;
            lightsOff();
            yellowLight.SetActive(true);
            nextTransition = Time.time + transitionTime;
            transitioning = true;
        }
        
    }

    private void greenManChange()
    {
        if (pedestrianState != State.green)
        {
            pedestrianState = State.green;
            menOff();
            greenMan.SetActive(true);
        }
    }

    private void redManChange()
    {
        if (pedestrianState != State.red)
        {
            pedestrianState = State.red;
            menOff();
            redMan.SetActive(true);
        }
    }

    private void NormalCycle()
    {

        //need to move the following to struct so as not to calculate every frame
        float[] t = new float[4];

        t[0] = bigCycleTime;
        t[1] = t[0] + smallCycleTime;
        t[2] = t[1] + bigCycleTime;
        t[3] = t[2] + smallCycleTime;

        float c = Time.time % t[3];

        int w = whereInArray(c, t);

        switch (w)
        {
            case 0:
                greenChange();
                redManChange();
                break;
            case 1:
                redChange();
                redManChange();
                break;
            case 2:
                redChange();
                greenManChange();
                break;
            case 3:
                redChange();
                redManChange();
                break;
            default:
                redChange();
                redManChange();
                break;
            
        }
    }

    private int whereInArray(float f, float[] array)
    {
        int i = 0;
        while (f > array[i])
        {
            i++;
            if (i >= array.Length)
                return i;
        }

        return i;
    }
    
}
