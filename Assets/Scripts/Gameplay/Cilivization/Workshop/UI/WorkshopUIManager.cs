using Assets.Scripts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Library = Assets.Scripts.Helpers.LibraryOfRenderedGameobjects<
    System.Collections.Generic.Dictionary<UnityEngine.GameObject, UnityEngine.Vector2>>;

namespace Assets.Scripts.Gameplay.Cilivization.Workshop.UI
{
    public class WorkshopUIManager : MonoBehaviour
    {
        #region origins
        public GameObject TemplatePanelOrigin;

        public GameObject HullPanelOrigin;

        public GameObject WeaponItemOrigin;
        #endregion

        #region Ship
        public GameObject ShipPlacement;

        public GameObject Slot;
        #endregion

        #region Stacks
        public GameObject TemplatesStack;

        public GameObject HullsStack;

        public GameObject WeaponsStack;

        public Resources ResourcesStack;

        public Overview OverviewStack;
        #endregion

        #region Template manager
        public GameObject NameInput;

        #endregion

        private List<StackMapping<ShipTemplate>> _templateStack =
            new List<StackMapping<ShipTemplate>>();

        private List<StackMapping<GameObject>> _hullsStack =
            new List<StackMapping<GameObject>>();

        #region Button mapper
        /// <summary>
        /// key is stack
        /// value is chosen button
        /// </summary>
        private Dictionary<GameObject, GameObject> _chosenButtons =
            new Dictionary<GameObject, GameObject>();
        #endregion

        private List<ShipTemplate> _currentReadyTemplates;

        private ShipsDatabase _shipsDatabase;

        public void Enable(
            List<ShipTemplate> ReadyTemplates,
            ShipsDatabase shipsDatabase)
        {
            this.gameObject.SetActive(true);
            _currentReadyTemplates = ReadyTemplates;
            _shipsDatabase = shipsDatabase;
            AddHulls(shipsDatabase);
            SetUpNameField();
            AddTemplatesAndGetFirst(shipsDatabase);
        }

        private void SetUpNameField()
        {
            var tmp = NameInput.GetComponent<TMP_InputField>();
            tmp.onValueChanged.AddListener((text) =>
            {
                var chosenTemplate = _chosenButtons[TemplatesStack];
                _templateStack.First(x =>
                    x.TemplateAtStack == chosenTemplate)
                        .Source.Name = text;
                GetTMPText(chosenTemplate.transform.GetChild(0).gameObject).text = text;
            });
        }

        public void AddNewTemplate()
        {
            _currentReadyTemplates.Add(new ShipTemplate
            {
                Name = "My new spaceship" + (_currentReadyTemplates.Count + 1),
            });
            var templatePanel = Instantiate(TemplatePanelOrigin, TemplatesStack.transform);
            _templateStack.Add(new StackMapping<ShipTemplate>
            {
                TemplateAtStack = templatePanel,
                Source = _currentReadyTemplates.Last(),
            });
            var button = FirstSetUpOfButtonOnTemplate(templatePanel);
            button.onClick.Invoke();
        }

        public void RemoveTemplate()
        {
            if(_templateStack.Count < 2)
            {
                return;
            }

            var chosenTemplate = _chosenButtons[TemplatesStack];
            var container = _templateStack.First(x =>
                     x.TemplateAtStack == chosenTemplate);
            var source = container.Source;
            _currentReadyTemplates.Remove(source);
            _templateStack.Remove(container);
            DestroyImmediate(chosenTemplate.gameObject);
            _templateStack.First().TemplateAtStack.GetComponent<Button>().onClick.Invoke();
        }

        private void AddTemplatesAndGetFirst(
            ShipsDatabase shipsDatabase)
        {
            foreach (var template in _currentReadyTemplates)
            {
                var templatePanel = Instantiate(TemplatePanelOrigin, TemplatesStack.transform);
                var panel = templatePanel.transform.GetChild(0).gameObject;
                panel.GetComponent<TextMeshPro>().text = template.Name;
                _templateStack.Add(new StackMapping<ShipTemplate>
                {
                    TemplateAtStack = templatePanel,
                    Source = template,
                });
                var button = FirstSetUpOfButtonOnTemplate(templatePanel);
                if(_templateStack.Count == 1)
                {
                    button.onClick.Invoke();
                }
            }
            if(_templateStack.Count == 0)
            {
                AddNewTemplate();
            }
        }

        private Button FirstSetUpOfButtonOnTemplate(GameObject templatePanel)
        {
            var button = templatePanel.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                var container = _templateStack.First(x =>
                    x.TemplateAtStack == templatePanel);
                GameObject hull = container.Source.Hull;
                MakeButtonActive(TemplatesStack, button);
                _hullsStack.First(x => hull == null || x.Source == hull)
                    .TemplateAtStack.GetComponent<Button>().onClick.Invoke();
                GetTMPInput(NameInput).text = container.Source.Name;
            });

            return button;
        }

        private void AddHulls(
            ShipsDatabase shipsDatabase)
        {
            foreach (var hull in shipsDatabase.Hulls)
            {
                var hullPanel = Instantiate(HullPanelOrigin, HullsStack.transform);
                var panel = hullPanel.transform.GetChild(0).gameObject;
                panel.GetComponent<RawImage>().texture =
                    Library.GetTexture(hull).RenderedTexture;
                _hullsStack.Add(new StackMapping<GameObject>
                {
                    TemplateAtStack = hullPanel,
                    Source = hull,
                });
                var button = hullPanel.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    MakeButtonActive(HullsStack, button);
                    var chosenTemplate = _chosenButtons[TemplatesStack];
                    var currentShipTemplate = _templateStack.First(x =>
                        x.TemplateAtStack == chosenTemplate)
                            .Source;
                    currentShipTemplate.SetNewHull(hull);
                    PlaceSpaceship(hull, currentShipTemplate, shipsDatabase);
                });
            }
        }

        private void PlaceSpaceship(
            GameObject hull,
            ShipTemplate shipTemplate,
            ShipsDatabase shipsDatabase)
        {
            var shipGraphicContainer =
                Library.GetTexture(hull, scale =>
                {
                    Dictionary<GameObject, Vector2> weapons =
                        new Dictionary<GameObject, Vector2>();
                    foreach (WeaponTemplate wt in shipTemplate.Weapons)
                    {
                        Vector3 position = wt.Weapon.transform.position * scale;
                        position = Library.WorldToCameraTexture(position).Value;
                        weapons.Add(wt.Weapon.gameObject, position);
                    }
                    return weapons;
                });
            ShipPlacement.GetComponent<RawImage>().texture =
                shipGraphicContainer.RenderedTexture;

            ShipPlacement.DestroyAllChilds();
            WeaponsStack.DestroyAllChilds();
            foreach (var places in shipGraphicContainer.Storage)
            {
                var slot = Instantiate(Slot, ShipPlacement.transform);
                var rTransform = slot.GetComponent<RectTransform>();
                rTransform.anchoredPosition = places.Value;
                var button = rTransform.GetComponent<Button>();
                var panel = slot.transform.GetChild(0).gameObject;
                var image = panel.GetComponent<RawImage>();

                // choose shell if there is no
                var weaponOfShipTemplate = shipTemplate.Weapons.First(x => x.Weapon == places.Key);
                if (weaponOfShipTemplate.ChosenShell == null)
                {
                    weaponOfShipTemplate.ChosenShell = shipsDatabase.WeaponMappings
                                .First(x => x.Weapon.tag == places.Key.tag).AvailableShells[0];
                }

                button.onClick.AddListener(() =>
                {
                    MakeButtonActive(ShipPlacement, button);
                    SlotClick(places.Key, weaponOfShipTemplate, image);
                });
                image.texture =
                    Library.GetTexture(weaponOfShipTemplate.ChosenShell).RenderedTexture;
            }
        }

        private void SlotClick(
            GameObject weapon,
            WeaponTemplate weaponTemplate,
            RawImage slotImage)
        {
            WeaponsStack.DestroyAllChilds();
            var shells = _shipsDatabase.GetShells(weapon);
            foreach (var shell in shells)
            {
                var weaponPanel = Instantiate(WeaponItemOrigin, WeaponsStack.transform);
                var panel = weaponPanel.transform.GetChild(0).gameObject;
                panel.GetComponent<RawImage>().texture =
                    Library.GetTexture(shell).RenderedTexture;

                var button = weaponPanel.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    MakeButtonActive(WeaponsStack, button);
                    slotImage.texture = Library.GetTexture(shell).RenderedTexture;
                    weaponTemplate.ChosenShell = shell;
                });
                if(weaponTemplate.ChosenShell == shell)
                {
                    button.onClick.Invoke();
                }
            }
        }

        private void MakeButtonActive(
            GameObject stack,
            Button button)
        {
            if (!_chosenButtons.ContainsKey(stack))
            {
                _chosenButtons.Add(stack, button.gameObject);
            }
            else
            {
                if(_chosenButtons[stack].ToString() != "null")
                {
                    _chosenButtons[stack].GetComponent<Button>().colors = new ColorBlock
                    {
                        normalColor = Color.white,
                        highlightedColor = Color.white,
                        pressedColor = button.colors.pressedColor,
                        disabledColor = Color.white,
                        colorMultiplier = 1,
                        selectedColor = Color.white,
                    };
                }
                _chosenButtons[stack] = button.gameObject;
            }
            button.colors = new ColorBlock
            {
                normalColor = button.colors.pressedColor,
                highlightedColor = button.colors.pressedColor,
                pressedColor = button.colors.pressedColor,
                disabledColor = Color.white,
                colorMultiplier = 1,
                selectedColor = button.colors.pressedColor,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TMP_Text GetTMPText(
            GameObject go) =>
            go.GetComponent<TMP_Text>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TMP_InputField GetTMPInput(
            GameObject go) =>
            go.GetComponent<TMP_InputField>();

        [Serializable]
        public class Resources
        {
            public GameObject Money;

            public GameObject Merilium;

            public GameObject Titan;

            public GameObject Uranus;

            public GameObject Time;

            private List<Cost> _costs =
                new List<Cost>();

            public void Calc()
            {

            }
        }

        [Serializable]
        public class Overview
        {
            public GameObject HP;

            public GameObject Speed;

            public GameObject Distance;

            public GameObject AttackPower;

            public GameObject Type;
        }

        private class StackMapping<TSource>
        {
            public GameObject TemplateAtStack;

            public TSource Source;
        }
    }
}
