﻿using System;
using UnityEngine;


[RequireComponent(typeof(MazeConstructor))]

public class GameController : MonoBehaviour
{
    private MazeConstructor generator;

    void Start()
    {
        generator = GetComponent<MazeConstructor>();
        generator.GenerateNewMaze(15, 17);

        Cursor.lockState = CursorLockMode.Locked;
    }
}
