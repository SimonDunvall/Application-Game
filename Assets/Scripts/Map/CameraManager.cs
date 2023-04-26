using Assets.Scripts.Buildings;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField]
        private Camera cam;

        private Vector3 dragOrigin;

        private void Update()
        {
            PanCamera();
            ClickOnBuilding();
        }

        private void PanCamera()
        {
            if (Input.GetMouseButtonDown(0))
                dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButton(0))
            {
                Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
                cam.transform.position += difference;
            }
        }

        private void ClickOnBuilding()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 rayOrigin = mousePos;
                Vector2 rayDirection = (rayOrigin - (Vector2)Camera.main.transform.position).normalized;

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection);
                if (hit)
                {
                    var building = hit.collider.GetComponent<IResourceBuilding>();
                    if (building != null)
                    {
                        UiManager.instance.OpenInspector(building.InnerStorage.ToString(), (int)building.TimeLeft, building.Level, building.ResourceType, building.GetInstanceID());
                    }
                }
            }
        }

    }
}