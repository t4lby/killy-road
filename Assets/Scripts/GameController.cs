using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {


    public float[] spawnInterval;
    public GameObject civilianPrefab;
    public GameObject engineerPrefab;
    public GameObject carPrefab;
    public GameObject car2Prefab;
    public GameObject skullTokenPrefab;
    public GameObject comboText;
    public LightController nearLights;
    public LightController farLights;
    public FixBoxController nearBox;
    public FixBoxController farBox;
    public float nextCarTime1;
    public float nextCarTime2;
    public int killCount;
    public Text killCountText;
    public int crashes;
    public Text crashCountText;
    public int tokenCount;
    public AwarenessController awarenessController;
    public float engineerSpawnTime = 60;
    public float carSpawnTime;
    public float carSpawnVariance; // < carSpawnTime
    public bool spawnNextCar1;
    public bool spawnNextCar2;
    public int comboMultiplier;
    public int comboThreshold;

    private List<SortedObject> spriteLocations;
    private float nextSpawnTime;
    private float nextEngineer;
    private bool hasCollided;

	
	void Start () {

        nextSpawnTime = Time.time + 1;
        nextEngineer = Time.time + 30;

        spriteLocations = new List<SortedObject>();
        spriteLocations.Add(nearLights);
        spriteLocations.Add(farLights);
        spriteLocations.Add(nearBox);
        spriteLocations.Add(farBox);

        nextCarTime1 = Time.time + randomCarTime(carSpawnTime,carSpawnVariance);
        nextCarTime2 = Time.time + randomCarTime(carSpawnTime, carSpawnVariance);
        spawnNextCar1 = true;
        spawnNextCar2 = true;

        hasCollided = false;

        killCount = 0;
        tokenCount = 0;
        crashes = 0;
        comboMultiplier = 1;

        comboText.SetActive(false);
	}
	
	void Update () {

        //check spawns
        if (Time.time > nextSpawnTime)
        {
            spawnCivilian();

            nextSpawnTime = Time.time + Random.Range(spawnInterval[0], spawnInterval[1]);
        }
        if (Time.time > nextEngineer)
        {
            spawnEngineer();

            float b = 1-awarenessController.barPercentage;
            nextEngineer = Time.time + b * engineerSpawnTime;
        }
        if (Time.time > nextCarTime1 && spawnNextCar1)
        {
            spawnCar(1);
            spawnNextCar1 = false;
        }
        if (Time.time > nextCarTime2 && spawnNextCar2)
        {
            spawnCar(2);
            spawnNextCar2 = false;
        }

        //sort display order
        spriteLocations.Sort(sortByBase);
        int i = 0;
        foreach (SortedObject o in spriteLocations)
        {
            o.GetComponent<SpriteRenderer>().sortingOrder = i;
            i++;
        }
        
        if (nearBox.fixedNess >= 1 && farBox.fixedNess >=1)
        {
            gameOver();
        }
        
	}

    private void spawnCivilian()
    {
        GameObject instance = Instantiate(civilianPrefab);
        CivilianController controller = instance.GetComponent<CivilianController>();
        int startSide = Random.Range(0, 2);
        controller.pedestrianLight = startSide < 1 ? nearLights: farLights; 
        controller.startSide = startSide;
        controller.gameController = this;
        spriteLocations.Add(controller);
    }

    private void spawnEngineer()
    {
        GameObject instance = Instantiate(engineerPrefab);
        EngineerController controller = instance.GetComponent<EngineerController>();
        int startSide = Random.Range(0, 2);
        controller.pedestrianLight = startSide < 1 ? nearLights : farLights; 
        controller.startSide = startSide;
        controller.gameController = this;
        controller.boxController = startSide < 1 ? farBox : nearBox;
        spriteLocations.Add(controller);

    }

    private void spawnCar(int type)
    {
        GameObject prefab = type == 1 ? carPrefab : car2Prefab;
        GameObject instance = Instantiate(prefab);
        CarController controller = instance.GetComponent<CarController>();
        spriteLocations.Add(controller);
        controller.trafficLight = type == 1 ? farLights : nearLights;
        controller.gameController = this;
    }

    public void spawnDeathToken(Vector3 location)
    {
        GameObject instance = Instantiate(skullTokenPrefab);
        instance.transform.position = location;
        SkullTokenController controller = instance.GetComponent<SkullTokenController>();
        spriteLocations.Add(controller);
        controller.gameController = this;
    }

    public void DisplayComboText()
    {
        comboText.GetComponent<ComboTextController>().SetDeathTime();
        comboText.SetActive(true);
    }
    

    static int sortByBase(SortedObject o1, SortedObject o2)
    {

        return o2.globalBase().CompareTo(o1.globalBase());
    }

    public void destroySprite(SortedObject toDelete)
    {
        spriteLocations.Remove(toDelete);
        Destroy(toDelete.gameObject);
    }

    public void gameOver()
    {
        Scores.Kills = killCount;
        Scores.Crashes = crashes;
        Scores.Combos = comboMultiplier;

        SceneManager.LoadScene("gameOver", LoadSceneMode.Single);
    }

    public float randomCarTime(float mean, float variance)
    {
        return Random.Range(mean - variance, mean + variance);
    }

}
