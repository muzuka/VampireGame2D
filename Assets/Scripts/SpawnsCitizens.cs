using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnsCitizens : MonoBehaviour {

    public GameObject spawn;
    public Transform spawnPos;
    public int spawnLimit;
    public int spawnTime;

    Timer spawnTimer;

    bool spawning = true;
    List<GameObject> citizens;

	// Use this for initialization
	void Start ()
    {
        spawnTimer = new Timer(spawnTime);
        citizens = new List<GameObject>();
	}

    // Update is called once per frame
    void Update ()
    {
        spawning = citizens.Count < spawnLimit;
		
        if (spawning)
        {
            spawnTimer.update(spawnCitizen);
        }

        for(int i = citizens.Count - 1; i >= 0; i--)
        {
            if (citizens[i] == null)
            {
                citizens.RemoveAt(i);
            }
        }
	}

    void spawnCitizen ()
    {
        citizens.Add(Instantiate(spawn, spawnPos.position, Quaternion.identity));
    }
}
