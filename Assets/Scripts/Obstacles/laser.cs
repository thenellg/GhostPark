using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    public LineRenderer lineOfSight;
    public LayerMask layerDetection;
    private PlayerController player;

    [Header("Length")]
    public bool active = false;
    public float maxRayDistance;
    public float actualDistance = 0f;
    public float length = 0f;
    public float drawTime = 0.1f;
    public bool changingDistance = false;

    [Header("Timer")]
    public bool timer = false;
    public bool timerRecurring = true;
    public float timerAmount = 3f;
    public float currentTime = 0f;


    [Header("Reflection")]
    public bool canReflect = false;
    public int reflections;

    [Header("Rotation")]
    public bool canRotate = false;
    public bool rotateRight = true;
    public float rotationSpeed;

    public bool limitedRotation = false;
    [Range(0, 1f)] [SerializeField] public float maxRotate = 0f;
    [Range(-1, 0f)] [SerializeField] public float minRotate = 0f;
    
    public Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.rotation;
        player = FindObjectOfType<PlayerController>();
        Physics2D.queriesStartInColliders = false;

        if (!changingDistance)
            actualDistance = maxRayDistance;
    }

    // Update is called once per frame
    private void Update()
    {
        if (timer)
        {
            if (currentTime >= timerAmount)
            {
                if (!timerRecurring)
                    timer = false;

                currentTime = 0;
                active = !active;
            }
            else
            {
                currentTime += Time.deltaTime;
            }
        }

        if (active)
        {
            if (actualDistance < maxRayDistance)
                actualDistance += drawTime;
            if (actualDistance > maxRayDistance)
                actualDistance = maxRayDistance;

            if (canRotate)
            {
                if (limitedRotation)
                {
                    if (transform.rotation.z >= maxRotate)
                    {
                        Debug.Log("rotate over max");
                        rotateRight = false;
                    }
                    else if (transform.rotation.z <= minRotate)
                    {
                        Debug.Log("rotate under min");
                        rotateRight = true;
                    }
                }

                if (rotateRight)
                    transform.Rotate(rotationSpeed * Vector3.forward * Time.deltaTime);
                else
                    transform.Rotate(-rotationSpeed * Vector3.forward * Time.deltaTime);
            }

            lineOfSight.positionCount = 1;
            lineOfSight.SetPosition(0, transform.position);

            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, actualDistance - length, layerDetection);
            // Ray
            Ray2D ray = new Ray2D(transform.position, transform.right);

            bool isMirror = false;
            Vector2 mirrorHitPoint = Vector2.zero;
            Vector2 mirrorHitNormal = Vector2.zero;

            for (int i = 0; i < reflections; i++)
            {
                lineOfSight.positionCount += 1;

                if (hitInfo.collider != null)
                {
                    lineOfSight.SetPosition(lineOfSight.positionCount - 1, hitInfo.point - ray.direction * -0.1f);

                    isMirror = false;
                    if (hitInfo.collider.CompareTag("Mirror"))
                    {
                        mirrorHitPoint = hitInfo.point;
                        mirrorHitNormal = hitInfo.normal;
                        hitInfo = Physics2D.Raycast((Vector2)hitInfo.point - ray.direction * -0.1f, Vector2.Reflect(hitInfo.point - ray.direction * -0.1f, hitInfo.normal), actualDistance - length, layerDetection);
                        isMirror = true;

                        if(i > 0)
                            length += Vector2.Distance(mirrorHitPoint, (Vector2)lineOfSight.GetPosition(i-1));
                    }
                    else if (hitInfo.collider.CompareTag("Player") || hitInfo.collider.CompareTag("Minecart"))
                    {
                        if (!player.controller.m_Settings.invincibility)
                        {
                            player.onDeath();
                            resetLaser();
                        }
                    }
                    else
                        break;
                }
                else
                {
                    if (isMirror)
                    {
                        lineOfSight.SetPosition(lineOfSight.positionCount - 1, mirrorHitPoint + Vector2.Reflect(mirrorHitPoint, mirrorHitNormal).normalized * (actualDistance - length));
                        break;
                    }
                    else
                    {
                        lineOfSight.SetPosition(lineOfSight.positionCount - 1, transform.position + transform.right * (actualDistance - length));
                        break;
                    }
                }
            }
        }
        else
        {
            actualDistance = 0f;
            lineOfSight.positionCount = 0;
        }
    }

    void resetLaser()
    {
        transform.rotation = originalRotation;
        rotateRight = true;
        actualDistance = 0f;
    }
}
