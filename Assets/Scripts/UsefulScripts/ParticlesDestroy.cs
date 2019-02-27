using UnityEngine;
using System.Collections;

public class ParticlesDestroy : MonoBehaviour {

    private ParticleSystem particleSystem1;
      
    void Start()
    {
        particleSystem1 = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (particleSystem1)
        {
            if (!particleSystem1.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
