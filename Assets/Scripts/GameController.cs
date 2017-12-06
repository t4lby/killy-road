using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {


    public float[] spawnInterval;
    public GameObject personPrefab;
    public GameObject carPrefab;
    public GameObject car2Prefab;
    public GameObject skullTokenPrefab;
    public LightController nearLights;
    public LightController farLights;
    public bool spawnNextCar;
    public bool spawnNextCar2;
    public int killCount;
    public Text killCountText;
    public int crashes;
    public Text crashCountText;

    private List<SortedObject> spriteLocations;
    private float nextSpawnTime;
    private bool hasCollided;

	
	void Start () {
        nextSpawnTime = Time.time + 1;

        spriteLocations = new List<SortedObject>();
        spriteLocations.Add(nearLights);
        spriteLocations.Add(farLights);

        spawnNextCar = true;
        spawnNextCar2 = true;

        hasCollided = false;

        killCount = 0;
	}
	
	void Update () {
        int rands = Random.Range(1, 100);

        //check spawns
        if (Time.time > nextSpawnTime)
        {
            spawnPerson();

            nextSpawnTime = Time.time + Random.Range(spawnInterval[0], spawnInterval[1]);
        }
        if (spawnNextCar && rands == 1)
        {
            spawnCar(1);
        }
        if (spawnNextCar2 && rands == 2)
        {
            spawnCar(2);
        }

        //sort display order
        spriteLocations.Sort(sortByBase);
        int i = 0;
        foreach (SortedObject o in spriteLocations)
        {
            o.GetComponent<SpriteRenderer>().sortingOrder = i;
            i++;
        }
        
        
	}

    private void spawnPerson()
    {
        GameObject instance = Instantiate(personPrefab);
        PersonController controller = instance.GetComponent<PersonController>();
        controller.pedestrianLight = nearLights;
        controller.gameController = this;
        spriteLocations.Add(controller);
    }

    private void spawnCar(int type)
    {
        GameObject prefab = type == 1 ? carPrefab : car2Prefab;
        GameObject instance = Instantiate(prefab);
        CarController controller = instance.GetComponent<CarController>();
        spriteLocations.Add(controller);
        controller.trafficLight = nearLights;
        controller.gameController = this;

        if (type == 1)
            spawnNextCar = false;
        else
            spawnNextCar2 = false;
    }

    public void spawnDeathToken(Vector3 location)
    {
        GameObject instance = Instantiate(skullTokenPrefab);
        instance.transform.position = location;
        SkullTokenController controller = instance.GetComponent<SkullTokenController>();
        spriteLocations.Add(controller);
        controller.gameController = this;

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
}
