﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartLevel : MonoBehaviour
{

    public GameObject orchestrator;
    private LevelSelect levelSelect;

    public int level;
    public int difficulty;
    public void Start()
    {
        levelSelect = orchestrator.GetComponent<LevelSelect>();
    }

    public void StartGivenLevel()
    {
        levelSelect.ExternalSetLevel(level, difficulty);
    }
}
