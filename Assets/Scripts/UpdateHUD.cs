using System;
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

        public struct RoundStatsData {
            public int Length;
            public ComponentDataArray<RoundState> State;
        }
        [Inject] RoundStatsData roundStatsData;

        private Button newGameButton;
        private Text statsText;

        public void SetupGameObjects() {
            newGameButton = GameObject.Find("NewGameButton").GetComponent<Button>();
            newGameButton.onClick.AddListener(OnNewGamePressed);
            statsText = GameObject.Find("StatsText").GetComponent<Text>();
        }

        protected override void OnUpdate() {
            if (spawnData.Length > 0 && roundStatsData.Length > 0) {
                statsText.text = $"Spawned Enemies: {spawnData.State[0].SpawnedEnemyCount}" +
                    $"\nKill Count: {roundStatsData.State[0].numberOfKills}";
            }
        }

        public void OnGameEnd() {
            newGameButton.gameObject.SetActive(true);
        }

        public void OnNewGamePressed() {
            SM.boot.NewGame();
            newGameButton.gameObject.SetActive(false);
        }
    }
}