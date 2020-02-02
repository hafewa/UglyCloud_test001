using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetScript : MonoBehaviour
{
    public Vector3 offset;
    [SerializeField] private Vector3 position;
    private MeshRenderer mr;

    public Vector3 Position
    {
        get => position;
        set => position = value;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();//mr=mesh renderer

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = position + offset;
    }

    public void SetColor(Vector3 color)
    {
        mr.sharedMaterial.SetColor(name="_BaseColor",new Color(r: color.x,g: color.y,b: color.z));//name裡面是可以改成想變化的變數
    }
}
