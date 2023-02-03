using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LovePlant : MonoBehaviour
{
    Animator anim;
    [SerializeField] Transform player;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("Dist", int.MaxValue);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Dist", Vector3.Distance(transform.position, player.position));
    }
}
