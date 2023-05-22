using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFaction
{
    Player,
    Enemy
}


public class CharacterBase : MonoBehaviour
{
    [SerializeField] EFaction _Faction;

    public EFaction Faction => _Faction;
}