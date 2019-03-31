using DefaultNamespace;
using SineOfMadness;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Boot : MonoBehaviour
{
    [SerializeField] private PlayerInputBehaviour playerInputBehaviour;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemy1Prefab;
    [SerializeField] private GameSettings settings;
    
    private Entity playerEntityPrefab;
    private Entity enemy1EntityPrefab;

    void Start()
    {
        playerEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, World.Active);
        enemy1EntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(enemy1Prefab, World.Active);
        InitPlayer();
        InitEnemySpawns();
    }

    private void InitEnemySpawns()
    {
        EntityManager entityManager = World.Active.EntityManager;
        Entity entity = entityManager.CreateEntity(typeof(SpawnSource));
        
        entityManager.SetComponentData(entity, new SpawnSource
        {
            spawnType = enemy1EntityPrefab,
            currDelay = 0,
            delayPerSpawn = 1,
            spawnArea = settings.PlayArea
        });
    }
    
    private void InitPlayer()
    {
        EntityManager entityManager = World.Active.EntityManager;
        Entity playerEntity = entityManager.Instantiate(playerEntityPrefab);
        entityManager.AddComponents(playerEntity, new ComponentTypes(typeof(PlayerComponent), typeof(Health), typeof(PlayerInput)));
        entityManager.SetComponentData(playerEntity, new Player {MaxSpeed = settings.PlayerMaxSpeed});
        entityManager.SetComponentData(playerEntity, new Health {Value = 100});
        playerInputBehaviour.SetPlayer(playerEntity);
    }
}