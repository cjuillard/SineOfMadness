using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "SineOfMadness/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public Rect PlayArea = new Rect(0,0,10,10);
    public bool MakePlayerInvincible = false;
    public float EnemyRadius = 0.5f;
    public float PlayerRadius = 0.5f;
    public float BulletSpeed = 10;
    public float FlockmateRadius = 10;
    public float BoidCohesionAcceleration = .1f;
    public float BoidAlignmentAcceleration = .1f;
    public float BoidGoalAcceleration = .2f;
}