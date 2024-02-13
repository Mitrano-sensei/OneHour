using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hoverable))]
public class EnemyScript : MonoBehaviour
{
    void Start()
    {
        transform.localScale = Vector3.zero;
        transform?.DOScale(Vector3.one, .2f).SetEase(Ease.InOutSine);

        var hoverable = GetComponent<Hoverable>();
        hoverable.OnHoverEnter.AddListener(e =>
        {
            GameManager.Instance.Loose();

            var sequence = DOTween.Sequence();
            sequence.Append(transform?.DOScale(Vector3.one * .01f, .2f).SetEase(Ease.InOutSine));
            sequence.OnComplete(() => gameObject.SetActive(false));
        });
    }

}
