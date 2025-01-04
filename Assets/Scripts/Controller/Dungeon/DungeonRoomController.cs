using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomController : MonoBehaviour
{
    [field:SerializeField] public List<CorridorAreaController> Doors { get; private set; }
}
