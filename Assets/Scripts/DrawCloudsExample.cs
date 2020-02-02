using UnityEngine;

public class DrawCloudsExample : MonoBehaviour
{
    public AnimationCurve curve;
    public int horizontalStackSize = 20;
    public float cloudHeight = 1f;
    public Mesh quadMesh;
    public Material cloudMaterial;
    float offset;
    public float animationspeedFactor = 100;
    [Range(0,10)] public float animationSpeed = 1;
    [Range(0,10)] public float animationSpeedSlower = 1;
    

    public int layer;
    public Camera camera;
    private Matrix4x4 matrix;
    private Matrix4x4[] matrices;
    public bool castShadows = false;
    public bool useGpuInstancing = false;
    public float CloudStartHeight = 40f;

    void Update()
    {
        //this couldn't use cuz it was for quad not for sphere
       // cloudMaterial.SetFloat("_midYValue", transform.position.y);
        
        cloudMaterial.SetFloat("_cloudHeight", cloudHeight);
        cloudMaterial.SetFloat("_animationSpeed", animationSpeed / animationspeedFactor);
        cloudMaterial.SetFloat("_animationSpeedSlower", animationSpeedSlower / animationspeedFactor);
        
        
        offset = cloudHeight / horizontalStackSize / 2f;
        Vector3 startPosition = transform.position + (Vector3.up * (offset * horizontalStackSize / 2f));

        if (useGpuInstancing) // initialize matrix array
        {
            matrices = new Matrix4x4[horizontalStackSize];
        }


        
        for (int i = 0; i < horizontalStackSize; i++)
        {
         //make a gradient to be public and use  the position to catch gradient for alpha to make different sphere have different alpha
            float normalizedRadius = (float) i / horizontalStackSize;
            float alphaValue = Mathf.Clamp01(curve.Evaluate(normalizedRadius));
            
         //   matrix = Matrix4x4.TRS(startPosition - (Vector3.up * offset * i), transform.rotation, transform.localScale);
         matrix = Matrix4x4.TRS(startPosition, transform.rotation, new Vector3(CloudStartHeight, CloudStartHeight, CloudStartHeight) +  new Vector3(i, i, i)*cloudHeight );
            if (useGpuInstancing)  
            {
                matrices[i] = matrix; // build the matrices array if using GPU instancing
            }
            else
            {   //assign different alpha to each sphere 
                Material mat = new Material(cloudMaterial);//define as new material
                mat.SetFloat("_globalAlpha", alphaValue);//make a new tag to use in shader graphic
                
                Graphics.DrawMesh(quadMesh, matrix, mat, layer, camera, 0, null, castShadows, false, false); // otherwise just draw it now
            }
        }
      
        
        
        if (useGpuInstancing) // draw the built matrix array
        {
            UnityEngine.Rendering.ShadowCastingMode shadowCasting = UnityEngine.Rendering.ShadowCastingMode.Off;
            if (castShadows)
                shadowCasting = UnityEngine.Rendering.ShadowCastingMode.On;

            Graphics.DrawMeshInstanced(quadMesh, 0, cloudMaterial, matrices, horizontalStackSize, null, shadowCasting, false, layer, camera);

        }
    }

}
