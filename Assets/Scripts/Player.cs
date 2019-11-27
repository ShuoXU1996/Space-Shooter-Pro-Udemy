using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    private float _speedMultiplier = 2.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private Vector3 _offset = new Vector3(0, 1.05f, 0);
    
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    [SerializeField]
    private AudioClip _laserSoundClip;
    
    private AudioSource _audioSource;

    [SerializeField]
    private int _scores;

    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is not attributed.");
        }
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is not attributed.");
        }
        if(_audioSource == null)
        {
            Debug.LogError("Audio on player is null.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip; 
        }

        transform.position = new Vector3(0, 0, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovements();
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Fire") && Time.time > _canFire)
        {
            FireLaser();
        }
#else
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        } 
#endif


    }

    void CalculateMovements()
    {
        //float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal"); 

        //float verticalInput = CrossPlatformInputManager.GetAxis("Vertical");
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
        
       
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x <= -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + _offset, Quaternion.identity);
        }
        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldActive == false)
        {
            if (_lives > 0)
            {
                _lives -= 1;
                if(_lives == 2)
                {
                    _leftEngine.SetActive(true);
                }
                else if(_lives == 1) 
                {
                    _rightEngine.SetActive(true);
                }
            }
            _uiManager.UpdateLives(_lives);
            if (_lives == 0)
            {
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
            }
        }
        else
        {
            return;
        }
    }

  
    public void ActiveTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownCoroutine());
    }

    IEnumerator TripleShotPowerDownCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void ActiveSpeedBoost()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownCoroutine());
    }

    IEnumerator SpeedBoostPowerDownCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ActiveShield()
    {
        _isShieldActive = true;
        //Behaviors
        _shieldVisualizer.SetActive(true);
        StartCoroutine(ShieldPowerDownCorotine());
    }

    IEnumerator ShieldPowerDownCorotine()
    {
        yield return new WaitForSeconds(5.0f);
        _isShieldActive = false;
        //De-behaviors
        _shieldVisualizer.SetActive(false);
    }

    public void AddScores(int points)
    {
        _scores += points;
        _uiManager.UpdateScores(_scores);
    }
}
