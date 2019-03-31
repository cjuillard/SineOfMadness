using SineOfMadness;
using TMPro;
using Unity.Entities;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestUI : ComponentSystem
    {
        public TextMeshProUGUI text;
        
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Player player, ref Health health) =>
            {
                if (text == null)
                {
                    GameObject go = GameObject.Find("Canvas/HealthText");
                    text = go.GetComponent<TextMeshProUGUI>();
                }
                if (text != null)
                {
                    text.text = $"Health = {health.Value}";
                }    
            });
            
        }
    }
}