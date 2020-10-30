using Mirror;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject[] playerPrefabs;

    private static List<Transform> spawnLocations = new List<Transform>();

    private int nextIndex = 0;

    public static void AddSpawnPoint(Transform transform)
    {
        spawnLocations.Add(transform);

        spawnLocations = spawnLocations.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(Transform transform) => spawnLocations.Remove(transform);

    public override void OnStartServer() => GameNetworkManager.OnServerReadied += SpawnPlayer;

    [ServerCallback]
    private void OnDestroy() => GameNetworkManager.OnServerReadied -= SpawnPlayer;

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        Transform spawnLoc = spawnLocations.ElementAtOrDefault(nextIndex);

        if (spawnLoc == null)
        {
            return;
        }

        GameObject playerInstance = Instantiate(playerPrefabs[0], spawnLoc.position, spawnLoc.rotation);

        NetworkServer.Spawn(playerInstance, conn);

        nextIndex++;
    }
}
