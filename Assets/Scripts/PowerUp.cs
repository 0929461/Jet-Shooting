using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _speedPowerUp;

    [SerializeField] private int _powerUpID;

    [SerializeField] private AudioClip _powerupSound;

    void Update()
    {
        transform.Translate(Vector3.down * _speedPowerUp * Time.deltaTime);

        if (transform.position.y < -5.50f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController player = collision.transform.GetComponent<PlayerController>();

            AudioSource.PlayClipAtPoint(_powerupSound, transform.position);

            if (player != null)
            {

                switch (_powerUpID)
                {
                    case 0:
                        player.OnEnableTripleLaser();
                        break;
                    case 1:
                        Debug.Log("SpeedPowerUp collected");
                        player.OnEnableSpeedBoost();
                        break;
                    case 2:
                        player.OnEnablePlayerShield();
                        Debug.Log("Shields collected");
                        break;
                    default:
                        Debug.Log("default");
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }
}
