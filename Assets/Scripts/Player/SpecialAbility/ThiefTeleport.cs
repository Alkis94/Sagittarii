using UnityEngine;
using UnityEngine.SceneManagement;

public class ThiefTeleport : SpecialAbility
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        Cooldown = 3f;
    }

    protected override void Ability()
    {
        var result = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.8f, 1 << LayerMask.NameToLayer("Ground"));
        if (result == null)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
