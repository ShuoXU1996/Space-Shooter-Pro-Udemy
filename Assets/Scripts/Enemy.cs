using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 8.0f;
    private Player _player;
    private Animator _animator;

    private AudioSource _audioSource;
    // Update is called once per frame
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = this.GetComponent<Animator>();
        _audioSource = this.GetComponent<AudioSource>();
        if(_player == null)
        {
            Debug.LogError("Playe is not attributed.");
        }
        if(_animator == null)
        {
            Debug.LogError("Animator of enemy destroy is not attributed.");
        }
        if(_audioSource == null)
        {
            Debug.LogError("Audio enemy is not attributed.");
        }
    }
    void Update()
    {
        //float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5f)
        {
            transform.position = new Vector3(Random.Range(-10.0f, 10.0f), 7f, 0);
        }

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("The " + other.transform.name + " is collided.");
        if (other.tag == "Player")
        {
            //other.transform.GetComponent<Player>().Damage();
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);

        }

        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            
            if(_player != null)
            {
                _player.AddScores(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);

        }
    }
}
