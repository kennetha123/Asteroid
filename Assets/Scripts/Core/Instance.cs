using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instance
{
    private GameManager _manager;
    private GameObject  _gameObject;

    public GameManager manager { get { return _manager; } }
    public Rigidbody2D rigidBody { get { return _gameObject.GetComponent<Rigidbody2D>(); } }
    public GameObject gameObject { get { return _gameObject; } }
    public Transform transform { get { return _gameObject.transform; } }

    public string destructableTag = "Destroyable";

    public Instance(GameManager _manager, GameObject _gameObject)
    {
        this._manager = _manager;
        this._gameObject = _gameObject;
    }

    public void Explode()
    {
        manager.particleManager.Spawn(transform.position);
    }

    public bool EucledianTorus()
    {
        if (transform.position.x > manager.camArea.x / 2 + 1)
        {
            transform.position = new Vector3(
                -manager.camArea.x / 2,
                transform.position.y
            );

            return true;
        }
        else if (transform.position.x < -manager.camArea.x / 2 - 1)
        {
            transform.position = new Vector3(
                manager.camArea.x / 2,
                transform.position.y
            );

            return true;
        }
        else if (transform.position.y > manager.camArea.y / 2 + 1)
        {
            transform.position = new Vector3(
                transform.position.x,
                -manager.camArea.y / 2
            );

            return true;
        }
        else if (transform.position.y < -manager.camArea.y / 2 - 1)
        {
            transform.position = new Vector3(
                transform.position.x,
                manager.camArea.y / 2
            );

            return true;
        }

        return false;
    }
}

public class Controller : Instance
{
    public List<GameObject> availableLaser = new List<GameObject>();

    public Controller(GameManager manager, GameObject gameObject) : base(manager, gameObject)
    {

    }

    public void Destroy()
    {
        Explode();

        if(availableLaser.Count > 0)
        {
            foreach (GameObject obj in availableLaser)
            {
                GameObject.Destroy(obj);
            }
        }

        GameObject.Destroy(gameObject);
    }
}