using UnityEngine;
using System.Collections;


namespace SineOfMadness {
    public class GameplaySettings : MonoBehaviour {

        public float PlayerMoveSpeed = 5;
        public float enemyInitialHealth = 10;
        public float enemySpeed = 2;
        public float spawnDelay = 0.1f;

        public Rect playfield = new Rect { x = -30.0f, y = -30.0f, width = 60.0f, height = 60.0f };
    }
}