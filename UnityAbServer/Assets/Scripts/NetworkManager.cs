﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{

    public static NetworkManager instance;

    public GameObject playerPrefab;
    public GameObject bulletPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        Server.Start(10, 32850);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstantiatePlayer()
    {
        return Instantiate(playerPrefab, new Vector3(0f, 0.5f, 0f), Quaternion.identity).GetComponent<Player>();
    }

    public Bullet InstantiateBullet(Transform _shootOrigin)
    {
        return Instantiate(bulletPrefab, _shootOrigin.position, _shootOrigin.rotation).GetComponent<Bullet>();
    }
}
