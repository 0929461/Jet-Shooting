using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private int _score;

    private int _multiplierSpeed = 2;
    
    [SerializeField] private GameObject _laserBeamPrefab;

    [SerializeField] private GameObject _tripleLaserPrefab;

    [SerializeField] private GameObject _shieldVisualizer;

    [SerializeField] private GameObject _rightEngine, _leftEngine;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserSound;

    private float _offset = 0.97f;

    private float _fireCoolDown = 0.20f;
    private float _canFire = -1.0f;
    [SerializeField] private int _lives = 3;

    private SpawnManager _spawnManager;

    private UIController _uiController;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldIsActive = false;


    
    private void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiController = GameObject.Find("Canvas").GetComponent<UIController>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.Log("The Spawn_Manager is NULL");
        }

        if (_uiController == null)
        {
            Debug.Log("The _UI_Controller is NULL");
        }

        if(_audioSource == null)
        {
            Debug.Log("AudioSource on the player is NULL");
        }
        _audioSource.clip = _laserSound;
    }

    private void Update()
    {
        PlayerMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            ShootLaser();
        }
    }

    void PlayerMovement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");
        //another way for user input
        //transform.Translate(Vector3.right * horizontalMove * _speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalMove * _speed * Time.deltaTime);

        Vector3 userInput = new Vector3(horizontalMove, verticalMove, 0);
        //optimized user input
        transform.Translate(userInput * _speed * Time.deltaTime);
        
        //if (transform.position.y >= 0)
        // {
        //    transform.position = new Vector3(transform.position.x, 0, 0);
        //}
        //else if (transform.position.y <= -4.0)
        // {
        //    transform.position = new Vector3(transform.position.x, -4.0f, 0);
        //}
        //^^^^^^
        //optimized code for moving y axis
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.7f, 5.76f),0);

        if (transform.position.x >= 9.45f)
        {
            transform.position = new Vector3(9.45f, transform.position.y, 0);
        }
        else if (transform.position.x <= -9.45)
        {
            transform.position = new Vector3(-9.45f, transform.position.y, 0);
        }
    }

    private void ShootLaser()
    {
        _canFire = Time.time + _fireCoolDown;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleLaserPrefab, transform.position + new Vector3(-0.30f, 0.15f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserBeamPrefab, transform.position + new Vector3(0, _offset, 0), Quaternion.identity);
        }
        _audioSource.Play();
    }

    public void DamagePlayer()
    {
        if (_isShieldIsActive == true)
        {
            _isShieldIsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        
        _lives -= 1;

        if(_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else
        {
            if (_lives == 1)
            {
                _leftEngine.SetActive(true);
            }
        }

        _uiController.UpdatePlayerLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void OnEnableTripleLaser()
    {     
        _isTripleShotActive = true;
        StartCoroutine(SecondsForTripleLaser());
    }

    private IEnumerator SecondsForTripleLaser()
    {
        yield return new WaitForSeconds(10);
        _isTripleShotActive = false;
    }

    public void OnEnableSpeedBoost()
    {
        _isSpeedBoostActive = true;
        _speed *= _multiplierSpeed;
        StartCoroutine(CoolDownSpeedPowerUp());
    }

    private IEnumerator CoolDownSpeedPowerUp()
    {
        yield return new WaitForSeconds(7.0f);
        _isSpeedBoostActive = false;
        _speed /= _multiplierSpeed;
    }

    public void OnEnablePlayerShield()
    {
        _isShieldIsActive = true;
        _shieldVisualizer.SetActive(true);
    }

   public void AddScore(int points)
    {
        _score += points;
        _uiController.UpdatePlayerScore(_score);
    }
}