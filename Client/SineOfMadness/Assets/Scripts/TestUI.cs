using TMPro;
using Unity.Entities;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestUI : ComponentSystem
    {
        public TextMeshProUGUI text;
        
        public struct Data
        {
            public readonly int Length;
            public ComponentDataArray<Boot.Player> Player;
            public ComponentDataArray<Boot.Health> Health;
        }

        [Inject] private Data m_Data;
        
        protected override void OnUpdate()
        {
            if (text == null)
            {
                GameObject go = GameObject.Find("Canvas/HealthText");
                text = go.GetComponent<TextMeshProUGUI>();
            }
            if (text != null && m_Data.Length > 0)
            {
                text.text = $"Health = {m_Data.Health[0].Value}";
            }
        }
    }
}