using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemy : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] private float waitSecond = 5;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(waitSecond);
        Destroy(enemy.gameObject);
    }
}
