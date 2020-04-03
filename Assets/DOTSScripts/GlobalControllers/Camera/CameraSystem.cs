
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.DOTSScripts.GlobalControllers.Camera
{
    //[UpdateAfter(typeof(PlayersMovementSystem))]
    public class CameraSystem : JobComponentSystem
    {
        private EntityQuery m_LocalPlayer;

        protected override void OnCreateManager()
        {
            // Cached access to a set of ComponentData based on a specific query 
            m_LocalPlayer = GetEntityQuery(ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<PlayerData>());
        }

        protected override void OnCreate()
        {
        }

        //[BurstCompile]
        public struct CameraFollowJob : IJobForEach<Translation, CameraComponent>
        {
            [DeallocateOnJobCompletion]
            public NativeArray<Translation> target;


            public float deltaTime;
            public void Execute(ref Translation camPosition, [ReadOnly] ref CameraComponent c2)
            {
                // check if player exist
                if (target.Length == 0)
                    return;
                Debug.Log(camPosition.Value);
                // Follow the Player
                float2 camP = new float2(camPosition.Value.x, camPosition.Value.y); /*+ GameBootstrap.instance.offset*/;
                float2 desiredP = new float2(target[0].Value.x, target[0].Value.y); /*+ GameBootstrap.instance.offset*/;
                float2 smoothedPosition = math.lerp(camP, desiredP, /*GameBootstrap.instance.CameraSmoothSpeed **/ deltaTime);
                camPosition.Value = new float3(smoothedPosition.x, smoothedPosition.y, camPosition.Value.z);

                // Rotate Camera to the Player
                /*float3 lookVector = target[0].Value - camPosition.Value;
                Quaternion rotation = Quaternion.LookRotation(lookVector);
                camRotation.Value = rotation;*/
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new CameraFollowJob
            {
                target = m_LocalPlayer.ToComponentDataArray<Translation>(Allocator.TempJob),
                deltaTime = Time.DeltaTime
            };
            return job.Schedule(this, inputDeps);
        }
    }
}