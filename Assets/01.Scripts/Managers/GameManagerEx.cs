using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    GameObject _player;
    //Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
    HashSet<GameObject> _monsters = new HashSet<GameObject>();

    public Action<int> OnSpawnEvent;

    public GameObject GetPlayer() { return _player; }

    public GameObject Spawn(Define.CretureType type, string path, Transform parent = null)
    {
        GameObject go = Manager.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Define.CretureType.Enemy:
                _monsters.Add(go);
                if (OnSpawnEvent != null)
                    OnSpawnEvent.Invoke(1);
                break;
            case Define.CretureType.Player:
                _player = go;
                break;
        }

        return go;
    }

    public Define.CretureType GetWorldObjectType(GameObject go)
    {
        BaseCtrl bc = go.GetComponent<BaseCtrl>();
        if (bc == null)
            return Define.CretureType.Unknown;

        return bc.cretureType;
    }

    public void Despawn(GameObject go)
    {
        Define.CretureType type = GetWorldObjectType(go);

        switch (type)
        {
            case Define.CretureType.Enemy:
                {
                    if (_monsters.Contains(go))
                    {
                        _monsters.Remove(go);
                        if (OnSpawnEvent != null)
                            OnSpawnEvent.Invoke(-1);
                    }
                }
                break;
            case Define.CretureType.Player:
                {
                    if (_player == go)
                        _player = null;
                }
                break;
        }

        Manager.Resource.Destroy(go);
    }
}
