using System.Collections;
using UnityEngine;

public struct PublicEvents {
    public delegate void LoadLevelDelegate();

    public event LoadLevelDelegate LoadNextLevel;
}