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

    private const float _maxSizePlanet = 45f;

    private float _lastSize;

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

    private void EventChangeSizeIconPlanet(int mass)
    {
        scaleFunc = true;
        float percentsMaximum = mass / (BodyPlanet.mappingUpgradeSpaceObject.criticalMassUpgrade / 100);
        _lastSize = percentsMaximum * _maxSizePlanet;
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
            foreach (var mesh in meshRendersIcon)
            {
                mesh.transform.localScale = new Vector3(Mathf.Lerp(mesh.transform.localScale.x, _lastSize, incrementationLerp * Time.deltaTime),
                                                        Mathf.Lerp(mesh.transform.localScale.y, _lastSize, incrementationLerp * Time.deltaTime),
                                                        Mathf.Lerp(mesh.transform.localScale.z, _lastSize, incrementationLerp * Time.deltaTime));
            }
            //Проверяем, что мы +- достигли нужного размера
            if (meshRendersIcon[0].transform.localScale.x < _lastSize + 0.5f ||
                meshRendersIcon[0].transform.localScale.x > _lastSize - 0.5f)
                scaleFunc = false;
        }
    }
}
