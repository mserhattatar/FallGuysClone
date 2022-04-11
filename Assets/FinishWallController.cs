using UnityEngine;

public class FinishWallController : MonoBehaviour
{
    public Camera sceneCamera, canvasCam; //The camera that looks at the model, and the camera that looks at the canvas.
    public GameObject brushContainer;

    private void Update()
    {
        if (Input.GetMouseButton(0))
            DoAction();
    }

    //The main action, instantiates a brush or decal entity at the clicked position on the UV map
    void DoAction()
    {
        var uvWorldPosition = Vector3.zero;
        if (HitTestUVPosition(ref uvWorldPosition))
        {
            var brushObj = (GameObject) Instantiate(Resources.Load("BrushEntity"), brushContainer.transform,
                true); //Paint a brush
            brushObj.transform.localPosition = uvWorldPosition; //The position of the brush (in the UVMap)
        }
    }


    //Returns the position on the texuremap according to a hit in the mesh collider
    private bool HitTestUVPosition(ref Vector3 uvWorldPosition)
    {
        Ray cursorRay = sceneCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cursorRay, out var hit, 200f))
        {
            if (hit.collider == null && !hit.collider.CompareTag("FinishWall"))
                return false;

            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
                return false;

            Vector2 pixelUV = hit.textureCoord;

            uvWorldPosition.x = pixelUV.x - canvasCam.orthographicSize; //To center the UV on X
            uvWorldPosition.y = pixelUV.y - canvasCam.orthographicSize; //To center the UV on Y
            uvWorldPosition.z = 0.0f;

            return true;
        }

        return false;
    }
}


/*
void FixedUpdate()
{
    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out hit, 200f))
    {
        if (hit.collider != null && hit.collider.CompareTag("FinishWall"))
        {
            Renderer rend = hit.transform.GetComponent<Renderer>();

            MeshCollider meshCollider = hit.collider as MeshCollider;

            if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null ||
                meshCollider == null)
                return;

            Texture2D tex = rend.material.mainTexture as Texture2D;
            Vector2 pixelUV = hit.textureCoord;
            pixelUV.x *= tex.width;
            pixelUV.y *= tex.height;
            Debug.Log("height = " + tex.height + ", width = " + tex.width);

            Color color = Color.white;
            tex.SetPixel(0, 0, color);
            Debug.Log((int) pixelUV.x + "  after  " + (int) pixelUV.y);
            tex.Apply();


            /*Texture2D texture = new Texture2D(128, 128);
            GetComponent<Renderer>().material.mainTexture = texture;
            var i = 0;
            for (int y = 0; y < texture.height/10; y++)
            {
                for (int x = 0; x < texture.width/10; x++)
                {
                    i++;
                    Debug.Log(y + "   " + i);
                    Color color2 = Color.red;
                    texture.SetPixel(x, y, color2);
                }
            }

            texture.Apply();#1#
        }
    }
}*/