using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBars : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public Slider hpslider;
    [SerializeField]
    public Slider mpslider;

    void Start()
    {
        Slider[] sliders = GetComponentsInChildren<Slider>();
        mpslider = (Slider)sliders[0];
        hpslider = (Slider)sliders[1];
    }

    // Update is called once per frame
   public void SetHPValue(float value)
    {
        hpslider.value = value;
    }

   public void SetMPValue(float value)
    {
        mpslider.value = value;
    }

    public void SetHPMaxValue(float value)
    {
        hpslider.maxValue = value;
        hpslider.value = value;
    }

    public void SetMPMaxValue(float value)
    {
        mpslider.maxValue = value;
        mpslider.value = value;
    }
}
