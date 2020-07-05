using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Helpers;
using System.Linq;
using Assets.Scripts.Gameplay;
using System.Linq;

public class GetModelPlayer : MonoBehaviour
{
    List<GameObject> meshRendersIcon = new List<GameObject>(7);

    private SpaceBody BodyPlanet { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        GameObject PlayerPlanet = GameObject.FindGameObjectWithTag(EnumTags.Player);
        BodyPlanet = PlayerPlanet.GetComponent<SpaceBody>();
        BodyPlanet.NotifyChangeMass += ChangeSizeIconPlanet;
        MeshRenderer[] meshRenderers = PlayerPlanet.GetComponentsInChildren<MeshRenderer>().Skip(1).ToArray();
        foreach (var meshPlayerPlanet in meshRenderers)
        {
            GameObject meshRenderIcon = Instantiate(meshPlayerPlanet.gameObject);
            meshRenderIcon.transform.SetParent(transform);
            meshRenderIcon.transform.localPosition = new Vector3(0, 0, 0);
            meshRenderIcon.layer = LayerHelper.UI;
            meshRendersIcon.Add(meshRenderIcon);
        }
    }

    private void ChangeSizeIconPlanet()
    {
        meshRendersIcon.Select(k => k.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
