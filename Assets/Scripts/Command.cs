using UnityEngine;

[CreateAssetMenu(fileName = "Command", menuName = "Legend-RPG/Command")]
public class Command : ScriptableObject
{
  public string Name, Description;
  public Sprite Sprite;
  public int MaxAmt, CurrentAmt;
  public Action Action;
}
