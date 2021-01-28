using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManagerBase
{
    private int _maxinstance;
    private int _spawnPointTotal; 
    private GameObject[] _prefabs;
    private GameManager _manager;

    #region Readonly

    public int maxInstance { get { return _maxinstance; } }
    public int spawnPointTotal { get { return _spawnPointTotal; } }
    public GameObject[] prefabs { get { return _prefabs; } }
    public GameManager manager { get { return _manager; } }

    #endregion

    public List<Vector2> spawnPos = new List<Vector2>();

    public AsteroidManagerBase(int _maxInstance, int _spawnPointTotal, GameObject[] _prefabs, GameManager _manager)
    {
        this._maxinstance = _maxInstance;
        this._spawnPointTotal = _spawnPointTotal;
        this._prefabs = _prefabs;
        this._manager = _manager;

        GetSpawnPos();
    }

    /// <summary>
    /// Get spawn position in the scene by
    /// creating a circle of point, then stretch
    /// them into a rectangle that fit the spawn area
    /// </summary>
    private void GetSpawnPos()
    {
        float defaultDeg = 360 / spawnPointTotal;

        for (int i = 0; i < spawnPointTotal; i++)
        {
            float deg = defaultDeg * i;

            float cos = Mathf.Cos(Mathf.Deg2Rad * deg);
            float sin = Mathf.Sin(Mathf.Deg2Rad * deg);

            Vector2 pos = new Vector2(
                Mathf.Clamp(manager.camArea.x * cos, -manager.camArea.x / 2 - 2, manager.camArea.x / 2 + 2),
                Mathf.Clamp(manager.camArea.y * sin, -manager.camArea.y / 2 - 1, manager.camArea.y / 2 + 1)
            );

            spawnPos.Add(pos);
        }
    }
}

public class AsteroidManager : AsteroidManagerBase
{
    private List<GameObject> availableAsteroids = new List<GameObject>();
    private List<GameObject> smallAsteroids = new List<GameObject>();

    public AsteroidManager(int maxInstance, int spawnPointTotal, GameObject[] prefabs, GameManager manager) : base(maxInstance, spawnPointTotal, prefabs, manager)
    {
        CreateStartingAsteroid();
    }

    /// <summary>
    /// Create some asteroid for object pooling
    /// </summary>
    private void CreateStartingAsteroid()
    {
        for (int i = 0; i < maxInstance; i++)
        {
            /* Will return the index back to minimum after i = length of asteroid prefab array
             * ex result: 1, 2, 3, 4, 5, 1, 2, 3 */
            int index = i - ((int)(i / prefabs.Length) * prefabs.Length);

            GameObject obj = Create(index);
            Add(obj);
        }
    }

    /// <summary>
    /// Create a new asteroid into scene
    /// </summary>
    /// <param name="type">asteroid type</param>
    /// <param name="pos">asteroid spawn position (spawn position index)</param>
    /// <returns></returns>
    public GameObject Create(int type, int pos = -1)
    {
        AsteroidController asteroids = GameObject.Instantiate(prefabs[type]).GetComponent<AsteroidController>();
        asteroids.Init(manager);

        if(type < 2) 
            smallAsteroids.Add(asteroids.gameObject); 

        if (pos >= 0)
        {
            asteroids.gameObject.SetActive(true);
            asteroids.instance.Activate(spawnPos[pos]);

            return asteroids.gameObject;
        }
        
        asteroids.gameObject.SetActive(false);
        return asteroids.gameObject;
    }

    /// <summary>
    /// Get available small asteroid in scene
    /// or create a new one
    /// </summary>
    /// <returns></returns>
    public GameObject GetSmallAsteroids()
    {
        foreach (GameObject obj in smallAsteroids)
        {
            if (!obj.activeSelf)
            {
                availableAsteroids.Remove(obj);

                return obj;
            }
        }

        int type = Random.Range(0, 2);
        GameObject asteroids = Create(type);

        smallAsteroids.Add(asteroids.gameObject);

        return asteroids.gameObject;
    }

    /// <summary>
    /// Spawn asteroid into the spawn pos
    /// and activate it
    /// </summary>
    /// <param name="pos"></param>
    public void Spawn(int pos)
    {
        int index = Random.Range(0, availableAsteroids.Count);

        availableAsteroids[index].GetComponent<AsteroidController>().instance.Activate(spawnPos[pos]);
        availableAsteroids.RemoveAt(index);
    }

    /// <summary>
    /// add new object into asteroid list in the scene
    /// </summary>
    /// <param name="obj"></param>
    public void Add(GameObject obj)
    {
        availableAsteroids.Add(obj);
    }
}
