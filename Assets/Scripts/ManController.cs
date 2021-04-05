using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ManController : MonoBehaviour
{
    [SerializeField] public Transform Head;

    [Header ("Head Shake")]
    [SerializeField] float strength;
    [SerializeField] int vibrato;
    [SerializeField] int randomness;
    [SerializeField] bool snapping;
    [SerializeField] bool fadeOut;

    Vector3 headOriginalPosition;

    void Awake()
    {
        headOriginalPosition = Head.position;
    }

    void Update()
    {
        if(Input.GetKeyDown("k"))
        {
            BounceHead();
        }
    }

    public void BounceHead()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(Head.DOShakePosition(2.0f, strength: new Vector3(0, strength, 0), vibrato: vibrato, randomness: randomness, snapping: snapping, fadeOut: fadeOut));
        sequence.Append(Head.DOMove(headOriginalPosition, 0.2f));
    }

}
