using Assets.Scripts.Gameplay;
using Assets.Scripts.Helpers;
using Assets.Scripts.Manager.ClassSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarUpgrade : UpgraderBase
{
    protected override void UpgradeClass(ITemplateManager templateManager, SpaceBody spaceBody)
    {
        spaceBody.gameObject.layer = LayerHelper.Sun;
        //base.Upgrade(templateManager, spaceBody);
    }
}
