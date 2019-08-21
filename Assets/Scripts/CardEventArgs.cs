using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardEventArgs : EventArgs
{
    public int CardIndex { get; private set; }

    public CardEventArgs(int cardIndex)
    {
        CardIndex = cardIndex;
    }
}
