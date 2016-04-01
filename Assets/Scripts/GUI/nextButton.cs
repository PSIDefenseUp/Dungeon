using UnityEngine;
using System.Collections;

public class nextButton : MonoBehaviour {

  public Game managerRef;

  // Use this for initialization
  void Start()
  {
    managerRef = GameObject.Find("GameManager").GetComponent<Game>();
  }
  void Update()
  {
  }
}
