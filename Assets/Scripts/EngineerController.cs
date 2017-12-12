using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerController : PersonController {

    public FixBoxController boxController;

    private Vector3 finalDestination;
	
	// Update is called once per frame
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
                    destination = boxController.transform.position + boxController.localWaitPoint;
                    state = State.crossedFar;
                }
                break;
            case State.crossingNear:
                if (MoveTo(destination, walkSpeed))
                {
                    destination = boxController.transform.position + boxController.localWaitPoint;
                    state = State.crossedNear;
                }
                break;
            case State.crossedFar:
                if (MoveTo(destination, walkSpeed))
                {
                    finalDestination = startPoints[Random.Range(0, 2)];
                    state = State.fixing;
                }
                break;
            case State.crossedNear:
                if (MoveTo(destination, walkSpeed))
                {
                    finalDestination = startPoints[Random.Range(2, 4)];
                    state = State.fixing;
                }
                break;

            case State.fixing:
                if (Fixing())
                {
                    destination = finalDestination;
                    state = State.completedFix;
                }
                break;

            case State.completedFix:
                if (MoveTo(finalDestination, walkSpeed))
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
                    gameController.spawnDeathToken(transform.position);
                }
                break;
            case State.fading:
                Fade();
                break;
            default:
                break;
        }
    }

    private bool Fixing()
    {
        if (boxController.fixedNess >= 1)
        {
            return true;
        }

        tilt();
        boxController.fixedNess += Time.deltaTime * boxController.fixSpeed;

        return false;
    }


}
