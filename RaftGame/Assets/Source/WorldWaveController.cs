using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using JetBrains.Annotations;

namespace RaftGame.Wave
{
    public class WorldWaveController : MonoBehaviour
    {
        private static WorldWaveController Instance;
        private List<Vector4> WorldWaves = new List<Vector4>();
        public Material WorldWater;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {

            StartCoroutine(TickWaves());
        }

        private IEnumerator TickWaves()
        {
            if (WorldWaves != null)
            {
                while (WorldWaves.Count > 0)
                {
                    for (int i = 0; i < WorldWaves.Count; i++)
                    {
                        var angleDirection = Quaternion.AngleAxis(WorldWaves[i].z, Vector3.up).eulerAngles.normalized;
                        WorldWaves[i] += new Vector4(angleDirection.x, angleDirection.y, 0, 0);
                    }

                    //Sort waves by distance
                    BubbleSortWaves();

                    //WorldWater.SetVectorArray("WaveHotspots", WorldWaves.ToArray());

                    yield return null;
                }
            }

            yield return null;
        }

        public static void SpawnWave(Vector3 waveInfo)
        {
            if (Instance != null)
            {
                Instance.WorldWaves.Add(waveInfo);

                //Play spawn wave sound

                //Play spawn wave fx
            }
        }

        /// <summary>
        /// Trust me, I work.
        /// </summary>
        private void BubbleSortWaves()
        {
            int swaps = 0;
            for (int i = 0; i < WorldWaves.Count; i++)
            {
                var sum = WorldWaves[i].x + WorldWaves[i].y;
                var nextSum = WorldWaves[i + 1].x + WorldWaves[i + 1].y;

                if (nextSum > sum)
                {
                    swap(i, i++);
                    swaps++;
                }
            }

            if(swaps > 0)
                BubbleSortWaves();
        }

        private void swap(int idxA, int idxB)
        {
            var tmp = WorldWaves[idxB];
            WorldWaves[idxB] = WorldWaves[idxA];
            WorldWaves[idxA] = tmp;
        }
    }
}