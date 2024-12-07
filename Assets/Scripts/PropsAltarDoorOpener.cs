using System;
using System.Collections;
using System.Collections.Generic;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;
public class PropsAltarDoorOpener : PropsAltar
{
    public WoodenDoorScript door;
	public override void ActivateFunction()
	{
        bool unlocked = door.Unlock();
        if (unlocked && GetComponent<PropsAltarLightSignal>())
        {
            GetComponent<PropsAltarLightSignal>().LightAltar();
        }
	}
    public override void DeactivateFunction()
	{
        door.Lock();
        if(GetComponent<PropsAltarLightSignal>()){
            GetComponent<PropsAltarLightSignal>().UndoLightAltar();
        }
	}
}
