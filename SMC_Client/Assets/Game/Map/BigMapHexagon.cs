using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BigMapHexagon : MonoBehaviour
{
    [SerializeField] private TextMeshPro testTxt;

    private int m_Cx = 0;
    private int m_Cy = 0;
    
    public void SetCoordinate(int x, int y)
    {
        m_Cx = x;
        m_Cy = y;

        testTxt.text = $"({m_Cx}, {m_Cy})";
        name = $"HEX:({m_Cx}, {m_Cy})";
    }
}
