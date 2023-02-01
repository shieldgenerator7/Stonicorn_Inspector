using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SpriteSet", menuName ="SpriteSet")]
public class SpriteSet : ScriptableObject
{
    [SerializeField]
    private List<Sprite> sprites;

    public Sprite this[int index] => sprites[index];
}
