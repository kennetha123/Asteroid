using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Public/Serializeable Variable

    public GameUI gameUI;

    [Header("Object")]
    public GameObject rocketPrefab;
    public GameObject[] ufo;
    public ParticleSystem explosionParticle;
    public GameObject[] asteroidPrefab;

    [Header("Setup")]
    [Tooltip("Starting health point.")]
    public int startingHealth = 3;
    [Tooltip("Maximum instance of asteroid wander around in the game.")]
    public int asteroidMaxInstance;
    public int particleMaxInstance = 10;
    public int maxUFOInstance = 2;

    [Tooltip("the amount of asteroids spawned in 1 second")]
    public float spawnRate = 5;

    [Tooltip("total of spawn point")]
    public int spawnPointTotal = 30;

    public AsteroidManager asteroidManager;
    public ParticleManager particleManager;

    #endregion

    #region Private/Hidden Variable

    private int defaultAsteroidMaxInstance;

    private float ufoSpawnTime = 20;
    private float defaultSpawnRate;
    private float spawnTime = 0;

    [HideInInspector]
    public Vector2 camArea = new Vector2();

    [HideInInspector] public int currentScore;
    [HideInInspector] public int asteroidCounter; 
    [HideInInspector] public int ufoCounter; 
    [HideInInspector] public int currentHealth;
    #endregion

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        spawnRate = defaultSpawnRate + ((float)currentScore / 100000f);
        asteroidMaxInstance = defaultAsteroidMaxInstance + ((int)currentScore / 5000);

        if (spawnTime > 0 && asteroidCounter < asteroidMaxInstance)
        {
            spawnTime -= Time.deltaTime;

            if (spawnTime <= 0)
            {
                int pos = Random.Range(0, spawnPointTotal);
                asteroidCounter++;

                try
                {
                    asteroidManager.Spawn(pos);
                }
                catch
                {
                    int type = Random.Range(0, asteroidPrefab.Length);
                    asteroidManager.Create(type, pos);
                }

                // reset timer
                spawnTime = 1f / spawnRate;
            }
        }
        else
        {
            spawnTime = 1f / spawnRate;
        }

        if (currentScore >= 5000)
        {
            if (ufoSpawnTime > 0 && ufoCounter < maxUFOInstance)
            {
                ufoSpawnTime -= Time.deltaTime;

                if (ufoSpawnTime <= 0)
                {
                    CreateUFO();

                    ufoSpawnTime = 20;
                }
            }
            else
            {
                ufoSpawnTime = 20;
            }
        }
    }

    public void Init()
    {
        GetCameraArea();
        SpawnPlayer();

        asteroidManager = new AsteroidManager(asteroidMaxInstance, spawnPointTotal, asteroidPrefab, this);
        particleManager = new ParticleManager(particleMaxInstance, explosionParticle.gameObject, this);

        // set default spawn rate
        defaultSpawnRate = spawnRate;

        // set default asteroid max instance
        defaultAsteroidMaxInstance = asteroidMaxInstance;

        // reset spawn time
        spawnTime = 1f / spawnRate;

        // reset asteroid counter
        asteroidCounter = 0;
    }

    /// <summary>
    /// Get Area that visible to the camera
    /// </summary>
    private void GetCameraArea()
    {
        Camera cam = Camera.main;

        float y = 2 * cam.orthographicSize;
        float x = y * cam.aspect;

        camArea = new Vector2(x, y);
    }

    /// <summary>
    /// spawn player instance into scene
    /// </summary>
    public void SpawnPlayer()
    {
        Rocket player = Instantiate(rocketPrefab).GetComponent<Rocket>();
        player.Init(this);
    }

    public void CreateUFO(int type = -1)
    {
        int pos = Random.Range(0, asteroidManager.spawnPos.Count);
        
        if(type < 0)
            type = Random.Range(0, this.ufo.Length);

        GameObject ufo = Instantiate(this.ufo[type], asteroidManager.spawnPos[pos], Quaternion.identity);

        switch (type)
        {
            case 0:
                ufo.GetComponent<BigUFOController>().Init(this);
                break;

            case 1:
                ufo.GetComponent<SmallUFOController>().Init(this);
                break;
        }

        ufoCounter++;

    }

    public void GameOver()
    {
        if (gameUI.PauseUI.activeSelf)
        {
            Time.timeScale = 1;
            gameUI.PauseUI.SetActive(false);
        }

        gameUI.PlayingUI.SetActive(false);
        gameUI.GameOverUI.SetActive(true);

        gameUI.finalScoreText.text = string.Format("Final Score: {0}", currentScore);
    }
}
