using Photon.Pun;
using UnityEngine;

public abstract class MoveEntity : MonoBehaviourPun
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected float slowRatio;

    //protected Animator animator;
    //private float m_Distance;
    //private float m_Angle;
    //private Vector3 m_Direction;
    //private Vector3 m_NetworkPosition;
    //private Vector3 m_StoredPosition;
    //private Quaternion m_NetworkRotation;
    //public bool m_SynchronizePosition = true;
    //public bool m_SynchronizeRotation = true;
    //public bool m_SynchronizeScale = false;

    //[Tooltip("Indicates if localPosition and localRotation should be used. Scale ignores this setting, and always uses localScale to avoid issues with lossyScale.")]
    //public bool m_UseLocal;
    //bool m_firstTake = false;

    //public virtual void Awake()
    //{
    //    animator = GetComponent<Animator>();
    //    m_StoredPosition = transform.localPosition;
    //    m_NetworkPosition = Vector3.zero;
    //    m_NetworkRotation = Quaternion.identity;
    //}

    //protected virtual void Reset()
    //{
    //    // Only default to true with new instances. useLocal will remain false for old projects that are updating PUN.
    //    m_UseLocal = true;
    //}

    //protected virtual void OnEnable()
    //{
    //    m_firstTake = true;
    //}

    //public virtual void Update()
    //{
    //    var tr = transform;

    //    if (!this.photonView.IsMine)
    //    {
    //        if (m_UseLocal)

    //        {
    //            tr.localPosition = Vector3.MoveTowards(tr.localPosition, this.m_NetworkPosition, this.m_Distance * (1.0f / PhotonNetwork.SerializationRate));
    //            tr.localRotation = Quaternion.RotateTowards(tr.localRotation, this.m_NetworkRotation, this.m_Angle * (1.0f / PhotonNetwork.SerializationRate));
    //        }
    //        else
    //        {
    //            tr.position = Vector3.MoveTowards(tr.position, this.m_NetworkPosition, this.m_Distance * (1.0f / PhotonNetwork.SerializationRate));
    //            tr.rotation = Quaternion.RotateTowards(tr.rotation, this.m_NetworkRotation, this.m_Angle * (1.0f / PhotonNetwork.SerializationRate));
    //        }
    //    }
    //}

    //public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    var tr = transform;

    //    // Write
    //    if (stream.IsWriting)
    //    {
    //        if (this.m_SynchronizePosition)
    //        {
    //            if (m_UseLocal)
    //            {
    //                this.m_Direction = tr.localPosition - this.m_StoredPosition;
    //                this.m_StoredPosition = tr.localPosition;
    //                stream.SendNext(tr.localPosition);
    //                stream.SendNext(this.m_Direction);
    //            }
    //            else
    //            {
    //                this.m_Direction = tr.position - this.m_StoredPosition;
    //                this.m_StoredPosition = tr.position;
    //                stream.SendNext(tr.position);
    //                stream.SendNext(this.m_Direction);
    //            }
    //        }

    //        if (this.m_SynchronizeRotation)
    //        {
    //            if (m_UseLocal)
    //            {
    //                stream.SendNext(tr.localRotation);
    //            }
    //            else
    //            {
    //                stream.SendNext(tr.rotation);
    //            }
    //        }

    //        if (this.m_SynchronizeScale)
    //        {
    //            stream.SendNext(tr.localScale);
    //        }
    //    }
    //    // Read
    //    else
    //    {
    //        if (this.m_SynchronizePosition)
    //        {
    //            this.m_NetworkPosition = (Vector3)stream.ReceiveNext();
    //            this.m_Direction = (Vector3)stream.ReceiveNext();

    //            if (m_firstTake)
    //            {
    //                if (m_UseLocal)
    //                    tr.localPosition = this.m_NetworkPosition;
    //                else
    //                    tr.position = this.m_NetworkPosition;

    //                this.m_Distance = 0f;
    //            }
    //            else
    //            {
    //                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
    //                this.m_NetworkPosition += this.m_Direction * lag;
    //                if (m_UseLocal)
    //                {
    //                    this.m_Distance = Vector3.Distance(tr.localPosition, this.m_NetworkPosition);
    //                }
    //                else
    //                {
    //                    this.m_Distance = Vector3.Distance(tr.position, this.m_NetworkPosition);
    //                }
    //            }

    //        }
                    

    //        if (m_firstTake)
    //        {
    //            m_firstTake = false;
    //        }
    //    }
    //}

    //감소비율 0.2ㄹ
    public virtual void Slow(float _slowRatio , float durationTime)
    {
        print("슬로우" + _slowRatio + "시간: " + durationTime);
        slowRatio = _slowRatio;
        Invoke("ResetSlow", durationTime);
    }

    void ResetSlow()
    {
        slowRatio = 0;
    }

    public virtual void Stop(float time)
    {
        print("임시스탑.!!");
    }
}
