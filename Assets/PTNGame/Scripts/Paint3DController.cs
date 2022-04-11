using System.Collections.Generic;
using UnityEngine;

public class Paint3DController : MonoBehaviour
{
    public Camera sceneCamera, canvasCam; //The camera that looks at the model, and the camera that looks at the canvas.
    public GameObject brushContainer;

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
            PaintWall();
    }

    //The main action, instantiates a brush or decal entity at the clicked position on the UV map
    private void PaintWall()
    {
        var uvWorldPosition = Vector3.zero;
        if (HitTestUVPosition(ref uvWorldPosition))
        {
            var brushObj = (GameObject) Instantiate(Resources.Load("BrushEntity"), brushContainer.transform,
                true); //Paint a brush
            brushObj.transform.localPosition = uvWorldPosition; //The position of the brush (in the UVMap)
        }

        var brushes = brushContainer.GetComponentsInChildren<Transform>();
        if (brushes.Length <= 100) return;
        foreach (var brush in brushContainer.GetComponentsInChildren<Transform>())
        {
            if (brush.gameObject.GetInstanceID() != brushContainer.GetInstanceID())
            {
                Destroy(brush.gameObject);
            }
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

            var orthographicSize = canvasCam.orthographicSize;
            uvWorldPosition.x = pixelUV.x - orthographicSize; //To center the UV on X
            uvWorldPosition.y = pixelUV.y - orthographicSize; //To center the UV on Y
            uvWorldPosition.z = 0.0f;

            return true;
        }

        return false;
    }
}