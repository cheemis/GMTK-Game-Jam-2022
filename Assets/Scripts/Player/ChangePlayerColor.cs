using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerColor : MonoBehaviour
{
    public List<Color> Presets = new List<Color>();
    public Material BodyDye;
    public Material DotsDye;

    public void ChangeBodyDye(int i)
    {
        BodyDye.color = Presets[i];
    }

    public void ChangeDotsDye(int i)
    {
        DotsDye.color = Presets[i];
    }

}
