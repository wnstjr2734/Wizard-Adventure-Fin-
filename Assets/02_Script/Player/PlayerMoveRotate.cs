using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾� �̵� �� ȸ�� ���� ������ ó��
/// </summary>
public class PlayerMoveRotate : MonoBehaviour
{
    [Header("Move Setting")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField, Tooltip("�߷� ������")]
    private float gravity = 9.81f;
    private float yVelocity = 0;
    private Vector2 rotation;

    [Header("Teleport")] 
    [SerializeField] private float teleportRange = 7.0f;
    [SerializeField] private float controlPointHeightMultiplier = 0.25f;
    [SerializeField] private Transform teleportDirectionTransform;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform teleportTarget;
    [SerializeField] private Transform footPos;
    [SerializeField] private float dashTime = 0.25f;

    private bool isTeleporting = false;

    private int mapLayerMask;
    private int enemyLayerMask;

    [Header("Hand Control")] 
    [SerializeField, Tooltip("�޼� ��Ʈ�ѷ�")]
    private HandController leftHandController;

    private RaycastHit hit;

    private CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();

        mapLayerMask = 1 << LayerMask.NameToLayer("Default");
        enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
    }

    private void Update()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
        }

        yVelocity -= gravity * Time.deltaTime;
        if (cc.enabled)
        {
            cc.Move(new Vector3(0, yVelocity, 0));
        }
    }

    public void StartTeleport()
    {
        leftHandController.SetLeftHandAction(HandController.LeftAction.Teleport);
        line.gameObject.SetActive(true);
        teleportTarget.gameObject.SetActive(true);
        print("Get Down Teleport");
    }

    public void OnTeleport()
    {
        if (Physics.Raycast(teleportDirectionTransform.position, teleportDirectionTransform.forward, out hit,
                teleportRange, mapLayerMask | enemyLayerMask))
        {
            Vector3 teleportPos;
            int hitLayerMask = 1 << hit.collider.gameObject.layer;
            // ���Ϳ� �浹�ϴ� ��� Ȥ�� ���� �ڷ���Ʈ�ϴ� ��� �浹�� �տ� �̵��ϵ��� ����
            if ((hitLayerMask & enemyLayerMask) > 0 || 
                Vector3.Dot(hit.normal, Vector3.up) < 0.5f)
            {
                RaycastHit hit2;
                Vector3 revisedPos = hit.point + hit.normal;
                bool canRevise = Physics.Raycast(revisedPos, Vector3.down, out hit2, Single.PositiveInfinity, mapLayerMask);
                Debug.Assert(canRevise, "Error : can't revised Teleport Pos");
                // Z-fighting�� �Ͼ�� �ʰ� �ڷ���Ʈ ��ġ ����
                teleportPos = hit2.point + Vector3.up * 0.05f;
            }
            // �׷��� �ʴٸ� �̵� ������ ���� ����
            else
            {
                // Z-fighting�� �Ͼ�� �ʰ� �ڷ���Ʈ ��ġ ����
                teleportPos = hit.point + Vector3.up * 0.05f;
            }
            teleportTarget.position = teleportPos;
            DrawTeleportLineCurve(teleportDirectionTransform.position, teleportTarget.position);
        }
    }

    public void EndTeleport()
    {
        print("Get Up Teleport");
        leftHandController.SetLeftHandAction(HandController.LeftAction.Default);
        line.gameObject.SetActive(false);
        teleportTarget.gameObject.SetActive(false);

        if (!isTeleporting)
        {
            StartCoroutine(nameof(IEDash));
        }
    }

    private IEnumerator IEDash()
    {
        // �ڷ���Ʈ ������ �ٽ� �ڷ���Ʈ�� �� ����
        isTeleporting = true;
        cc.enabled = false;
        Vector3 origin = transform.position;
        Vector3 targetPos = teleportTarget.position - footPos.localPosition; // �� ��ġ ����

        // �뽬 �ϴ� ���� Vignette ȿ�� ����

        float multiplier = 1 / dashTime;
        for (float t = 0.0f; t < dashTime; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(origin, targetPos, t * multiplier);
            yield return null;
        }
        transform.position = targetPos;

        cc.enabled = true;
        isTeleporting = false;
    }

    private void DrawTeleportLineCurve(Vector3 startPos, Vector3 endPos)
    {
        Vector3 mid = (startPos + endPos) * 0.5f;
        Vector3 controlPointPos = mid + Vector3.up * Vector3.Distance(startPos, endPos) * controlPointHeightMultiplier;

        line.SetPosition(0, startPos);
        for (int i = 1; i < line.positionCount - 1; i++)
        {
            float t = (float)i / line.positionCount;
            var m = Vector3.Lerp(startPos, controlPointPos, t);
            var n = Vector3.Lerp(controlPointPos, endPos, t);
            var b = Vector3.Lerp(m, n, t);

            line.SetPosition(i, b);
        }
        line.SetPosition(line.positionCount - 1, endPos);
    }

    public void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01) return;

        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        // For simplicity's sake, we just keep movement in a single plane here.
        // Rotate direction according to world Y rotation of player.
        var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        if (cc.enabled)
        {
            cc.Move(move * scaledMoveSpeed);
        }
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
