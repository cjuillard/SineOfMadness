using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace SineOfMadness {
    public class UpdateHud : ComponentSystem {

        public struct SpawnData {
            public int Length;
            public ComponentDataArray<EnemySpawnSystemState> State;
        }

        [Inject] SpawnData spawnData;

        private Button newGameButton;
        private Text statsText;

        public void SetupGameObjects() {
            newGameButton = GameObject.Find("NewGameButton").GetComponent<Button>();
            newGameButton.onClick.AddListener(OnNewGamePressed);
            statsText = GameObject.Find("StatsText").GetComponent<Text>();
        }

        protected override void OnUpdate() {
            if (spawnData.Length > 0) {
                statsText.text = $"Spawned Enemies: {spawnData.State[0].SpawnedEnemyCount}";
            }
        }

        public void OnNewGamePressed() {
            SM.boot.NewGame();
            newGameButton.gameObject.SetActive(false);
        }
    }
}