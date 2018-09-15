﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NightCity.Structs
{
    using Creators;
    using Structs;
    using Utilities;
    using Random = UnityEngine.Random;

    [Serializable]
    public struct SimpleCar
    {
#pragma warning disable 0414
        public Vector2 pos;
        public Vector2 dir;
#pragma warning restore 0414
    }

    [Serializable]
    public struct Car
    {
#pragma warning disable 0414
        [SerializeField]
        private Vector2 pos;
#pragma warning restore 0414
        [SerializeField]
        private Vector2 dir;

        [SerializeField]
        private int roadID;
        [SerializeField]
        private float progress;
        [SerializeField]
        private bool isForward;
        [SerializeField]
        private bool isIntersection;
        [SerializeField]
        private float magnitude;
        [SerializeField]
        private int nextID;
        [SerializeField]
        private Vector2 from;
        [SerializeField]
        private Vector2 to;


        public Car(Road road)
        {
            this.roadID = road.Id;
            this.progress = Random.Range(0f, road.OffsetMagnitude);
            this.isForward = Random.value < 0.5f ? true : false;

            this.dir = road.Direction * (this.isForward == true ? 1f : -1f);
            this.magnitude = road.OffsetMagnitude;

            var from = (this.isForward == true ? road.OffsetFrom : road.OffsetTo);
            this.from = from;
            this.to = (this.isForward == true ? road.OffsetTo : road.OffsetFrom);
            this.pos = from + this.dir * this.progress;

            this.isIntersection = false;
            this.nextID = 0;
        }

        public void Update(CityArea city, float speed, float offset, float straightRate)
        {
            var road = city.Roads[this.roadID];

            if(this.GetProgress() >= 1f)
            {
                this.SetNextRoad(road, city, offset, straightRate);
                road = city.Roads[this.roadID];
            }
            else
            {
                this.progress = Mathf.Min(this.magnitude, this.progress + speed);
            }
            
            this.pos = Vector2.Lerp(this.from, this.to, this.GetProgress());
        }

        public float GetProgress()
        {
            return this.progress / this.magnitude;
        }

        private void SetNextRoad(Road road, CityArea city, float offset, float straightRate)
        {
            if(this.isIntersection == true)
            {
                var next = city.Roads[this.nextID];

                this.dir = next.Direction * (this.isForward == true ? 1f : -1f);
                this.magnitude = next.OffsetMagnitude;

                this.from = this.pos;
                this.to = (this.isForward == true ? next.OffsetTo : next.OffsetFrom)
                    + (this.dir.Normal() * offset);

                this.roadID = this.nextID;
                this.isIntersection = false;
            }
            else
            {
                this.nextID = RoadPointer.GetNextRoadID(
                    road, this.isForward == true ? road.ToPointID : road.FromPointID, city, straightRate);

                var next = city.Roads[this.nextID];
                this.isForward = next.IsForward(this.to);

                var dir = next.Direction * (isForward == true ? 1f : -1f);

                this.from = this.to;
                this.to = (isForward == true ? next.OffsetFrom : next.OffsetTo)
                     + dir.Normal() * offset;

                dir = this.to - this.from;
                this.dir = dir.normalized;
                this.magnitude = dir.magnitude;

                this.isIntersection = true;
            }

            this.progress = 0f;
        }
        
        public static implicit operator SimpleCar(Car car)
        {
            return new SimpleCar()
            {
                pos = car.pos,
                dir = car.dir
            };
        }
    }
}
