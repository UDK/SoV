using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Helpers;
using System.Linq;
using Assets.Scripts.Gameplay;

public class GetModelPlayer : MonoBehaviour
{
    private List<GameObject> meshRendersIcon = new List<GameObject>(7);

    //Для тестов сделал публичным, но его надо будет приватным сделать
    public SpaceBody BodyPlanet { get; set; }

    private bool scaleFunc = false;

    private float incrementationLerp = 0.5f;

    private float rotateIconPlanet = -0.75f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject PlayerPlanet = GameObject.FindGameObjectWithTag(EnumTags.Player);
        BodyPlanet = PlayerPlanet.GetComponent<SpaceBody>();
        BodyPlanet.NotifyChangeMass += EventChangeSizeIconPlanet;
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

    private void EventChangeSizeIconPlanet()
    {
        scaleFunc = true;
    }

    private void Update()
    {
        ChangeSizqIconPlanet();
        RotateIconPlanet();
    }

    private void RotateIconPlanet()
    {
        foreach (var mesh in meshRendersIcon)
        {
            mesh.transform.Rotate(new Vector3(0, rotateIconPlanet, 0));
        }
    }

    private void ChangeSizqIconPlanet()
    {
        if (scaleFunc)
        {
            //Все константы должны быть заменены11!!!1
            foreach (var mesh in meshRendersIcon)
            {
                mesh.transform.localScale = new Vector3(Mathf.Lerp(mesh.transform.localScale.x, 50f, incrementationLerp * Time.deltaTime),
                                                        Mathf.Lerp(mesh.transform.localScale.y, 50f, incrementationLerp * Time.deltaTime),
                                                        Mathf.Lerp(mesh.transform.localScale.z, 50f, incrementationLerp * Time.deltaTime));
            }
            if (meshRendersIcon[0].transform.localScale.x < 50.5f)
                scaleFunc = false;
        }
    }
}
