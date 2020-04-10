using Assets.Scripts.Physics.Sattellite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Игровой объект
/// </summary>
public class BodyBehaviourBase : MonoBehaviour, ISatelliteBody
{
    [SerializeField]
    private float mass;


    SatelliteManagerBehavior satelliteManager;

    public float Mass => mass;

    private void Awake()
    {
        satelliteManager = GetComponent<SatelliteManagerBehavior>();
        mass = Random.Range(1f, 500f);
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

    public void Destroy()
    {
        this.Destroy();
    }
}
