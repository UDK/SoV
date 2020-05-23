using Assets.Scripts.Gameplay;
using Assets.Scripts.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Manager.ClassSystem
{
    public interface ITemplateManager
    {
        SpaceBody GenerateSpaceBody { get; }

        GravitationBehaviour SetUpGravitation(SpaceBody spaceBody);
    }
}
