using SineOfMadness;
using Unity.Entities;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerInputBehaviour : MonoBehaviour
    {
        [SerializeField] private TouchJoystick leftJoystick;
        [SerializeField] private TouchJoystick rightJoystick;

        private Entity? player;

        private void Update()
        {
            if (player == null)
                return;
            
            PlayerInput input = new PlayerInput();
            input.Move = leftJoystick.NormalizedVelocity;
            input.Shoot = rightJoystick.NormalizedVelocity;
            
            World.Active.EntityManager.SetComponentData(player.Value, input);
        }

        public void SetPlayer(Entity player)
        {
            this.player = player;
        }
    }
}