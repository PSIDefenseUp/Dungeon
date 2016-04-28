using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class UnitSpawner : NetworkBehaviour
{

  public GameObject UnitPrefab;
  public int xPosition;
  public int yPosition;
  public int zPosition;
  public int unitRotation;

  public override void OnStartServer()
  {
    base.OnStartServer();
    Vector3 spawnPosition = new Vector3(xPosition, xPosition, zPosition);
    Quaternion SpawnRotation = Quaternion.Euler(0, unitRotation, 0);

    GameObject Unit = (GameObject)Instantiate(UnitPrefab, spawnPosition, SpawnRotation);
    NetworkServer.Spawn(Unit);

  }

}
