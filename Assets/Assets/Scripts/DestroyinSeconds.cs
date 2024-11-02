using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyinSeconds : MonoBehaviour
{
    [SerializeField] private float seconds = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, seconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
