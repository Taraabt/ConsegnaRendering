using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDuplicator : MonoBehaviour
{

    private List<GameObject> pooledObject = new List<GameObject>();
    private int amountToPool = 200;
    //Input input;

    public float activeTime = 2f;
    public Material mat;
    public float meshRefereshRate = 0.1f;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    public float MeshTime = 3;


    private void Awake()
    {
        //input = new Input();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = new GameObject();
            MeshRenderer mr = obj.AddComponent<MeshRenderer>();
            MeshFilter mf = obj.AddComponent<MeshFilter>();
            obj.SetActive(false);
            pooledObject.Add(obj);
        }
    }

    //private void OnEnable()
    //{
    //    input.Shader.AfterImage.performed+=AfterImage;
    //}

    //private void OnDisable()
    //{
    //    input.Shader.AfterImage.performed -= AfterImage;
    //}


    //private void AfterImage(InputAction.CallbackContext context)
    //{
    //    Debug.Log("sium");
    //    StartCoroutine(ActivateTrail(activeTime));
    //}


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ActivateTrail(activeTime));
        }
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0; i < pooledObject.Count; i++)
        {
            if (!pooledObject[i].activeInHierarchy)
            {
                return pooledObject[i];
            }
        }
        return null;
    }

    IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= meshRefereshRate;
            if (skinnedMeshRenderers == null)
            {
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            }
            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = GetPooledObject();
                gObj.transform.SetPositionAndRotation(transform.position, transform.rotation);

                MeshRenderer mr = gObj.GetComponent<MeshRenderer>();
                MeshFilter mf = gObj.GetComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);
                mf.mesh = mesh;
                mr.material = mat;

                gObj.SetActive(true);
                StartCoroutine(DeActivate(gObj));
            }
            yield return new WaitForSeconds(meshRefereshRate);
        }
    }

    IEnumerator DeActivate(GameObject obj)
    {
        yield return new WaitForSeconds(MeshTime);
        obj.SetActive(false);
    }

}
