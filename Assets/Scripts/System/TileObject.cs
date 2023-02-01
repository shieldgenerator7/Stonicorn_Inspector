using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileObject
{
    /// <summary>
    /// True: revealing this will damage the player
    /// </summary>
    public virtual bool Dangerous => false;

    /// <summary>
    /// True: will be detected by the scanner
    /// </summary>
    public virtual bool Detectable => Dangerous;
}
