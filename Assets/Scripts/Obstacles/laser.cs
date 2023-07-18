using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    public LineRenderer lineOfSight;
    public float maxRayDistance;
    public LayerMask layerDetection;
    private PlayerController player;

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
    }

    // Update is called once per frame
    private void Update()
    {
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

            if(rotateRight)
                transform.Rotate(rotationSpeed * Vector3.forward * Time.deltaTime);
            else
                transform.Rotate(-rotationSpeed * Vector3.forward * Time.deltaTime);
        }

        lineOfSight.positionCount = 1;
        lineOfSight.SetPosition(0, transform.position);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, maxRayDistance, layerDetection);
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
                    mirrorHitPoint = (Vector2)hitInfo.point;
                    mirrorHitNormal = (Vector2)hitInfo.normal;
                    hitInfo = Physics2D.Raycast((Vector2)hitInfo.point - ray.direction * -0.1f, Vector2.Reflect(hitInfo.point - ray.direction * -0.1f, hitInfo.normal), maxRayDistance, layerDetection);
                    isMirror = true;
                }
                else
                    break;
            }
            else
            {
                if (isMirror)
                {
                    lineOfSight.SetPosition(lineOfSight.positionCount - 1, mirrorHitPoint + Vector2.Reflect(mirrorHitPoint, mirrorHitNormal) * layerDetection);
                    break;
                }
                else
                {
                    lineOfSight.SetPosition(lineOfSight.positionCount - 1, transform.position + transform.right * maxRayDistance);
                    break;
                }
            }
        }

    }

    void resetLaser()
    {
        transform.rotation = originalRotation;
        rotateRight = true;
    }
}
