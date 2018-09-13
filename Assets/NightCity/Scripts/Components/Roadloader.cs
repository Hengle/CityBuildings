﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using UnityEngine;

namespace NightCity.Components
{
    using Creators;
    using Structs;

    [DisallowMultipleComponent]
    [AddComponentMenu("Night City/Components/Roadloader")]
    public class Roadloader : MonoBehaviour
    {
        public const uint MaxVertexCount = 128u;
        public const uint PerVertex = 8u;
        public const uint MaxPointPerGeom = MaxVertexCount / PerVertex;

        public const string PropColor = "_Color";
        public const string PropSize = "_Size";
        public const string PropBasicWidth = "_basicWidth";
        public const string PropHeight = "_height";
        public const string PropMaxPointPerGeom = "_maxPointPerGeom";
        public const string PropGeomData = "_geomData";

        [SerializeField]
        private Color color = new Color(0.99f, 0.75f, 0.70f, 1f);
        [SerializeField]
        private float size = 1f;
        [SerializeField]
        private float basicWidth = 24f;
        [SerializeField]
        private float height = 0f;
        [SerializeField]
        private Material material = null;

        private ComputeBuffer geomBuffer = null;
        private int vertsCount = 1;


        public void Init(Skyscraper skyscraper)
        {
            var roads = skyscraper.CityArea.Roads.Values.Select(r => (SimpleRoad)r);

            this.geomBuffer = new ComputeBuffer(roads.Count(), Marshal.SizeOf(typeof(SimpleRoad)), ComputeBufferType.Default);
            this.geomBuffer.SetData(roads.ToArray());

            var step = skyscraper.CityArea.MaxDistance / skyscraper.CityArea.Interval;
            this.vertsCount = Mathf.CeilToInt(step / MaxPointPerGeom);
        }

        private void OnRenderObject()
        {
            if(this.geomBuffer == null)
            {
                return;
            }

            this.material.SetPass(0);

            this.material.SetColor(PropColor, this.color);
            this.material.SetFloat(PropSize, this.size);
            this.material.SetFloat(PropBasicWidth, this.basicWidth);
            this.material.SetFloat(PropHeight, this.height);
            this.material.SetBuffer(PropGeomData, this.geomBuffer);
            this.material.SetFloat(PropMaxPointPerGeom, MaxPointPerGeom);

            Graphics.DrawProcedural(MeshTopology.Points, this.vertsCount, this.geomBuffer.count);
        }

        private void OnDestroy()
        {
            this.geomBuffer?.Release();
        }
    }
}
