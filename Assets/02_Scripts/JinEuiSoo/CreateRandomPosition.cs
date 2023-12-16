using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace JES
{
    public static class JESFunctions
    {
        private static BoxCollider2D _wideCollider;
        private static BoxCollider2D _insideCollider;

        //[SerializeField] GameObject _testObject;
        //[SerializeField] GameObject[] _points;
        public static void SetCollider(BoxCollider2D wideCollider, BoxCollider2D insideCollider)
        {
            _wideCollider = wideCollider;
            _insideCollider = insideCollider;
        }

        public static Vector2 CreateRandomInstance()
        {
            Vector2 outSideMax = _wideCollider.bounds.max;
            Vector2 outSideMin = _wideCollider.bounds.min;
            Vector2 innerSideMax = _insideCollider.bounds.max;
            Vector2 innerSideMin = _insideCollider.bounds.min;

            // clock wise
            // min x, max x, min y, max y
            float4 sideA = new float4(innerSideMax.x, outSideMax.x, outSideMin.y, outSideMax.y);
            float4 sideB = new float4(innerSideMin.x, innerSideMax.x, outSideMin.y, innerSideMin.y);
            float4 sideC = new float4(outSideMin.x, innerSideMin.x, outSideMin.y, outSideMax.y);
            float4 sideD = new float4(innerSideMin.x, innerSideMax.x, innerSideMax.y, outSideMax.y);

            float4 positionRandom = sideA;
            int randomInt = Random.Range(0, 4);

            switch (randomInt)
            {
                case 0:
                    positionRandom = sideA;
                    break;
                case 1:
                    positionRandom = sideB;
                    break;
                case 2:
                    positionRandom = sideC;
                    break;
                case 3:
                    positionRandom = sideD;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            return new Vector2( Random.Range(positionRandom.x, positionRandom.y), Random.Range(positionRandom.z, positionRandom.w));                  
            }

        }

    }
