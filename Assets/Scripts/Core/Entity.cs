using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity<T> : MonoBehaviour where T : Instance
{
    public T instance;

    public int score;
}
