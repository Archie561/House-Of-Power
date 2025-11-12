using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameScreenController : MonoBehaviour
{
    [SerializeField] private GameScreenConfigDatabase _screenConfigsDatabase;
    [SerializeField] private NavigationBarController _navigationBarController;

    private const string HiddenScreenClassName = "game-screen-hidden";

    private VisualElement _root;
    private Dictionary<GameScreen, VisualElement> _screenElementsMap;

    private void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        if (_screenConfigsDatabase == null)
        {
            Debug.LogError("_screenConfigsDatabase не призначено в інспекторі!");
            return;
        }

        InitializeScreenElements();
        _navigationBarController.OnNavigationButtonSelected += ChangeGameScreen;
    }

    private void InitializeScreenElements()
    {
        _screenElementsMap = new Dictionary<GameScreen, VisualElement>();

        foreach (var screenConfig in _screenConfigsDatabase.ScreenConfigs)
        {
            var screenElement = _root.Q<VisualElement>(screenConfig.ScreenName);
            if (screenElement != null)
            {
                _screenElementsMap[screenConfig.GameScreenType] = screenElement;
            }
            else
            {
                Debug.LogWarning($"Screen with name {screenConfig.ScreenName} not found in UI Document.");
            }
        }
    }

    private void ChangeGameScreen(GameScreen gameScreenType)
    {
        foreach (var pair in _screenElementsMap)
        {
            if (pair.Key == gameScreenType)
                pair.Value.RemoveFromClassList(HiddenScreenClassName);
            else
                pair.Value.AddToClassList(HiddenScreenClassName);
        }
    }

    private void OnDisable()
    {
        if (_navigationBarController != null)
            _navigationBarController.OnNavigationButtonSelected -= ChangeGameScreen;
    }
}
