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
using Assets.Scripts.Gameplay.Cilivization.Descriptions;
using Assets.Scripts.Manager;

namespace Assets.Scripts.Gameplay.Cilivization.Workshop.UI
{
    public class WorkshopUIManager : MonoBehaviour
    {
        public delegate void OnClose();

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

        private List<StackMapping<ShipBuildTemplate>> _templateStack =
            new List<StackMapping<ShipBuildTemplate>>();

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

        private List<ShipBuildTemplate> _currentReadyTemplates;

        private Database _shipsDatabase;

        private OnClose _onClose;
        public void Enable(
            List<ShipBuildTemplate> ReadyTemplates,
            Database shipsDatabase,
            OnClose onClose)
        {
            GameManager.Pause = true;
            this.gameObject.SetActive(true);
            _onClose = onClose;
            _currentReadyTemplates = ReadyTemplates;
            _shipsDatabase = shipsDatabase;
            AddHulls(shipsDatabase);
            SetUpNameField();
            AddTemplatesAndGetFirst(shipsDatabase);
        }

        public void Disable()
        {
            _onClose();
            this.gameObject.SetActive(false);
            _currentReadyTemplates = null;
            _shipsDatabase = null;
            GameManager.Pause = false;
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

        private void AddTemplatesAndGetFirst(
            Database shipsDatabase)
        {
            foreach (var template in _currentReadyTemplates)
            {
                AddNewTemplate(template);
            }
            if (_templateStack.Count != 0)
            {
                _templateStack.First().TemplateAtStack.GetComponent<Button>().onClick.Invoke();
            }
            else if (_templateStack.Count == 0)
            {
                AddNewTemplate();
            }
        }

        private void AddNewTemplate(
            ShipBuildTemplate readyTemplate = null)
        {
            if(readyTemplate == null)
            {
                readyTemplate = new ShipBuildTemplate
                {
                    Name = "My new spaceship" + (_currentReadyTemplates.Count + 1),
                };
                _currentReadyTemplates.Add(readyTemplate);
            }
            var templatePanel = Instantiate(TemplatePanelOrigin, TemplatesStack.transform);
            _templateStack.Add(new StackMapping<ShipBuildTemplate>
            {
                TemplateAtStack = templatePanel,
                Source = readyTemplate,
            });
            var button = FirstSetUpOfButtonOnTemplate(templatePanel);
            button.onClick.Invoke();
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
                _hullsStack.First(x => hull == null || x.Source.name == hull.name)
                    .TemplateAtStack.GetComponent<Button>().onClick.Invoke();
                GetTMPInput(NameInput).text = container.Source.Name;
            });

            return button;
        }

        public void RemoveTemplate()
        {
            if (_templateStack.Count < 2)
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

        private void AddHulls(
            Database shipsDatabase)
        {
            foreach (var hull in shipsDatabase.Hulls)
            {
                var hullPanel = Instantiate(HullPanelOrigin, HullsStack.transform);
                var panel = hullPanel.transform.GetChild(0).gameObject;
                panel.GetComponent<RawImage>().texture =
                    hull.GetComponent<Description>().Image;//Library.GetTexture(hull).RenderedTexture;
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
            ShipBuildTemplate shipTemplate,
            Database shipsDatabase)
        {
            var shipGraphicContainer =
                Library.GetTexture(hull, scale =>
                {
                    var sizes = Library.GetSizes;
                    var rectT = ShipPlacement.GetComponent<RectTransform>();
                    var xScale = rectT.rect.width / sizes.x;
                    var yScale = rectT.rect.height / sizes.y;
                    Dictionary<GameObject, Vector2> weapons =
                        new Dictionary<GameObject, Vector2>();
                    foreach (ModuleTemplate wt in shipTemplate.Modules)
                    {
                        Vector3 position = wt.ModuleSlot.transform.position * scale;
                        position = Library.WorldToCameraTexture(position).Value;
                        position = new Vector2(position.x * xScale, position.y * yScale);
                        weapons.Add(wt.ModuleSlot.gameObject, position);
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
                var modulesOfShipTemplate = shipTemplate.Modules.First(x => x.ModuleSlot == places.Key);
                if (modulesOfShipTemplate.ChosenModule == null)
                {
                    modulesOfShipTemplate.ChosenModule = shipsDatabase.ModuleMappings
                                .First(x => x.ModuleSlot.name == places.Key.name).AvailableModules[0];
                }

                button.onClick.AddListener(() =>
                {
                    MakeButtonActive(ShipPlacement, button);
                    SlotClick(places.Key, shipTemplate, modulesOfShipTemplate, image);
                });
                image.texture =
                    modulesOfShipTemplate.ChosenModule.GetComponent<Description>().Image;
            }
            CalculateOverview(shipTemplate);
        }

        private void SlotClick(
            GameObject weapon,
            ShipBuildTemplate shipTemplate,
            ModuleTemplate weaponTemplate,
            RawImage slotImage)
        {
            WeaponsStack.DestroyAllChilds();
            var shells = _shipsDatabase.GetShells(weapon);
            foreach (var shell in shells)
            {
                var weaponPanel = Instantiate(WeaponItemOrigin, WeaponsStack.transform);
                var panel = weaponPanel.transform.GetChild(0).gameObject;
                panel.GetComponent<RawImage>().texture =
                    shell.GetComponent<Description>().Image;

                var button = weaponPanel.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    MakeButtonActive(WeaponsStack, button);
                    slotImage.texture =
                        shell.GetComponent<Description>().Image;
                    weaponTemplate.ChosenModule = shell;
                    CalculateOverview(shipTemplate);
                });
                if(weaponTemplate.ChosenModule == shell)
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

        private void CalculateOverview(
            ShipBuildTemplate shipTemplate)
        {
            shipTemplate.CalculateCharacteristics();
            shipTemplate.CalculateCost();
            GetTMPText(ResourcesStack.Merilium).text = shipTemplate.Cost.Merilium.ToString();
            GetTMPText(ResourcesStack.Money).text = shipTemplate.Cost.Money.ToString();
            GetTMPText(ResourcesStack.Uranus).text = shipTemplate.Cost.Uranus.ToString();
            GetTMPText(ResourcesStack.Titan).text = shipTemplate.Cost.Titan.ToString();
            GetTMPText(ResourcesStack.Time).text = shipTemplate.Cost.Time + " s";
            GetTMPText(OverviewStack.HP).text = shipTemplate.ShipCharacteristics.HP.ToString();
            GetTMPText(OverviewStack.Distance).text =
                $"{shipTemplate.ShipCharacteristics.MinAttackDistance} - {shipTemplate.ShipCharacteristics.MaxAttackDistance} Gm";
            GetTMPText(OverviewStack.Speed).text = $"{shipTemplate.ShipCharacteristics.Speed * 10} Gm/s";
            GetTMPText(OverviewStack.Type).text = shipTemplate.Hull.GetComponent<AI.SpaceShipAI>().StrategyType.ToString();
            GetTMPText(OverviewStack.AttackPower).text = 
                (shipTemplate.ShipCharacteristics.MaxAttackDistance *
                shipTemplate.ShipCharacteristics.HP *
                shipTemplate.ShipCharacteristics.Speed).ToString();
        }

        [Serializable]
        public class Resources
        {
            public GameObject Money;

            public GameObject Merilium;

            public GameObject Titan;

            public GameObject Uranus;

            public GameObject Time;
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
