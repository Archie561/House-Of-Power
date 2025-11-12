using UnityEngine;

[CreateAssetMenu(fileName = "GameScreenDatabase", menuName = "Scriptable Objects/GameScreenDatabase")]
public class GameScreenConfigDatabase : ScriptableObject
{
    public GameScreen DefaultGameScreen;
    public GameScreenConfig[] ScreenConfigs;
}
