using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SceneManagerController : MonoBehaviour
{
    public static SceneManagerController Instance;

    [Header ("Camera Shake")]
    [SerializeField] float duration;
    [SerializeField] float strength;
    [SerializeField] int vibrato;
    [SerializeField] int randomness;
    [SerializeField] bool fadeOut;

    Camera mainCamera;
    Vector3 cameraOriginalPosition;

    void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
        cameraOriginalPosition = mainCamera.transform.position;
    }

    public void ShakeCamera()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(mainCamera.DOShakePosition(duration, new Vector3(0, strength, 0), vibrato, randomness, fadeOut));
        sequence.Append(mainCamera.transform.DOMove(cameraOriginalPosition, 0.2f));
    }
}
