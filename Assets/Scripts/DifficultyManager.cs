using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
        public float gameSpeed = 5f;
        public float speedIncrement = 0.1f;

        void Update()
        {
            IncreaseDifficulty();
        }

        void IncreaseDifficulty()
        {
            gameSpeed += Time.deltaTime * speedIncrement;
        }
 }


