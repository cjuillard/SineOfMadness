using System.Numerics;
using SineOfMadness;
using Unity.Entities;
using Unity.Mathematics;
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
            if (!Application.isMobilePlatform)
            {
                if (math.lengthsq(input.Move) == 0)
                {
                    if (Input.GetKey(KeyCode.D)) input.Move.x++;
                    if (Input.GetKey(KeyCode.A)) input.Move.x--;
                    if (Input.GetKey(KeyCode.S)) input.Move.y--;
                    if (Input.GetKey(KeyCode.W)) input.Move.y++;

                    if (math.lengthsq(input.Move) > 0) input.Move = math.normalize(input.Move);
                }

                if (math.lengthsq(input.Shoot) == 0)
                {
                    
                    if (Input.GetKey(KeyCode.RightArrow)) input.Shoot.x++;
                    if (Input.GetKey(KeyCode.LeftArrow)) input.Shoot.x--;
                    if (Input.GetKey(KeyCode.DownArrow)) input.Shoot.y--;
                    if (Input.GetKey(KeyCode.UpArrow)) input.Shoot.y++;

                    if (math.lengthsq(input.Shoot) > 0) input.Shoot = math.normalize(input.Shoot);
                }
            }

            World.Active.EntityManager.SetComponentData(player.Value, input);
        }

        public void SetPlayer(Entity player)
        {
            this.player = player;
        }
    }
}