using UnityEngine;

public interface IInitializable 
{
    void Initialize(Transform InitializePosition, int horizontalDirection = 0, int verticalDirection = 0);
}

