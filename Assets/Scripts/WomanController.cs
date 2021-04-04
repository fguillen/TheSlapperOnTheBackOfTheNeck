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

    float timeStopped;

    Vector3 handHiddenPosition;

    Rigidbody2D rb;

    Tweener handTweener;

    enum HandState
    {
        hidden,
        showing,
        shown
    }

    HandState handState;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        handHiddenPosition = hand.localPosition;
    }

    void Start()
    {
    }

    void Update()
    {
        Move();
        FlipTowardsVelocity();
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

    void FlipTowardsVelocity()
    {
        if(
            (rb.velocity.x < 0 && transform.localScale.x > 0) ||
            (rb.velocity.x > 0 && transform.localScale.x < 0)
        )
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void ShowHand()
    {
        handState = HandState.showing;
        handTweener.Kill();
        handTweener = hand.DOLocalMove(handShownTransform.localPosition, 1.0f);
    }

    void HideHand()
    {
        handState = HandState.hidden;
        handTweener.Kill();
        handTweener = hand.DOLocalMove(handHiddenPosition, 0.5f);
    }

    void ShouldShowHand()
    {
        if(rb.velocity.x == 0 && handState == HandState.hidden && timeStopped > waitTimeUntilShowHand)
            ShowHand();

        if(rb.velocity.x != 0 && handState == HandState.showing)
            HideHand();
    }
}
