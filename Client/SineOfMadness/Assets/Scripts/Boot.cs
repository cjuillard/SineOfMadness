using DefaultNamespace;
using SineOfMadness;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Boot : MonoBehaviour
{
    [SerializeField] private PlayerInputBehaviour playerInputBehaviour;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameSettings settings;
    
    private Entity playerEntityPrefab;

    public struct Player : IComponentData
    {
        public float MaxSpeed;
    }

    public struct Health : IComponentData {
        public float Value;
    }

    void Start()
    {
        playerEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, World.Active);
        InitPlayer();
    }

    private void InitPlayer()
    {
        EntityManager entityManager = World.Active.EntityManager;
        Entity playerEntity = entityManager.Instantiate(playerEntityPrefab);
        entityManager.AddComponents(playerEntity, new ComponentTypes(typeof(Player), typeof(Health), typeof(PlayerInput)));
        entityManager.SetComponentData(playerEntity, new Player {MaxSpeed = settings.PlayerMaxSpeed});
        entityManager.SetComponentData(playerEntity, new Health {Value = 100});
        playerInputBehaviour.SetPlayer(playerEntity);
    }
}