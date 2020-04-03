using Assets.DOTSScripts.GlobalControllers.InputControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Jobs;

namespace Assets.DOTSScripts.GlobalControllers.Control
{
    [GenerateAuthoringComponent]
    public struct ControlComponent : IComponentData
    {
        public float TargetSpeed;
        public WhoIs Target;
    }
}
