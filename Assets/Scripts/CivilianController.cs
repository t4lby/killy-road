using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianController : PersonController {

	
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

    

}
