using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WomanController : MonoBehaviour
{

    [SerializeField] float speed;

    [SerializeField] Transform hand;

    [SerializeField] float waitTimeUntilShowHand = 1.0f;
    [SerializeField] TrailRenderer handTrail;

    [SerializeField] List<ManController> men;

    [SerializeField] ParticleSystem explosion;


    [Header ("Hand Positions")]
    [SerializeField] Transform handShownTransform;
    [SerializeField] Transform handUpTransform;
    [SerializeField] Transform handDown1Transform;
    [SerializeField] Transform handDown2Transform;
    [SerializeField] Transform handDown2WrapperTransform;


    RandomSoundPlayer randomSoundPlayer;
    ManController targetMan;
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
        randomSoundPlayer = GetComponent<RandomSoundPlayer>();
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
        HandControllers();
        SetTargetMan();
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

    void SetTargetMan()
    {

        targetMan = men.Where(e => e.Head.position.x > transform.position.x).OrderBy(e => Vector3.Distance(transform.position, e.Head.position)).ToArray()[0];
        handDown2WrapperTransform.position = new Vector3(targetMan.Head.position.x, handDown2WrapperTransform.position.y, handDown2WrapperTransform.position.z);
    }

    void ShowHand()
    {
        print("ShowHand");
        handState = HandState.showing;

        handTweener.Kill();
        handTweener = hand.DOMove(handShownTransform.position, 1.0f).OnComplete(SetHandAsShown);
    }

    void SetHandAsShown()
    {
        handState = HandState.shown;
    }

    void HideHand()
    {
        print("HideHand");
        handState = HandState.hidden;
        handTweener.Kill();
        handTweener = hand.DOLocalMove(handHiddenPosition, 0.3f);
    }

    void ShootHand()
    {
        print("ShootHand");
        handState = HandState.shooting;

        handTrail.enabled = true;
        handTweener.Kill();

        // handTweener = hand.DOMove(handUpTransform.position, 0.5f);
        Sequence handShootSequence = DOTween.Sequence();
        handShootSequence.Append(hand.DOMove(handUpTransform.position, 0.2f));
        handShootSequence.Append(hand.DOPath(new Vector3[] { handUpTransform.position, handDown1Transform.position, handDown2Transform.position }, 0.1f, PathType.CatmullRom, PathMode.Ignore, 10).SetEase(Ease.Linear));
        handShootSequence.OnComplete(HandImpact);
    }

    void HandImpact()
    {
        print("HandImpact");
        targetMan.BounceHead();
        timeStopped = 0;
        SceneManagerController.Instance.ShakeCamera();
        ShowExplosion();
        PlaySnapSound();
        HideHand();
        handTrail.enabled = false;
    }

    void ShowExplosion()
    {
        ParticleSystem newExplostion = Instantiate(explosion, targetMan.Head.position, Quaternion.identity);
        Destroy(newExplostion.gameObject, 2f);
    }

    void PlaySnapSound()
    {
        randomSoundPlayer.Play();
    }

    void HandControllers()
    {
        if(rb.velocity.x == 0 && handState == HandState.hidden && timeStopped > waitTimeUntilShowHand)
            ShowHand();

        if(rb.velocity.x != 0 && (handState == HandState.showing || handState == HandState.shown))
            HideHand();

        if(handState != HandState.shooting && Input.GetKeyDown("space"))
            ShootHand();
    }
}
