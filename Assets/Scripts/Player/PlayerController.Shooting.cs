using UnityEngine;
using Screen = UnityEngine.Device.Screen;
using Vector2 = System.Numerics.Vector2;

namespace Player
{
    public partial class PlayerController
    {
        private void HandleShooting()
        {
            Vector3 screenCenterPoint = new Vector3(Screen.width / 2f, Screen.height / 2f);
            
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
            }
        }
    }
}