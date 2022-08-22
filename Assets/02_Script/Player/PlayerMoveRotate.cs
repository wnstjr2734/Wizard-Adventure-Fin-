using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 이동 및 회전 관련 로직을 처리
/// </summary>
public class PlayerMoveRotate : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    [Header("Teleport")] 
    [SerializeField] private float teleportRange = 7.0f;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform teleportTarget;
    [SerializeField] private Transform footPos;

    private Vector2 rotation;

    private RaycastHit hit;

    public void StartTeleport()
    {
        line.gameObject.SetActive(true);
        teleportTarget.gameObject.SetActive(true);
        print("Get Down Teleport");
    }

    public void OnTeleport()
    {
        if (Physics.Raycast(leftHandTransform.position, leftHandTransform.forward, out hit,
                teleportRange, 1 << LayerMask.NameToLayer("Default")))
        {
            // 벽에 텔레포트 하지 않도록 보정
            if (Vector3.Dot(hit.normal, Vector3.up) > 0.5f)
            {
                // Z-fighting이 일어나지 않게 텔레포트 위치 보정
                teleportTarget.position = hit.point + Vector3.up * 0.05f;
                DrawTeleportLineCurve(footPos.localPosition, teleportTarget.localPosition);
            }
        }
    }

    public void EndTeleport()
    {
        print("Get Up Teleport");
        line.gameObject.SetActive(false);
        teleportTarget.gameObject.SetActive(false);

        transform.position = teleportTarget.position - footPos.localPosition;
    }

    private void DrawTeleportLineCurve(Vector3 startPos, Vector3 endPos)
    {
        Vector3 mid = (endPos - startPos) * 0.5f;
        Vector3 controlPointPos = mid + Vector3.up * mid.magnitude;

        for (int i = 0; i < line.positionCount; i++)
        {
            float t = (float)i / line.positionCount;
            var m = Vector3.Lerp(startPos, controlPointPos, t);
            var n = Vector3.Lerp(controlPointPos, endPos, t);
            var b = Vector3.Lerp(m, n, t);

            line.SetPosition(i, b);
        }
    }

    public void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;
        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        // For simplicity's sake, we just keep movement in a single plane here. Rotate
        // direction according to world Y rotation of player.
        var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        transform.position += move * scaledMoveSpeed;
    }

    public void Look(Vector2 rotate)
    {
        if (rotate.sqrMagnitude < 0.01)
            return;
        var scaledRotateSpeed = rotateSpeed * Time.deltaTime;
        rotation.y += rotate.x * scaledRotateSpeed;
        rotation.x = Mathf.Clamp(rotation.x - rotate.y * scaledRotateSpeed, -89, 89);
        transform.localEulerAngles = rotation;
    }
}
