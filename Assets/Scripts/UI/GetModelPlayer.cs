using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Helpers;
using System.Linq;
using Assets.Scripts.Gameplay;
using System.Threading.Tasks;
using System.Threading;

public class GetModelPlayer : MonoBehaviour
{
    private List<GameObject> meshRendersIcon = new List<GameObject>(7);

    //Для тестов сделал публичным, но его надо будет приватным сделать
    public SpaceBody BodyPlanet { get; set; }

    private bool scaleFunc = false;

    private float incrementationLerp = 0.5f;

    private float rotateIconPlanet = -0.75f;

    private const float _maxSizePlanet = 45f;
    /// <summary>
    /// Нужна, что при достижении максимальной массы 
    /// </summary>
    private const float _upMaxPrecent = 3f;

    private float _lastSize;

    private float _pastMass;
    /// <summary>
    /// Определяет масса увеличилась или уменьшилась
    /// </summary>
    private bool _weightGain;


    // Start is called before the first frame update
    void Start()
    {
        GameObject PlayerPlanet = GameObject.FindGameObjectWithTag(EnumTags.Player);
        BodyPlanet = PlayerPlanet.GetComponent<SpaceBody>();
        BodyPlanet.NotifyChangeMass += EventChangeSizeIconPlanet;
        _pastMass = BodyPlanet.Mass;
        MeshRenderer[] meshRenderers = PlayerPlanet.GetComponentsInChildren<MeshRenderer>().Skip(1).ToArray();
        foreach (var meshPlayerPlanet in meshRenderers)
        {
            GameObject meshRenderIcon = Instantiate(meshPlayerPlanet.gameObject);
            meshRenderIcon.transform.SetParent(transform);
            meshRenderIcon.transform.localPosition = new Vector3(0, 0, 0);
            meshRenderIcon.transform.localScale = new Vector3(10f, 10f, 10f);
            meshRenderIcon.layer = LayerHelper.UI;
            meshRendersIcon.Add(meshRenderIcon);
        }
    }

    private void EventChangeSizeIconPlanet(int mass)
    {
        scaleFunc = true;
        float percentsMaximum = mass / ((BodyPlanet.mappingUpgradeSpaceObject.criticalMassUpgrade + _upMaxPrecent) / 100);
        _lastSize = percentsMaximum / 100 * _maxSizePlanet;
        if (mass > _pastMass)
            _weightGain = true;
        else
            _weightGain = false;
        _pastMass = mass;

    }

    private async void Update()
    {
        ChangeSizqIconPlanet();
        await RotateIconPlanet();
    }

    private async Task RotateIconPlanetAsync()
    {
        foreach (var mesh in meshRendersIcon)
        {
            mesh.transform.Rotate(new Vector3(0, rotateIconPlanet, 0));
            await Task.Yield();
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
            if (meshRendersIcon[0].transform.localScale.x < _lastSize + 0.5f &&
                _weightGain == false)
                scaleFunc = false;
            else if (meshRendersIcon[0].transform.localScale.x > _lastSize - 0.5f &&
                    _weightGain == true)
                scaleFunc = false;
        }
    }
}
