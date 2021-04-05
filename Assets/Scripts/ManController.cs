using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ManController : MonoBehaviour
{
    [SerializeField] Transform head;
    [SerializeField] float strength;
    [SerializeField] int vibrato;
    [SerializeField] int randomness;
    [SerializeField] bool snapping;
    [SerializeField] bool fadeOut;


    void Update()
    {
        if(Input.GetKeyDown("k"))
        {
            BounceHead();
        }
    }

    void BounceHead()
    {
        head.DOShakePosition(2.0f, strength: new Vector3(0, strength, 0), vibrato: vibrato, randomness: randomness, snapping: snapping, fadeOut: fadeOut);
    }
}
