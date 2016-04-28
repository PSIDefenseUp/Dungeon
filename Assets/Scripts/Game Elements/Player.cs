using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[System.Serializable]
public struct player
{
  public string playerName;     // Player Name
  public int playerIndex;       // Which player number is this?
  public int team;              // The 'team' this player is on -- 0: heroes, 1: dungeon master
  public int keyCount;          // The number of keys this player possesses

  public player(int x, string name)
  {
    playerName = name;
    playerIndex = 0;
    team = x;
    keyCount = 0;
  }
}

[System.Serializable]
public class SyncListPlayer : SyncListStruct<player>
{

}