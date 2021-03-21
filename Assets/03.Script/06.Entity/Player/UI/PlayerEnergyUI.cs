using UnityEditor;

using UnityEngine;
using UnityEngine.UI;
public class PlayerEnergyUI : MonoBehaviour
{
    [SerializeField] Slider[] slider_energy;
    [SerializeField] GameObject[] fillImage;
    [SerializeField] int maxCount;
    

    public void SetupMaxCount(int _maxCount)
    {
        maxCount = _maxCount;
        for(int i = 0; i < 3; i++)
        {
            if (i < maxCount)
            {
                slider_energy[i].gameObject.SetActive(true);
                //fillImage[i].gameObject.SetActive(true);
            }
            else
            {
                slider_energy[i].gameObject.SetActive(false);
                fillImage[i].gameObject.SetActive(false);

            }
        }
        
    }


    public void UpdateEnergy(float value)
    {
        //print(value);
        for(int i =0; i < maxCount; i++)
        {
            if(i + 1 <=  value)
            {
                fillImage[i].SetActive(true);
                continue;
            }
            fillImage[i].SetActive(false);
            float reFillValue = value - i;
            if(reFillValue >= 0 )
            {
                slider_energy[i].value = reFillValue;
            }
            else
            {
                slider_energy[i].value = 0;
            }


        }


    }
}
