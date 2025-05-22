using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] Transform body = default;
    [SerializeField] IKFootSolver otherFoot1 = default;
    [SerializeField] IKFootSolver otherFoot2 = default;
    [SerializeField] float speed = 5;
    [SerializeField] float stepDistance = 1;
    [SerializeField] float stepLength = 1;
    [SerializeField] float stepHeight = 1;
    [SerializeField] Vector3 footOffset = default;

    float footSpacing;
    Vector3 oldPosition, currentPosition, newPosition;
    Vector3 oldNormal, currentNormal, newNormal;
    float lerp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        footSpacing = transform.localPosition.x;
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = currentPosition;
        transform.up = currentNormal;

        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit info, 5, terrainLayer.value))
        {
            if (Vector3.Distance(newPosition, info.point) > stepDistance && !otherFoot1.IsMoving() && !otherFoot2.IsMoving() && lerp >= 1)
            {
                lerp = 0;

                Vector3 localFootPosition = body.InverseTransformPoint(transform.position);
                Vector3 localTargetPosition = body.InverseTransformPoint(info.point);
                Vector3 targetPosition = info.point + footOffset;

                //XÃà 
                if (Mathf.Abs(localTargetPosition.x - localFootPosition.x) > stepDistance)
                {
                    int directionX = localTargetPosition.x > localFootPosition.x ? 1 : -1;
                    targetPosition = info.point + (body.right * stepLength * directionX) + footOffset;
                }

                //ZÃà
                if (Mathf.Abs(localTargetPosition.z - localFootPosition.z) > stepDistance)
                {
                    int directionZ = localTargetPosition.z > localFootPosition.z ? 1 : -1;
                    targetPosition = info.point + (body.forward * stepLength * directionZ) + footOffset;
                }

                newPosition = Vector3.Lerp(newPosition, targetPosition, 0.8f);
                transform.up = info.normal;
            }
        }

        if (lerp < 1)
        {
            Vector3 footPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            footPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = footPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.2f);
    }

    public bool IsMoving()
    {
        return lerp < 1;
    }
}

