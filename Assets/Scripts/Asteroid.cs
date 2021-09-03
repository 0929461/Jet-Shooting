using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float speedZ = 2f;

    [SerializeField] private GameObject _asteroidExplosionPrefab;

    private SpawnManager _spawnManager;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * speedZ * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser")
        {
            Vector3 asterPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Instantiate(_asteroidExplosionPrefab, asterPos, Quaternion.identity);
            Destroy(collision.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.35f);
        }
    }
}
