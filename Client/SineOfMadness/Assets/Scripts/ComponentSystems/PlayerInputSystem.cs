using SineOfMadness;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerInputSystem : ComponentSystem
    {
        struct PlayerData {
            public readonly int Length;

            public ComponentDataArray<PlayerInput> Input;
        }

        [Inject] private PlayerData m_Players;
        
        protected override void OnUpdate()
        {
            float dt = Time.deltaTime;

            for (int i = 0; i < m_Players.Length; ++i) {
                UpdatePlayerInput(i, dt);
            }
        }
        
        private void UpdatePlayerInput(int i, float dt) {
            // TODO get the input from the UI components - this component system needs a reference
            //  Option 1 - Have an external MonoBehaviour that knows about the UI grab the system and initialize it
            PlayerInput pi;

//            pi.Move.x = Input.GetAxis("Horizontal");
//            pi.Move.y = Input.GetAxis("Vertical");
//            pi.Shoot.x = Input.GetAxis("ShootX");
//            pi.Shoot.y = Input.GetAxis("ShootY");
//            
//            pi.FireCooldown = Mathf.Max(0.0f, m_Players.Input[i].FireCooldown - dt);
//
//            m_Players.Input[i] = pi;
        }
    }
}