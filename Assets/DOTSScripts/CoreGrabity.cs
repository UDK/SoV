using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;

public class CoreGrabity : MonoBehaviour
{
    NativeList<Entity> entityList;

    void Awake()
    {
        NativeList<Entity> entityList = new NativeList<Entity>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EntityManager entity = World.AllWorlds[0].EntityManager;
        EntityArchetype archetype = entity.CreateArchetype(typeof(LocalToWorld));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
