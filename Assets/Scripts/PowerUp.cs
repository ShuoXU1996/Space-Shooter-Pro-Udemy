using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _idPowerUps;
    //private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _clip;

    // Update is called once per frame
    void Update()
    {
        //_audioSource = GetComponent<AudioSource>();
        //if(_audioSource == null)
        //{
        //    Debug.LogError("Audio source for powerups is null.");
        //}
        transform.Translate(Vector3.down * Time.deltaTime *_speed);
        if(transform.position.y <= -4.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {   
            Player player = collision.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if(player != null)
            {
                switch (_idPowerUps)
                {
                    case 0:
                        player.ActiveTripleShot();
                        break;
                    case 1:
                        player.ActiveSpeedBoost();
                        break;
                    case 2:
                        player.ActiveShield();
                        break;
                    default:
                        Debug.Log("Default values.");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }

    
}
