using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Allows creation of simple lightning bolts
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class LightningBolt : GripMagic
{
    [Tooltip("라이트닝 볼트 목표 위치. " +
             "If null, EndPosition is used.")]
    public GameObject EndObject;

    [Tooltip("The end position where the lightning will end at. This is in world space if EndObject is null, otherwise this is offset from EndObject position.")]
    public Vector3 EndPosition;

    [SerializeField, Tooltip("최대 사정거리")] 
    private float maxDistance = 20.0f;

    [Range(4, 10)]
    [Tooltip("How manu generations? Higher numbers create more line segments.")]
    public int Generations = 6;

    [SerializeField, Range(0.01f, 1.0f)]
    [Tooltip("Lightning Bolt 유지 시간\n" +
             "continuous mode에선 다음 번개(모양)을 그릴 때까지 기다리는 시간")]
    private float duration = 0.05f;
    private float timer;

    [SerializeField, Tooltip("timer 끝나도 계속 지속할지")]
    private bool continuousMode = false;

    [SerializeField, Range(0.0f, 1.0f)]
    [Tooltip("How chaotic should the lightning be? (0-1)")]
    private float chaosFactor = 0.15f;

    [Tooltip("In manual mode, the trigger method must be called to create a bolt")]
    public bool ManualMode;

    [Header("Damage")] 
    [SerializeField, Tooltip("번개 맞으면 줄 데미지 정보")]
    public ElementDamage elementDamage;

    [Header("Others")]
    [SerializeField, Tooltip("라이트닝 볼트 히트시킬 대상")]
    private LayerMask lightningLayerMask;

    /// <summary>
    /// Assign your own random if you want to have the same lightning appearance
    /// </summary>
    [HideInInspector]
    [System.NonSerialized]
    public System.Random RandomGenerator = new System.Random();

    private LineRenderer lineRenderer;
    private List<KeyValuePair<Vector3, Vector3>> segments = new List<KeyValuePair<Vector3, Vector3>>();
    private int startIndex;

    private Camera mainCamera;
    private bool orthographic;



    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        orthographic = (mainCamera != null && mainCamera.orthographic);
    }

    private void Update()
    {
        orthographic = (mainCamera != null && mainCamera.orthographic);
        if (timer <= 0.0f)
        {
            if (continuousMode)
            {
                timer = duration;
                Trigger();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        timer -= Time.deltaTime;
    }

    public override void StartMagic()
    {
        Trigger();
    }

    private void GetPerpendicularVector(ref Vector3 directionNormalized, out Vector3 side)
    {
        if (directionNormalized == Vector3.zero)
        {
            side = Vector3.right;
        }
        else
        {
            // use cross product to find any perpendicular vector around directionNormalized:
            // 0 = x * px + y * py + z * pz
            // => pz = -(x * px + y * py) / z
            // for computational stability use the component farthest from 0 to divide by
            float x = directionNormalized.x;
            float y = directionNormalized.y;
            float z = directionNormalized.z;
            float px, py, pz;
            float ax = Mathf.Abs(x), ay = Mathf.Abs(y), az = Mathf.Abs(z);
            if (ax >= ay && ay >= az)
            {
                // x is the max, so we can pick (py, pz) arbitrarily at (1, 1):
                py = 1.0f;
                pz = 1.0f;
                px = -(y * py + z * pz) / x;
            }
            else if (ay >= az)
            {
                // y is the max, so we can pick (px, pz) arbitrarily at (1, 1):
                px = 1.0f;
                pz = 1.0f;
                py = -(x * px + z * pz) / y;
            }
            else
            {
                // z is the max, so we can pick (px, py) arbitrarily at (1, 1):
                px = 1.0f;
                py = 1.0f;
                pz = -(x * px + y * py) / z;
            }
            side = new Vector3(px, py, pz).normalized;
        }
    }

    public void RandomVector(ref Vector3 start, ref Vector3 end, float offsetAmount, out Vector3 result)
    {
        if (orthographic)
        {
            Vector3 directionNormalized = (end - start).normalized;
            Vector3 side = new Vector3(-directionNormalized.y, directionNormalized.x, directionNormalized.z);
            float distance = ((float)RandomGenerator.NextDouble() * offsetAmount * 2.0f) - offsetAmount;
            result = side * distance;
        }
        else
        {
            Vector3 directionNormalized = (end - start).normalized;
            Vector3 side;
            GetPerpendicularVector(ref directionNormalized, out side);

            // generate random distance
            float distance = (((float)RandomGenerator.NextDouble() + 0.1f) * offsetAmount);

            // get random rotation angle to rotate around the current direction
            float rotationAngle = ((float)RandomGenerator.NextDouble() * 360.0f);

            // rotate around the direction and then offset by the perpendicular vector
            result = Quaternion.AngleAxis(rotationAngle, directionNormalized) * side * distance;
        }
    }

    /// <summary>
    /// Trigger a lightning bolt. Use this if ManualMode is true.
    /// </summary>
    public void Trigger()
    {
        Vector3 start, end;
        timer = duration + Mathf.Min(0.0f, timer);

        start = transform.position;
        end = ShootLightningBolt(start, transform.forward);

        startIndex = 0;
        GenerateLightningBolt(start, end, Generations, Generations, 0.0f);
        UpdateLineRenderer();
    }

    private void GenerateLightningBolt(Vector3 start, Vector3 end, int generation, int totalGenerations, float offsetAmount)
    {
        if (generation < 0 || generation > 8)
        {
            return;
        }
        else if (orthographic)
        {
            start.z = end.z = Mathf.Min(start.z, end.z);
        }

        segments.Add(new KeyValuePair<Vector3, Vector3>(start, end));
        if (generation == 0)
        {
            return;
        }

        Vector3 randomVector;
        if (offsetAmount <= 0.0f)
        {
            offsetAmount = (end - start).magnitude * chaosFactor;
        }

        while (generation-- > 0)
        {
            int previousStartIndex = startIndex;
            startIndex = segments.Count;
            for (int i = previousStartIndex; i < startIndex; i++)
            {
                start = segments[i].Key;
                end = segments[i].Value;

                // determine a new direction for the split
                Vector3 midPoint = (start + end) * 0.5f;

                // adjust the mid point to be the new location
                RandomVector(ref start, ref end, offsetAmount, out randomVector);
                midPoint += randomVector;

                // add two new segments
                segments.Add(new KeyValuePair<Vector3, Vector3>(start, midPoint));
                segments.Add(new KeyValuePair<Vector3, Vector3>(midPoint, end));
            }

            // halve the distance the lightning can deviate for each generation down
            offsetAmount *= 0.5f;
        }
    }

    private void UpdateLineRenderer()
    {
        int segmentCount = (segments.Count - startIndex) + 1;
        lineRenderer.positionCount = segmentCount;

        if (segmentCount < 1)
        {
            return;
        }

        int index = 0;
        lineRenderer.SetPosition(index++, segments[startIndex].Key);

        for (int i = startIndex; i < segments.Count; i++)
        {
            lineRenderer.SetPosition(index++, segments[i].Value);
        }

        segments.Clear();
    }

    private Vector3 ShootLightningBolt(Vector3 position, Vector3 direction)
    {
        bool rayHit = Physics.Raycast(position, direction, out var hit, 20, lightningLayerMask);
        Collider hitEnemy;

        // 히트 스캔 방식 - 조준 보정 필요
        if (rayHit)
        {
            // 라이트닝 볼트 위치 지정
            hitEnemy = hit.collider;
        }
        // 직선 거리에 없을 땐 가장 가까운 녀석을 기준으로 함
        else
        {
            Vector3 endPosition = position + maxDistance * direction;
            // 적만 맞추기
            var enemies = Physics.OverlapCapsule(position, endPosition, 3,
                lightningLayerMask);
            if (enemies.Length > 0)
            {
                // 가장 가까운 적을 맞춤
                float minDistance = System.Single.MaxValue;
                hitEnemy = enemies[0];
                foreach (var enemy in enemies)
                {
                    float distance = Vector3.Distance(position, enemy.transform.position);
                    if (distance < minDistance)
                    {
                        hitEnemy = enemy;
                        minDistance = distance;
                    }
                }
            }
            // 만약 직선 거리에서도 없었다면 지형을 맞춘다
            else
            {
                Physics.Raycast(position, direction, out hit, maxDistance, 1 << LayerMask.NameToLayer("Default"));
                return hit.point;
            }
        }

        // 데미지 전달
        var status = hitEnemy.GetComponent<CharacterStatus>();
        if (status)
        {
            status.TakeDamage(elementDamage);
        }

        return hitEnemy.transform.position;
    }

    public override void TurnOn()
    {
        
    }

    public override void TurnOff()
    {
        
    }
}