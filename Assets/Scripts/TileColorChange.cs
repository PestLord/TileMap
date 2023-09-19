using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColorChange : MonoBehaviour
{
    [SerializeField] private Color _possibleColor;

    [SerializeField] private Color _prohibitedColor;

    private Renderer[] _childs;
    // Start is called before the first frame update
    void Start()
    {
        _childs = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnChangeColor(bool possible)
    {
        if (possible)
        {
            foreach (var child in _childs)
            {
                child.material.color = _possibleColor;
            }
        }
        else
        {
            foreach (var child in _childs)
            {
                child.material.color = _prohibitedColor;
            }          
        }
            
    }
}
