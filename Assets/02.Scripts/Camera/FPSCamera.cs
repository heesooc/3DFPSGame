using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

// 1인칭 슈팅 (First Person Shooter)
// 게임상의 캐릭터의 시점을 보는 카메라
public class FPSCamera : MonoBehaviour
{
    public Transform Target;

    private void LateUpdate()
    {
        if (GameManager.Instance.State != GameState.Go)
        {
            return;
        }

        transform.localPosition = Target.position;

        Vector2 xy = CameraManager.Instance.XY;
        transform.eulerAngles = new Vector3(-xy.y, xy.x, 0);

    }
}