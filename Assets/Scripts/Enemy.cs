using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 2.0f;

    private PlayerController _playerController;

    private UIController _uiController;

    private Animator _anim;

    private AudioSource _audioSource;

    [SerializeField] private AudioClip _enemyExplosionSound;

    [SerializeField] private GameObject _enemyLaserPrefab;

    private float _fireRate = 3.0f;

    private float _canFire = -1;

    private void Start()
    {
        if(_playerController == null)
        {
            Debug.Log("PlayerController is NULL");
            _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        _uiController = GameObject.Find("Canvas").GetComponent<UIController>();

        _anim = gameObject.GetComponent<Animator>();
    
        if(_anim == null)
        {
            Debug.LogError("Animator is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _enemyExplosionSound;
    }

    void Update()
    {
        EnemyMovement();

        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].EnemyLaserActive();
            }
        }
    }

    private void EnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5)
        {
            float randomRange = Random.Range(-8, 8f);
            transform.position = new Vector3(randomRange, 7, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController player = collision.transform.GetComponent<PlayerController>();

            if (player != null)
            {
                player.DamagePlayer();
            }
            
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 1f;

            _audioSource.Play();
            Destroy(this.gameObject, 1.80f);
        }
         if (collision.tag == "Laser")
        {
            Destroy(collision.gameObject);
            
            if (_playerController != null)
            {
                _playerController.AddScore(10);
            }
            
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 1f;

            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 1.80f);
        }
    }

    
}
