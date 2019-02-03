using UnityEngine;
using System.Collections;

public class ParticlesDestroy : MonoBehaviour {

    private ParticleSystem ps;
      
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
