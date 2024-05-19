using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Plaform/Palette")]
public class PlatfromPaletteSO : ScriptableObject
{
    public Sprite LeftSprite;
    public Sprite MiddleSprite;
    public Sprite RightSprite;
    public Sprite SoloSprite;


    public Sprite GetDisplaySprite(int currentIndex, int platformLength)
    {
        if (platformLength == 1)
        {
            return SoloSprite;
        }
        if (currentIndex == platformLength - 1)
        {
            return RightSprite;
        }
        if (currentIndex == 0)
        {
            return LeftSprite;
        }

        return MiddleSprite;
    }
}
