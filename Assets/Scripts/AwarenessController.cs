using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwarenessController : MonoBehaviour {
    public GameController gameController;
    public float decrease;
    public RectTransform.Edge bottom;
    public float barPercentage;

    private Image img;
    private RectTransform rt;
    private float originalHeight;
    private float originalWidth;
    private float originalYPos;
    private float crashCount2;


    // Use this for initialization
    void Start () {
        img = GetComponent<Image>();
        rt = GetComponent<RectTransform>();


        originalYPos = rt.anchoredPosition.y;
        originalHeight = rt.sizeDelta.y;
        originalWidth = rt.sizeDelta.x;
        
	}
	
	// Update is called once per frame
	void Update () {


        if (crashCount2 != gameController.crashes)
        {
            crashCount2 = gameController.crashes;
            barPercentage += ((1 - barPercentage) * 0.33f);
        }

        if (barPercentage > 0.1f)
            barPercentage -= decrease * Time.deltaTime;

        rt.sizeDelta =  new Vector2(originalWidth, originalHeight * barPercentage);
        //rt.localPosition = new Vector3(0,-barPercentage * originalHeight / 2);

        img.color = new Color(2*barPercentage,2*(1-barPercentage),0,1f);
    }

    
    

}
