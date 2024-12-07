using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.PixelArtTopDown_Basic;
public class PropsAltarLightSignal : MonoBehaviour
{
    public GameObject altarRunes;
    public void LightAltar(){
        altarRunes.SetActive(true);
    }
    public void UndoLightAltar(){
        altarRunes.SetActive(false);
    }
}
