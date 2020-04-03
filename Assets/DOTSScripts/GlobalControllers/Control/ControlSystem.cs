using Assets.DOTSScripts.GlobalControllers.InputControl;
using Assets.DOTSScripts.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;

namespace Assets.DOTSScripts.GlobalControllers.Control
{
    public class ControlSystem : JobComponentSystem
    {
        protected override void OnCreate()
        {
        }

        //[BurstCompile]
        public struct ControlJob : IJobForEachWithEntity<PhysicsVelocity, PhysicsMass, ControlComponent>
        {
            public float DeltaTime;
            public void Execute(
                Entity _entity,
                int index,
                ref PhysicsVelocity physicsVelocity,
                ref PhysicsMass physicsMass,
                ref ControlComponent playerControl)
            {
                Actions actions = ControlManager.GetCurrentActions(playerControl.Target);
                var iv = InputDecider.GetMovingVector(actions);
                var l = physicsVelocity.Linear;
                //Debug.Log(physicsVelocity.Linear);
                /*if(iv.x != 0 || iv.y != 0)
                {
                    var currentSpeed = math.sqrt(math.pow(l.x, 2) + math.pow(l.y, 2));
                    float3 speed = 0;
                    if (currentSpeed < playerControl.TargetSpeed)
                    {
                        speed = math.lerp(physicsVelocity.Linear, math.float3(iv * playerControl.TargetSpeed, 0), DeltaTime);
                    }
                    physicsVelocity.Linear = speed;
                }*/
                physicsVelocity.Linear = MovementCalculator.GetNeedVelocity(
                    physicsVelocity.Linear,
                    iv,
                    playerControl.TargetSpeed,
                    DeltaTime);
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            ControlManager.PlayerInputUpdate();
            var job = new ControlJob
            {
                DeltaTime = Time.DeltaTime
            };
            return job.Schedule(this, inputDeps);
        }
    }
}
