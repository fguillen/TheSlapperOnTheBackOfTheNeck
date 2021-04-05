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
        // Head.DOShakeScale(duration: 0.5f, strength: new Vector3(1, 1, 0), vibrato: 2, randomness: 0, fadeOut: true);
        Sequence sequenceScale = DOTween.Sequence();
        sequenceScale.Append(Head.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.1f));
        sequenceScale.AppendInterval(0.3f);

        sequenceScale.Append(Head.DOScale(new Vector3(1, 1, 1), 0.1f));



        // Sequence sequenceBouncing = DOTween.Sequence();
        sequenceScale.Join(Head.DOShakePosition(2.0f, strength: new Vector3(0, strength, 0), vibrato: vibrato, randomness: randomness, snapping: snapping, fadeOut: fadeOut));
        sequenceScale.Append(Head.DOMove(headOriginalPosition, 0.2f));
    }

}
