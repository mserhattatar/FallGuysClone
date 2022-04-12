using UnityEngine;

public class Paint3DManager : MonoBehaviour
{
    // The camera that looks at the model, and the camera that looks at the canvas.
    [SerializeField] private Camera sceneCamera, canvasCam;

    [SerializeField] private GameObject brushContainer, objectToPainted;
    private bool startPaining;

    private void OnEnable() => GameManager.FinisLineAction += FinishLinePaint;

    private void OnDisable() => GameManager.FinisLineAction -= FinishLinePaint;

    private void LateUpdate()
    {
        if (startPaining)
            PaintWall();
    }

    private void FinishLinePaint()
    {
        objectToPainted.SetActive(true);
        startPaining = true;
    }


    // The main action, instantiates a brush or decal entity at the clicked position on the UV map
    private void PaintWall()
    {
        var uvWorldPosition = Vector3.zero;
        if (HitTestUVPosition(ref uvWorldPosition))
        {
            var brushObj = (GameObject) Instantiate(
                Resources.Load("BrushEntity"), brushContainer.transform, true);
            // The position of the brush (in the UVMap)
            brushObj.transform.localPosition = uvWorldPosition;
        }

        var brushes = brushContainer.GetComponentsInChildren<Transform>();

        if (brushes.Length <= 100)
            return;
        foreach (var brush in brushContainer.GetComponentsInChildren<Transform>())
        {
            if (brush.gameObject.GetInstanceID() != brushContainer.GetInstanceID())
            {
                Destroy(brush.gameObject);
            }
        }
    }


    // Returns the position on the texuremap according to a hit in the mesh collider
    private bool HitTestUVPosition(ref Vector3 uvWorldPosition)
    {
        Ray cursorRay = sceneCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(cursorRay, out var hit, 200f))
            return false;

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
}