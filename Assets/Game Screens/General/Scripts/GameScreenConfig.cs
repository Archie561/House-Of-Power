using UnityEngine;

[CreateAssetMenu(fileName = "GameScreenConfig", menuName = "Scriptable Objects/GameScreenConfig")]
public class GameScreenConfig : ScriptableObject
{
    [Header("Єдиний ключ (з enums)")]
    public GameScreen GameScreenType;

    [Header("Ім'я для пошуку в UXML")]
    public string ButtonName;
    public string ScreenName;
    public Sprite ButtonIcon;

    // Сюди в майбутньому можна додати іконку, 
    // текст-підказку, звук відкриття тощо.
}
