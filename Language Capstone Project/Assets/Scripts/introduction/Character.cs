using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterObject character;

    Material mat;

    void Start()
    {
        mat = GetComponentInChildren<Renderer>().material;

        if(character.isMale)
        {
            mat.color = Color.blue;
        }
        else
        {
            mat.color = Color.red;
        }
    }
}
