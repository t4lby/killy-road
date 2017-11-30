using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {


    public float[] spawnInterval;
    public GameObject personPrefab;
    public GameObject carPrefab;
    public LightController lights;
    public bool spawnNextCar;

    private List<SortedObject> spriteLocations;
    private float nextSpawnTime;

	
	void Start () {
        nextSpawnTime = Time.time + 1;

        spriteLocations = new List<SortedObject>();
        spriteLocations.Add(lights);

        spawnNextCar = true;
	}
	
	void Update () {

        //check spawns
		if (Time.time > nextSpawnTime)
        {
            spawnPerson();

            nextSpawnTime = Time.time + Random.Range(spawnInterval[0], spawnInterval[1]);
        }
        if (spawnNextCar)
        {
            spawnCar();
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
        controller.pedestrianLight = lights;
        controller.gameController = this;
        spriteLocations.Add(controller);
    }

    private void spawnCar()
    {
        GameObject instance = Instantiate(carPrefab);
        CarController controller = instance.GetComponent<CarController>();
        spriteLocations.Add(controller);
        controller.trafficLight = lights;
        controller.gameController = this;

        spawnNextCar = false;
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
