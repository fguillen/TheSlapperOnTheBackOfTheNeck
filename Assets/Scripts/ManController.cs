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
        float scale = Random.Range(1.2f, 1.6f);
        float scaleDuration = Random.Range(0.1f, 0.3f);
        float rotation = Random.Range(-20f, 20f);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(Head.DOScale(new Vector3(scale, scale, scale), 0.1f));
        sequence.Join(Head.DORotate(new Vector3(0, 0, rotation), 0.1f));
        sequence.AppendInterval(scaleDuration);
        sequence.Append(Head.DOScale(new Vector3(1, 1, 1), 0.1f));
        sequence.Join(Head.DORotate(new Vector3(0, 0, 0), 0.1f));
        sequence.Join(Head.DOShakePosition(2.0f, strength: new Vector3(0, strength, 0), vibrato: vibrato, randomness: randomness, snapping: snapping, fadeOut: fadeOut));
        sequence.Append(Head.DOMove(headOriginalPosition, 0.2f));
    }

}
