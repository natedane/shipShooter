using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem explosion;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator removeEnemy(){
      yield return new WaitForSeconds(1f);
      Instantiate(explosion, transform.position, Quaternion.identity).Play();

      Destroy(gameObject);
    }
}
