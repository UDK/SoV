using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Helpers;
using System.Linq;

public class GetModelPlayer : MonoBehaviour
{
    private GameObject PlayerPlanet { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        PlayerPlanet = GameObject.FindGameObjectWithTag(EnumTags.Player);
        MeshRenderer[] meshRenderers = PlayerPlanet.GetComponentsInChildren<MeshRenderer>().Skip(1).ToArray();
        foreach (var meshPlayerPlanet in meshRenderers)
        {
            GameObject meshRenderIcon = Instantiate(meshPlayerPlanet.gameObject);
            meshRenderIcon.transform.SetParent(transform);
            meshRenderIcon.transform.localPosition = new Vector3(0, 0, 0);
            meshRenderIcon.layer = LayerHelper.UI;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
