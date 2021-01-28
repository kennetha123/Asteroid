using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private ParticleManager manager;

    // called when this particle stopped
    private void OnParticleSystemStopped()
    {
        Destroy();
    }

    public void Init(ParticleManager manager)
    {
        this.manager = manager;
    }

    /// <summary>
    /// activate this particle in a position
    /// </summary>
    /// <param name="pos"></param>
    public void Activate(Vector2 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;
        GetComponent<ParticleSystem>().Play();
    }

    /// <summary>
    /// Remove this particle from sight
    /// </summary>
    public void Destroy()
    {
        gameObject.SetActive(false);
        manager.Add(gameObject);
    }
}
