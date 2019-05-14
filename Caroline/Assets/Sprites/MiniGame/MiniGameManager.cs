using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public static int rocksDestroyed = 0;

    private void Awake()
    {
        rocksDestroyed = 0;
    }
}
