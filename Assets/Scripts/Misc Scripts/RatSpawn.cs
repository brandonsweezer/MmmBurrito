using UnityEngine;
using System.Collections;

public class RatSpawn : MonoBehaviour
{
    public Rats rat;
    public int spawnTimer;
    public float range;

    // Use this for initialization
    void Start()
    {
        rat.spawnPoint = transform.position;
        rat.spawnTimer = spawnTimer;
        rat.tolerance = range;
        Instantiate(rat, new Vector3(1000, 0, 1000), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
