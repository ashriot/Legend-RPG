﻿using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : MonoBehaviour {
  public Image Image;
  public GameObject Cursor;
  public int Initiative;
  public Unit Unit;
  public bool IsHero { get { return Unit?.GetType() == typeof(Hero); } }
  public bool IsDead { get { return Unit?.CurHp > 0; } }
  // status effects

  public virtual void Setup() {}
  public void RollInitiative() {
    Initiative = Random.Range(0, Unit.Agility);
  }
}
