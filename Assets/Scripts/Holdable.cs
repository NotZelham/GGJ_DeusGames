using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : MonoBehaviour
{
    protected PlayerController owner;

    public void SetOwner(PlayerController _owner)
    {
        owner = _owner;
    }
}
