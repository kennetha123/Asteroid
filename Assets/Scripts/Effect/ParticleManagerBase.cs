using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManagerBase 
{
    private int _maxInstance;
    private GameObject _prefab;
    private GameManager _manager;

    public int maxInstance { get { return _maxInstance; } }
    public GameObject prefab { get { return _prefab; } }
    public GameManager manager { get { return _manager; } }

    public ParticleManagerBase(int _maxInstance, GameObject _prefab, GameManager _manager)
    {
        this._maxInstance = _maxInstance;
        this._prefab = _prefab;
        this._manager = _manager;
    }
}

public class ParticleManager : ParticleManagerBase
{
    private List<ParticleSystem> availableExplosionParticle = new List<ParticleSystem>();

    public ParticleManager(int maxInstance, GameObject prefab, GameManager manager) : base(maxInstance, prefab, manager)
    {
        CreateStartingParticle();
    }

    /// <summary>
    /// spawn some particle in the scene for object pooling
    /// </summary>
    public void CreateStartingParticle()
    {
        for (int i = 0; i < maxInstance; i++)
        {
            GameObject particle = Create();
            particle.SetActive(false);
        }
    }

    /// <summary>
    /// create a new particle gameobject in the scene
    /// </summary>
    /// <returns></returns>
    public GameObject Create()
    {
        GameObject particle = GameObject.Instantiate(prefab);
        particle.GetComponent<Particle>().Init(this);

        Add(particle);

        return particle;
    }

    /// <summary>
    /// Spawn this particle into a position and activate it
    /// </summary>
    /// <param name="pos"></param>
    public void Spawn(Vector2 pos)
    {
        try
        {
            Particle particle = availableExplosionParticle[Random.Range(0, availableExplosionParticle.Count)].GetComponent<Particle>();
            particle.Activate(pos);

            availableExplosionParticle.Remove(particle.GetComponent<ParticleSystem>());
        }
        catch
        {
            Particle particle = Create().GetComponent<Particle>();
            particle.Activate(pos);
        }
    }

    /// <summary>
    /// adding new obj into list of partcile in scene
    /// </summary>
    /// <param name="obj"></param>
    public void Add(GameObject obj)
    {
        availableExplosionParticle.Add(obj.GetComponent<ParticleSystem>());
    }
}
