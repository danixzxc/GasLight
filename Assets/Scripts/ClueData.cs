using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Clue", menuName = "ClueData")]
public class ClueData : ScriptableObject
{
    public int scoreValue;
    public new string name;
    public string description;
    public Sprite sprite;
}
