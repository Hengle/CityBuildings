﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NightCity
{
    using Components;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(WindowTexture))]
    [RequireComponent(typeof(Skyscraper))]
    [AddComponentMenu("Night City/Main Controller")]
    public class MainController : MonoBehaviour
    {
        private WindowTexture windowTexture = null;
        private Skyscraper skyScraper = null;


        private void Awake()
        {
            this.windowTexture = GetComponent<WindowTexture>();
            this.windowTexture.Init();

            this.skyScraper = GetComponent<Skyscraper>();
            this.skyScraper.Init(this.windowTexture);
        }
    }
}

// 道幅は4mで2車線
// ビルの高さはMinimumで30m（10階建て） (w*hは15*15くらいから）
