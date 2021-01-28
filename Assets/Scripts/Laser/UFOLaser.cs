using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOLaser : MonoBehaviour
{
    #region Public/Serializeable Variable

    public LaserData data;
    public Laser<UFO> instance;

    #endregion

    #region Private/Hidden Variable

    private float currentAliveTime = 0;         // automatically destroy this laser after currentAliveTime = 0

    #endregion

    // Update is called once per frame
    void Update()
    {
        // automatically destroy after certain amount of time
        if (currentAliveTime > 0)
        {
            currentAliveTime -= Time.deltaTime;
            instance.rigidBody.velocity = instance.direction.normalized * instance.data.speed;

            if (currentAliveTime <= 0)
            {
                instance.Destroy();
            }
        }

        instance.EucledianTorus();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Camera.main.GetComponent<CameraShake>().Shake(0.2f);
            collision.gameObject.GetComponent<Rocket>().Hit();
            instance.Destroy();
        }
    }

    /// <summary>
    /// Get Rocket reference from the scene
    /// </summary>
    /// <param name="rocket"></param>
    public void Init(UFO ufo, GameManager manager)
    {
        instance = new Laser<UFO>(manager, gameObject, ufo, data);
    }

    /// <summary>
    /// activate this laser from pos, and firing into dir
    /// </summary>
    /// <param name="pos">fire point position</param>
    /// <param name="dir">fire point direction</param>
    public void Activate(Vector2 pos, Vector2 dir)
    {
        transform.position = pos;

        instance.direction = dir;
        currentAliveTime = instance.data.aliveTime;

        gameObject.SetActive(true);
    }
}
