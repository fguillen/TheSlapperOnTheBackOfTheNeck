using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WomanController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform hand;
    [SerializeField] Transform handShownTransform;
    [SerializeField] float waitTimeUntilShowHand = 1.0f;
    [SerializeField] TrailRenderer handTrail;


    [SerializeField] Transform handUpTransform;
    [SerializeField] Transform handDown1Transform;
    [SerializeField] Transform handDown2Transform;

    float timeStopped;

    Vector3 handHiddenPosition;

    Rigidbody2D rb;

    Tweener handTweener;

    enum HandState
    {
        hidden,
        showing,
        shown,
        shooting
    }

    HandState handState;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        handHiddenPosition = hand.localPosition;
        handTrail.enabled = false;
    }

    void Start()
    {
    }

    void Update()
    {
        Move();
        // FlipTowardsVelocity();
        UpdateTimeStopped();
        ShouldShowHand();
    }

    void UpdateTimeStopped()
    {
        if(rb.velocity.x == 0)
            timeStopped += Time.deltaTime;
        else
            timeStopped = 0;
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    // void FlipTowardsVelocity()
    // {
    //     if(
    //         (rb.velocity.x < 0 && transform.localScale.x > 0) ||
    //         (rb.velocity.x > 0 && transform.localScale.x < 0)
    //     )
    //         transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    // }

    void ShowHand()
    {
        handState = HandState.showing;
        handTweener.Kill();
        handTweener = hand.DOLocalMove(handShownTransform.localPosition, 1.0f).OnComplete(SetHandAsShown);
    }

    void SetHandAsShown()
    {
        handState = HandState.shown;
    }

    void HideHand()
    {
        handState = HandState.hidden;
        handTweener.Kill();
        handTweener = hand.DOLocalMove(handHiddenPosition, 0.3f);
    }

    void ShootHand()
    {
        handState = HandState.shooting;
        handTrail.enabled = true;
        // handTweener.Kill();

        // handTweener = hand.DOLocalMove(handUpTransform.position, 0.5f);
        Sequence handShootSequence = DOTween.Sequence();
        handShootSequence.Append(hand.DOLocalMove(handUpTransform.localPosition, 0.2f));
        handShootSequence.Append(hand.DOLocalPath(new Vector3[] { handUpTransform.localPosition, handDown1Transform.localPosition, handDown2Transform.localPosition }, 0.1f, PathType.CatmullRom, PathMode.Ignore, 10).SetEase(Ease.Linear));
        handShootSequence.OnComplete(HandImpact);
    }

    void HandImpact()
    {
        timeStopped = 0;
        HideHand();
        handTrail.enabled = false;
    }

    void ShouldShowHand()
    {
        if(rb.velocity.x == 0 && handState == HandState.hidden && timeStopped > waitTimeUntilShowHand)
            ShowHand();

        if(rb.velocity.x != 0 && (handState == HandState.showing || handState == HandState.shown))
            HideHand();

        if(Input.GetKeyDown("space"))
            ShootHand();
    }
}
