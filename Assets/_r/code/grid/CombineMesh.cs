using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineMesh : MonoBehaviour
{
    public MeshFilter[] meshFilters;
    public MeshFilter   combinedMesh;

    public       int[] testIndices;
    private float testYChange = 1f;
    
    private MeshRenderer[] meshRenderers;

    public void CombineMeshes()
    {
             CombineInstance[] combine = new CombineInstance[meshFilters.Length];
             
             int i = 0;
             while (i < meshFilters.Length)
             {
                 combine[i].mesh                = meshFilters[i].sharedMesh;
                 combine[i].transform           = meshFilters[i].transform.localToWorldMatrix;
                 meshRenderers[i]               = meshFilters[i].gameObject.GetComponent<MeshRenderer>();
                 meshRenderers[i].enabled       = false;
                 meshFilters[i].gameObject.name = $"{i}";
                 i++;
             }
     
             Mesh mesh = new Mesh();
             mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
             mesh.CombineMeshes(combine);
             combinedMesh.mesh = mesh;
    }

    public void EnableMeshRenderers()
    {
        int i = 0;
        while (i < meshRenderers.Length)
        {
            meshRenderers[i].enabled = true;
            i++;
        }
    }

    public void AlterMeshesByIndex(int[] meshIndices, float yChange)
    {
        foreach (int i in meshIndices)
        {
            meshRenderers[i].transform.position = new Vector3(meshRenderers[i].transform.position.x,
                meshRenderers[i].transform.position.y + yChange, meshRenderers[i].transform.position.z);
        }
    }

    private void OnEnable()
    {
        meshRenderers = new MeshRenderer[meshFilters.Length];
        CombineMeshes();
        SubscribeToEvents(true);
        // StartCoroutine(TestActiveCombine());
    }

    private IEnumerator TestActiveCombine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            Debug.Log("live mesh combine... GO!");
            EnableMeshRenderers();
            AlterMeshesByIndex(testIndices, testYChange);
            CombineMeshes();
        }
    }

    private void OnDisable()
    {
        SubscribeToEvents(false);
    }

    private void SubscribeToEvents(bool state)
    {
        if(state)
        {
            

        }
        else
        {
            
        }
    }
    //void Start() {}
    //void Update() {}
}