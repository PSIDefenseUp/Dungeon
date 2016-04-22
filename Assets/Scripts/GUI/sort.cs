﻿using UnityEngine;
using System.Collections;

public class AlphaNumericSort : UnityEditor.BaseHierarchySort
{
  public override int Compare(GameObject lhs, GameObject rhs)
  {
    if (lhs == rhs) return 0;
    if (lhs == null) return -1;
    if (rhs == null) return 1;

    return UnityEditor.EditorUtility.NaturalCompare(lhs.name, rhs.name);
  }
}