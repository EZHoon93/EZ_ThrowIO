using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : PoolableObject
{
    [SerializeField] TextMeshProUGUI text;

    
    public void Play(string _content , Vector3 targetPos)
    {
        this.transform.position = targetPos + new Vector3(0,2,0);
        this.transform.localScale = new Vector3(1, 1, 1);
        text.text = _content;
        text.DOFade(0f, 2.0f);
        transform.DOPunchScale(Vector3.one * 0.5f, 0.5f);
        transform.DOMove(transform.position + Vector3.up * 2, 1.0f).OnComplete(() => { Push(); });
    }

    public override void Push()
    {
        base.Push();
        text.DOFade(1f, 0.0f);
    }

}
