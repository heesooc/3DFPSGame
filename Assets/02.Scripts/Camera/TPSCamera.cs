using UnityEngine;

// 3인칭 슈팅 (Third Person Shooter)
// 게임상의 캐릭터가 보는 시점이 아닌, 캐릭터를 보는 시점 즉, 3인칭 관찰자 시점의 카메라
public class TPSCamera : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset = new Vector3(0, 3f, -3f);


    public float a;
    private void LateUpdate()
    {
        if (GameManager.Instance.State != GameState.Go)
        {
            return;
        }

        transform.localPosition = Target.position + Offset;
        transform.LookAt(Target);

        Vector2 xy = CameraManager.Instance.XY;
        transform.RotateAround(Target.position, Vector3.up, xy.x);
        transform.RotateAround(Target.position, transform.right, -xy.y);


        transform.localPosition = Target.position - transform.forward * Offset.magnitude + Vector3.up * (Offset.y - a);
    }
}