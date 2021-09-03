using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;

    [SerializeField] private GameObject _enemyContainer;

    [SerializeField] private GameObject _tripleLaserPowerUpPrefab;

    [SerializeField] private GameObject _speedupPowerUpPrefab;

    [SerializeField] private GameObject[] _powerUps;

    private bool _stopSpawningEnemies = false;

    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine("SpawnEnemy");
        StartCoroutine(SpawnTriplePowerUp());
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawningEnemies==false)
        {
            if(_enemyPrefab != null)
            {
                Vector3 posRandomSpawn = new Vector3(Random.Range(-8, 8), 7, 0);
                var enemy = Instantiate(_enemyPrefab, posRandomSpawn, Quaternion.identity);
                
                enemy.transform.parent = _enemyContainer.transform;
                yield return new WaitForSeconds(5);
            }
        }
    }

    private IEnumerator SpawnTriplePowerUp()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawningEnemies==false)
        {
            Vector3 postToSpawn = new Vector3(Random.Range(-8, 8), 7, 0);

            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_powerUps[randomPowerUp], postToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawningEnemies = true;
    }
}
