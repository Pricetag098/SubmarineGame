
using System.Collections.Generic;
using UnityEngine;

public class PipSlider : MonoBehaviour
{

    [SerializeField] ControlLever targetControl;
    [SerializeField] List<SpritPip> positivePips = new List<SpritPip>();
    [SerializeField] List<SpritPip> negativePips = new List<SpritPip>();
    [SerializeField] float increment;
    [SerializeField] bool invert;

    private void Start()
    {
        targetControl.onValueChanged += SetValue;
        BlankPips();

    }

    void SetValue(float value)
    {
        if (invert)
            value = -value;

        BlankPips();

        if(value > 0)
        {
            int pipCount = (int)(value/increment);
            for (int i = 0; i < pipCount; i++)
            {
                positivePips[i].SetState(true);
            }
        }

        else if(value < 0)
        {
            int pipCount = (int)(Mathf.Abs(value) / increment);
            for (int i = 0; i < pipCount; i++)
            {
                negativePips[i].SetState(true);
            }
        }
    }

    void BlankPips()
    {
        foreach (SpritPip pip in positivePips)
            pip.SetState(false);
        foreach (SpritPip pip in negativePips)
            pip.SetState(false);
    }

}
