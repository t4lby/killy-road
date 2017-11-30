using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortedObject : MonoBehaviour {

    public float basePoint;

    public float globalBase()
    {
        return transform.position.y + basePoint;
    }
	
}
