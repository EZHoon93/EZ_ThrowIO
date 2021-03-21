using System.Collections;

using UnityEngine;

public class CharacterObject : PoolableObject
{
    //[SerializeField] CharacterStatsData characterStatsData;

    public CharacterStatsData CharacterStatsData;
    
    public SkinnedMeshRenderer skinnedMesh;

    public Material orinealMaterial;
    public Material hitMaterial;
    public Material transMaterial;
    public Transform rightHand;

    public bool isHit { get; set; }

    
    private void Reset()
    {
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        rightHand = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightHand);
    }
    private void Awake()
    {
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void OnEnable()
    {
        isHit = false;
    }


    public void OnDamage()
    {
        skinnedMesh.material = hitMaterial;
        Invoke("ResetMateiral", 0.5f);
        isHit = true;
    }

    public void ResetMateiral()
    {
        skinnedMesh.material = orinealMaterial;
        isHit = false;
    }

    public void SetupStatsData(CharacterStatsData _characterStatsData)
    {
        CharacterStatsData = _characterStatsData;
    }

    public void Local_UpdateTransparent(bool isTransparent)
    {
        if (isTransparent)
        {
            skinnedMesh.material = transMaterial;
        }
        else
        {
            skinnedMesh.material = orinealMaterial;
        }
    }
   

}
