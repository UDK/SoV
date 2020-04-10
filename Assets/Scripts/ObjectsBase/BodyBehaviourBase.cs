using Assets.Scripts.ObjectsBase.Containers;
using Assets.Scripts.Physics.Sattellite;
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
    private IGameplayBody dataOfGameplay;

    public IBody DataOfGameplay { get => (IBody)dataOfGameplay; }

    SatelliteManagerBehavior satelliteManager;

    private void Awake()
    {
        dataOfGameplay = new BodyGameplay(this);
        satelliteManager = GetComponent<SatelliteManagerBehavior>();
        dataOfGameplay.Mass = Random.Range(1f, 500f);
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
