using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class NavigationBarController : MonoBehaviour
{
    [SerializeField] private GameScreenConfigDatabase _screenConfigsDatabase;

    public event Action<GameScreen> OnNavigationButtonSelected;

    private const string SelectedNavigationButtonClassName = "navigation-button-selected";

    private VisualElement _root;
    private Dictionary<Button, GameScreen> _buttonScreenMap;

    private void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        if (_screenConfigsDatabase == null)
        {
            Debug.LogError("_screenConfigsDatabase не призначено в інспекторі!");
            return;
        }

        InitializeNavigationButtons();
    }

    //Гарантує, що кнопка , відповідна екрану за замовчуванням, буде вибрана тільки після того як всі OnEnable завершаться.
    private void Start() => UpdateSelectedButton(_buttonScreenMap.FirstOrDefault(x => x.Value == _screenConfigsDatabase.DefaultGameScreen).Key);

    private void InitializeNavigationButtons()
    {
        _buttonScreenMap = new Dictionary<Button, GameScreen>();

        foreach (var screenConfig in _screenConfigsDatabase.ScreenConfigs)
        {
            var button = _root.Q<Button>(screenConfig.ButtonName);
            if (button != null)
            {
                _buttonScreenMap[button] = screenConfig.GameScreenType;
                button.RegisterCallback<ClickEvent>(OnNavigationButtonClick);
            }
            else
            {
                Debug.LogWarning($"Button with name {screenConfig.ButtonName} not found in UI Document.");
            }
        }
    }

    private void OnNavigationButtonClick(ClickEvent evt) => UpdateSelectedButton(evt.currentTarget as Button);

    private void UpdateSelectedButton(Button selectedButton)
    {
        if (selectedButton.ClassListContains(SelectedNavigationButtonClassName))
            return;

        foreach (var button in _buttonScreenMap.Keys)
            button.RemoveFromClassList(SelectedNavigationButtonClassName);

        selectedButton.AddToClassList(SelectedNavigationButtonClassName);
        OnNavigationButtonSelected?.Invoke(_buttonScreenMap[selectedButton]);
    }

    private void OnDisable()
    {
        if (_buttonScreenMap == null) return;

        foreach (var button in _buttonScreenMap.Keys)
        {
            button.UnregisterCallback<ClickEvent>(OnNavigationButtonClick);
        }
    }
}