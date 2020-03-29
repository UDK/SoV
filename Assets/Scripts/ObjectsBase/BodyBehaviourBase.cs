using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Игровой объект
/// </summary>
public class BodyBehaviourBase : MonoBehaviour
{
    [SerializeField]
    public float Mass;

    private void Awake()
    {
        Mass = Random.Range(1f, 500f);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
