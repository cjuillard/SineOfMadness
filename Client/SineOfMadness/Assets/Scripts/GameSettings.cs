using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "SineOfMadness/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public float PlayerMaxSpeed = 0.1f;
    public Rect PlayArea = new Rect(0,0,10,10);
}