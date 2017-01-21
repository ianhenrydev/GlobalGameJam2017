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
        public MeshRenderer WorldWater;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SpawnWave(new Vector4(-10, 0, 5, 0));
            SpawnWave(new Vector4(-20, 0, 10, 0));
            SpawnWave(new Vector4(-15, 0, 15, 0));
            SpawnWave(new Vector4(-15, 0, 20, 0));

            StartCoroutine(TickWaves());
        }

        private IEnumerator TickWaves()
        {
            if (WorldWaves != null)
            {
                while (true)
                {
                    if (WorldWaves.Count <= 0)
                    {
                        yield return null;
                        continue;
                    }

                    for (int i = 0; i < WorldWaves.Count; i++)
                    {
                        //var angleDirection = Quaternion.AngleAxis(WorldWaves[i].z, Vector3.up).eulerAngles.normalized;
                        //WorldWaves[i] += new Vector4(angleDirection.x, angleDirection.y, 0, 0);

                        WorldWaves[i] += new Vector4(0.1f, 0, 0, 0);
                    }
                    
                    MaterialPropertyBlock newBlock = new MaterialPropertyBlock();
                    newBlock.SetVectorArray(Shader.PropertyToID("_WaveHotSpots"), WorldWaves.ToArray());

                    WorldWater.SetPropertyBlock(newBlock);

                    yield return null;
                }
            }

            yield return null;
        }

        public static void SpawnWave(Vector4 waveInfo)
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
            for (int i = 0; i <= WorldWaves.Count - 2; i++)
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