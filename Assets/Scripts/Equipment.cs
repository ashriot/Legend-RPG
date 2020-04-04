using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Legend-RPG/Equipment")]
public class Equipment : ScriptableObject {
  public EquipmentTypes EquipmentType;
}

public enum EquipmentTypes {
  Head,
  Body,
  Legs,
  Feet,
  Ring,
  Count
}