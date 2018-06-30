using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Transforms2D;
using UnityEngine;

namespace SineOfMadness {

    public class Boot : MonoBehaviour {

        public static EntityArchetype PlayerArchetype;
        public static EntityArchetype BasicEnemyArchetype;  // follow enemy type

        public static MeshInstanceRenderer PlayerLook;
        public static MeshInstanceRenderer BasicEnemyLook;

        public static GameplaySettings Settings;

        public void Awake() {
            SM.boot = this;
            Initialize();
            InitializePrototypes();

            World.Active.GetOrCreateManager<UpdateHud>().SetupGameObjects();
        }

        public void Initialize() {
            // This method creates archetypes for entities we will spawn frequently in this game.
            // Archetypes are optional but can speed up entity spawning substantially.

            EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();
            PlayerArchetype = entityManager.CreateArchetype(typeof(Player), typeof(Health), typeof(Position2D), typeof(Heading2D), 
                typeof(PlayerInput), typeof(TransformMatrix));

            BasicEnemyArchetype = entityManager.CreateArchetype(
                typeof(Enemy), typeof(Health),
                typeof(Position2D), typeof(Heading2D),
                typeof(TransformMatrix), typeof(MoveSpeed), typeof(MoveForward));

            GameObject settingsGO = GameObject.Find("GameplaySettings");
            Settings = settingsGO.GetComponent<GameplaySettings>();
        }

        public void InitializePrototypes() {
            PlayerLook = GetLookFromPrototype("Prototypes/Player");
            PlayerLook.mesh = CreatePlayerMesh();

            BasicEnemyLook = GetLookFromPrototype("Prototypes/BasicEnemy");
            BasicEnemyLook.mesh = CreatePlayerMesh();
        }

        private Mesh CreatePlayerMesh() {
            Mesh mesh = new Mesh();

            float hWidth = 2.5f, hHeight= 2.5f;
            Vector3[] vertices = new Vector3[4];
            
            vertices[0] = new Vector3(-hWidth, 0, -hHeight);
            vertices[1] = new Vector3(hWidth, 0, -hHeight);
            vertices[2] = new Vector3(-hWidth, 0, hHeight);
            vertices[3] = new Vector3(hWidth, 0, hHeight);

            mesh.vertices = vertices;

            int[] tri = new int[6];

            tri[0] = 0;
            tri[1] = 2;
            tri[2] = 1;

            tri[3] = 2;
            tri[4] = 3;
            tri[5] = 1;

            mesh.triangles = tri;

            Vector3[] normals = new Vector3[4];

            normals[0] = Vector3.up;
            normals[1] = Vector3.up;
            normals[2] = Vector3.up;
            normals[3] = Vector3.up;

            mesh.normals = normals;

            Vector2[] uv = new Vector2[4];

            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(1, 0);
            uv[2] = new Vector2(0, 1);
            uv[3] = new Vector2(1, 1);

            mesh.uv = uv;

            return mesh;
        }

        private static MeshInstanceRenderer GetLookFromPrototype(string protoName) {
            var proto = GameObject.Find(protoName);
            var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
            Object.Destroy(proto);
            return result;
        }

        public void NewGame() {
            EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();
            Entity player = entityManager.CreateEntity(PlayerArchetype);

            entityManager.SetComponentData(player, new Position2D { Value = new float2(0.0f, 0.0f) });
            entityManager.SetComponentData(player, new Heading2D { Value = new float2(0.0f, 1.0f) });

            entityManager.AddSharedComponentData(player, PlayerLook);


            // TODO test add some enemies
            Entity enemy = entityManager.CreateEntity(BasicEnemyArchetype);

            entityManager.SetComponentData(enemy, new Position2D { Value = new float2(2.0f, 0.0f) });
            entityManager.SetComponentData(enemy, new Heading2D { Value = new float2(0.0f, 1.0f) });

            entityManager.AddSharedComponentData(enemy, BasicEnemyLook);

            EnemySpawnSystem.SetupComponentData(entityManager);
        }
    }
}