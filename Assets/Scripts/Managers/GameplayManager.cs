using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    #region Singleton
    public GameplayManager Instance { get; private set; } = null;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
        Initiate();
    }
    #endregion

    private void Initiate()
    {

    }

}
