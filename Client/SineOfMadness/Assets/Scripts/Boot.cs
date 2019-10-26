using System;
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
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameSettings settings;

    private static Boot instance;
    public static Boot Instance => instance;
    public static GameSettings Settings => Instance.settings;
    
    private Entity playerEntityPrefab;
    private Entity enemy1EntityPrefab;
    private Entity playerBulletEntity;
    public Entity PlayerBulletEntity => playerBulletEntity;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, World.Active);
        enemy1EntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(enemy1Prefab, World.Active);
        playerBulletEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, World.Active);

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
        entityManager.AddComponents(playerEntity, new ComponentTypes(typeof(PlayerInput)));
        entityManager.SetComponentData(playerEntity, new Player {MaxSpeed = settings.PlayerMaxSpeed});
        if(settings.MakePlayerInvincible)
            entityManager.SetComponentData(playerEntity, new Health {Value = 25000});
        playerInputBehaviour.SetPlayer(playerEntity);
    }
}