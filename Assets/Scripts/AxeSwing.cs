using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AxeSwing : MonoBehaviour
{
    public BoxCollider2D axeCollider;
    public bool swing = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Swing(){
        if(swing) return;
        axeCollider.isTrigger = true;
        swing = true;
        transform.DOLocalRotate(new Vector3(0, 0, 45), 0.15f, RotateMode.LocalAxisAdd).OnComplete(() => {
            axeCollider.isTrigger = false;
            transform.DOLocalRotate(new Vector3(0, 0, -45), 0.15f, RotateMode.LocalAxisAdd).OnComplete(() => {
                swing = false;
            });
        });
    }
}
