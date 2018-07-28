using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTextController : MonoBehaviour {

	private float deathTime;

	
	
	// Update is called once per frame
	void Update () {
		if (Time.time > deathTime)
        {
            gameObject.SetActive(false);
        }
	}

    public void SetDeathTime()
    {
        deathTime = Time.time + 2;
    }
}
