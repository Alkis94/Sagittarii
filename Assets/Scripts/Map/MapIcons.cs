using UnityEngine;

public class MapIcons
{
    public GameObject HorizontalRoad { get; private set; }
    public GameObject VerticalRoad { get; private set; }
    public GameObject NormalRoom { get; private set; }
    public GameObject BossRoom { get; private set; }
    public MapIcons (string location)
    {
        HorizontalRoad = Resources.Load($"Map/{location}/{location}HorizontalRoad") as GameObject;
        VerticalRoad = Resources.Load($"Map/{location}/{location}VerticalRoad") as GameObject;
        NormalRoom = Resources.Load($"Map/{location}/{location}NormalRoom") as GameObject;
        BossRoom = Resources.Load($"Map/{location}/{location}BossRoom") as GameObject;
    }
}

