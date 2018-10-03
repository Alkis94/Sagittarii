using UnityEngine;

public interface IInitializable 
{
    void Initialize(Transform InitializePosition, int horizontalDirection = 0, int verticalDirection = 0);
}

public interface IInitializableProjectile
{
    void Initialize(Transform InitializePosition, float horizontalDirection = 0, float verticalDirection = 0);
}