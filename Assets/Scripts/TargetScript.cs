using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Hoverable))]
public class TargetScript : MonoBehaviour
{
    [SerializeField] private bool _isActivated = false;
    [SerializeField] private UnityEvent _onActivation = new();

    public bool IsActivated { get => _isActivated; set => _isActivated = value; }
    public UnityEvent OnActivation { get => _onActivation; set => _onActivation = value; }

    void Start()
    {
        transform.localScale = Vector3.zero;
        transform?.DOScale(Vector3.one, .2f).SetEase(Ease.InOutSine);
        


        var hoverable = GetComponent<Hoverable>();

        _onActivation.AddListener(() => Debug.Log("Activated ! "));

        hoverable.OnHoverEnter.AddListener(e =>
        {
            _isActivated = true;
            _onActivation.Invoke();

            var sequence = DOTween.Sequence();
            sequence.Append(transform?.DOScale(Vector3.one * .01f, .2f).SetEase(Ease.InOutSine));
            sequence.OnComplete(() => gameObject.SetActive(false));
        });
    }

}
