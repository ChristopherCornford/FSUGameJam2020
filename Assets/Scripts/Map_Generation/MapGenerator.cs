using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MapGenerator : MonoBehaviour
{
    public Texture2D MapImage { get; private set; }

    [Header("Input Settings")]
    [SerializeField] private Texture2D mapImage = null;
    [Space]
    private RawImage mapUIImage = null;
    

    [Header("Output Settings")]
    [SerializeField] bool SaveMapAsAsset = true;
    [SerializeField] string SaveLocationPath;
    [Space]

    [Header("Image Dimensions")]
    [SerializeField] private int mapHeight;
    [SerializeField] private int mapWidth;
    [Space]

    [Header("Color - Prefab Assignments")]
    [SerializeField] Color32 color1 = new Color32(0, 0, 0, 0);
    [SerializeField] GameObject object1 = null;
    [Space]
    [SerializeField] Color32 color2 = new Color32(0, 0, 0, 0);
    [SerializeField] GameObject object2 = null;
    [Space]
    [SerializeField] Color32 color3 = new Color32(0, 0, 0, 0);
    [SerializeField] GameObject object3 = null;
    [SerializeField] Material testMaterial = null;

    List<GameObject> myChildren = new List<GameObject>();

    private void Start() => StartCoroutine(CreateNewMap());


    private void OnValidate()
    {
        if (!mapUIImage) { mapUIImage = GameObject.FindObjectOfType(typeof(RawImage)) as RawImage; }

        if (mapImage == MapImage)
        {
            return;
        }
        else
        {
            if(mapImage != null)
            {
                MapImage = mapImage;

                mapHeight = mapImage.height;
                mapWidth = mapImage.width;

                mapUIImage.texture = mapImage;
            }
        }
    }

    IEnumerator CreateNewMap()
    {
        Time.timeScale = 0.5f;

        string newFolderPath = string.Empty;

        if(SaveMapAsAsset)
        {
            string newFolder = AssetDatabase.CreateFolder("Assets", MapImage.name);

            newFolderPath = AssetDatabase.GUIDToAssetPath(newFolder);
        }

        int xVal = mapWidth;
        int yVal = mapHeight;

        for (int x = 0; x < xVal; x++)
        {
            GameObject newChild = new GameObject("Child " + x.ToString(), typeof(MeshFilter), typeof(MeshRenderer));
            newChild.transform.parent = transform;

            myChildren.Add(newChild);

            GameObject[] newBlocks = new GameObject[yVal];

            for (int y = 0; y < yVal; y++)
            {
                Color32 pixelColor = MapImage.GetPixel(x, y);
               
                if (Color.Equals(pixelColor, color1))
                {
                    GameObject newBlock = Instantiate(object1, new Vector3(x, 0, y), Quaternion.identity);
                    newBlock.transform.parent = newChild.transform;
                    newBlocks[y] = newBlock;
                }
                else if (Color.Equals(pixelColor, color2))
                {
                    GameObject newBlock = Instantiate(object2, new Vector3(x, 0, y), Quaternion.identity);
                    newBlock.transform.parent = newChild.transform;
                    newBlocks[y] = newBlock;
                }
                else if (Color.Equals(pixelColor, color3))
                {
                    GameObject newBlock = Instantiate(object3, new Vector3(x, 0, y), Quaternion.identity);
                    newBlock.transform.parent = newChild.transform;
                    newBlocks[y] = newBlock;
                }
            }

            if(newBlocks[0] != null)
                newChild.GetComponent<MeshFilter>().sharedMesh = MergeObjectsAndMaterials(newBlocks, newChild);

            foreach(GameObject o in newBlocks)
            {
                o.SetActive(false);
            }

            if(SaveMapAsAsset)
            {
                AssetDatabase.CreateAsset(newChild.GetComponent<MeshFilter>().sharedMesh, newFolderPath + "/" + x.ToString()+".asset");
            }

            yield return new WaitForFixedUpdate();
        }

        if (SaveMapAsAsset)
        {
            this.gameObject.name = mapImage.name + "_Map";

            PrefabUtility.SaveAsPrefabAsset(this.gameObject, SaveLocationPath + this.gameObject.name + ".prefab");
        }

        Time.timeScale = 1;
    }
    
    public Mesh MergeObjectsAndMaterials(GameObject[] objs, GameObject parentObj)
    {
        List<MeshFilter> filters = new List<MeshFilter>();
        List<MeshRenderer> renderers = new List<MeshRenderer>();
        
        foreach (GameObject o in objs)
        {
            if(o.GetComponent<MeshFilter>())
                filters.Add(o.GetComponent<MeshFilter>());
            if (o.GetComponent<MeshRenderer>())
                renderers.Add(o.GetComponent<MeshRenderer>());
        }

        List<Material> materials = new List<Material>();

        foreach(MeshRenderer mr in renderers)
        {
            Material[] localMaterials = mr.sharedMaterials;
            foreach(Material m in localMaterials)
            {
                if (!materials.Contains(m))
                {
                    materials.Add(m);
                }
            }
        }

        parentObj.GetComponent<MeshRenderer>().sharedMaterials = materials.ToArray();

        List<Mesh> submeshes = new List<Mesh>();

        foreach(Material m in materials)
        {
            List<CombineInstance> combiners = new List<CombineInstance>();

            foreach(MeshFilter mf in filters)
            {
                MeshRenderer meshRenderer = mf.GetComponent<MeshRenderer>();
                if(!meshRenderer)
                {
                    Debug.LogError(mf.name + " has no MeshRenderer!");
                    continue;
                }

                Material[] localMaterials = meshRenderer.sharedMaterials;

                for (int index = 0; index < localMaterials.Length; index++)
                {
                    if (localMaterials[index] != m)
                        continue;

                    CombineInstance ci = new CombineInstance();

                    ci.mesh = mf.sharedMesh;
                    ci.subMeshIndex = index;
                    ci.transform = mf.transform.localToWorldMatrix;
                    combiners.Add(ci);
                }
            }

            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combiners.ToArray(), true);
            submeshes.Add(mesh);
        }

        List<CombineInstance> finalCombine = new List<CombineInstance>();

        foreach (Mesh mesh in submeshes)
        {
            CombineInstance ci = new CombineInstance();

            ci.mesh = mesh;
            ci.subMeshIndex = 0;
            ci.transform = Matrix4x4.identity;
            finalCombine.Add(ci);
        }

        Mesh finalMesh = new Mesh();

        finalMesh.CombineMeshes(finalCombine.ToArray(), false);
        
        return finalMesh;
    }
}
