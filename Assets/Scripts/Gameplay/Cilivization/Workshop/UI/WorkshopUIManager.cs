using Assets.Scripts.Helpers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

        public GameObject ResourcesStack;
        #endregion

        private List<StackMapping> _templateStack =
            new List<StackMapping>();

        private List<StackMapping> _hullsStack =
            new List<StackMapping>();

        private List<StackMapping> _weaponsStack =
            new List<StackMapping>();

        private GameObject _chosenTemplate;

        private GameObject _chosenHull;

        private List<ShipTemplate> _currentReadyTemplates;

        private ShipsDatabase _shipsDatabase;

        public void Enable(
            List<ShipTemplate> ReadyTemplates,
            ShipsDatabase shipsDatabase)
        {
            this.gameObject.SetActive(true);
            _currentReadyTemplates = ReadyTemplates;
            _shipsDatabase = shipsDatabase;
            foreach(var template in _currentReadyTemplates)
            {
                var templatePanel = Instantiate(TemplatePanelOrigin, TemplatesStack.transform);
                var panel = templatePanel.transform.GetChild(0).gameObject;
                panel.GetComponent<TextMeshPro>().text = template.Name;
                _templateStack.Add(new StackMapping
                {
                    TemplateAtStack = panel,
                    SourceGameObject = template.ShipBase,
                });
            }
            foreach(var hull in shipsDatabase.Hulls)
            {
                var hullPanel = Instantiate(HullPanelOrigin, HullsStack.transform);
                var panel = hullPanel.transform.GetChild(0).gameObject;
                panel.GetComponent<RawImage>().texture =
                    Library.GetTexture(hull).RenderedTexture;
                _hullsStack.Add(new StackMapping
                {
                    TemplateAtStack = panel,
                    SourceGameObject = hull,
                });

                PlaceSpaceship(hull);
            }
        }

        private void PlaceSpaceship(GameObject hull)
        {
            var shipGraphicContainer =
                Library.GetTexture(hull, scale =>
                {
                    Dictionary<GameObject, Vector2> weapons =
                        new Dictionary<GameObject, Vector2>();
                    foreach (Transform child in hull.transform)
                    {
                        Vector3 position = child.position * scale;
                        position = Library.WorldToCameraTexture(position).Value;
                        weapons.Add(child.gameObject, position);
                    }
                    return weapons;
                });
            ShipPlacement.GetComponent<RawImage>().texture =
                shipGraphicContainer.RenderedTexture;

            List<Button> buttons = new List<Button>();
            foreach (var places in shipGraphicContainer.Storage)
            {
                var item = Instantiate(Slot, ShipPlacement.transform);
                var rTransform = item.GetComponent<RectTransform>();
                rTransform.anchoredPosition = places.Value;
                var button = rTransform.GetComponent<Button>();
                buttons.Add(button);
                button.onClick.AddListener(() =>
                {
                    button.colors = new ColorBlock
                    {
                        normalColor = button.colors.selectedColor,
                        highlightedColor = button.colors.selectedColor,
                        pressedColor = button.colors.selectedColor,
                        disabledColor = Color.white,
                        colorMultiplier = 1,
                        selectedColor = button.colors.selectedColor,
                    };
                    buttons.ForEach(b =>
                    {
                        if (b != button)
                        {
                            b.colors = new ColorBlock
                            {
                                normalColor = Color.white,
                                highlightedColor = Color.white,
                                pressedColor = Color.white,
                                disabledColor = Color.white,
                                colorMultiplier = 1,
                                selectedColor = button.colors.selectedColor,
                            };
                        }
                    });
                    SlotClick(places.Key);
                });
            }
        }

        private void SlotClick(GameObject weapon)
        {
            WeaponsStack.DestroyAllChilds();
            _weaponsStack.Clear();
            var shells = _shipsDatabase.GetShells(weapon);
            foreach (var shell in shells)
            {
                var weaponPanel = Instantiate(WeaponItemOrigin, WeaponsStack.transform);
                var panel = weaponPanel.transform.GetChild(0).gameObject;
                panel.GetComponent<RawImage>().texture =
                    Library.GetTexture(shell).RenderedTexture;
                _weaponsStack.Add(new StackMapping
                {
                    TemplateAtStack = panel,
                    SourceGameObject = shell,
                });
            }
        }

        private class Resources
        {
            public TextMeshPro Money;

            public TextMeshPro Merilium;

            public TextMeshPro Titan;

            public TextMeshPro Uranus;

            public TextMeshPro Time;
        }

        private class StackMapping
        {
            public GameObject TemplateAtStack;

            public GameObject SourceGameObject;
        }
    }
}
