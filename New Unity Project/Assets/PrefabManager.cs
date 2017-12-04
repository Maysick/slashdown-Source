using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NamedPrefab {
    public string name;
    public GameObject go;
}

public class PrefabManager : MonoBehaviour {

    public static PrefabManager instance;

    [SerializeField]
    public NamedPrefab[] list;

    public GameObject GetPrefabByName(string _name) {
        foreach (NamedPrefab np in list) {
            if (np.name == _name) return np.go;
        }
        return null;
    }

    private void Awake() {
        instance = this;
    }
}
