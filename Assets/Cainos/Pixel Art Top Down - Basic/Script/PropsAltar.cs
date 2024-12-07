using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

//when something get into the alta, make the runes glow
namespace Cainos.PixelArtTopDown_Basic
{

    public class PropsAltar : MonoBehaviour
    {
        public List<SpriteRenderer> runes;
        public float lerpSpeed;

        private Color curColor;
        private Color targetColor;
        int objectCount = 0;

        private void Awake()
        {
            targetColor = runes[0].color;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            objectCount++;
            if(objectCount == 1)
                ActivateFunction();
            targetColor.a = 1.0f;
        }

        public virtual void ActivateFunction(){

        }

        public virtual void DeactivateFunction(){

        }

        private void OnTriggerExit2D(Collider2D other)
        {
            objectCount--;
            if(objectCount == 0)
                DeactivateFunction();
            targetColor.a = 0.0f;
        }

        private void Update()
        {
            curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);

            foreach (var r in runes)
            {
                r.color = curColor;
            }
        }
    }
}
