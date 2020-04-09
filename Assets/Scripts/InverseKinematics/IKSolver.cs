#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class IKSolver : MonoBehaviour
{
    public int chainLength = 2;

    public Transform target;
    public Transform pole;

    [Header("Solver Parameters")]
    public int iterations = 10;

    public float delta = 0.001f;

    [Range(0, 1)]
    public float snapBackStrength = 1f;

    protected float[] bonesLength;
    protected float completeLength;
    protected Transform[] joints;
    protected Vector3[] jointPositions;
    protected Vector3[] startDirectionSucc;
    protected Quaternion[] startRotationJoint;
    protected Quaternion startRotationTarget;
    protected Transform chainRoot;

    //[Header("Corrections")];
    //public GameObject 

    private int loop;
    private int innerLoop;
    private float angle;
    private Plane plane;
    private Vector3 targetPosition;
    private Vector3 direction;
    private Vector3 polePosition;
    private Vector3 projectedPole;
    private Vector3 projectedBone;
    private Transform current;
    //private Quaternion targetRotation;

    void Awake()
    {
        IKSetup();
    }

    void IKSetup()
    {
        joints = new Transform[chainLength + 1];
        jointPositions = new Vector3[chainLength + 1];
        bonesLength = new float[chainLength];
        startDirectionSucc = new Vector3[chainLength + 1];
        startRotationJoint = new Quaternion[chainLength + 1];

        chainRoot = transform;
        for (loop = 0; loop <= chainLength; loop++)
        {
            if (chainRoot == null)
            {
                throw new UnityException("The chain value is longer than the ancestor chain!");
            }
            chainRoot = chainRoot.parent;
        }

        if (target == null)
        {
            target = new GameObject(gameObject.name + " Target").transform;
            SetPositionRootSpace(target, GetPositionRootSpace(transform));
        }
        startRotationTarget = GetRotationRootSpace(target);


        current = transform;
        completeLength = 0;
        for (loop = joints.Length - 1; loop >= 0; loop--)
        {
            joints[loop] = current;
            startRotationJoint[loop] = GetRotationRootSpace(current);

            if (loop == joints.Length - 1)
            {
                startDirectionSucc[loop] = GetPositionRootSpace(target) - GetPositionRootSpace(current);
            }
            else
            {
                startDirectionSucc[loop] = GetPositionRootSpace(joints[loop + 1]) - GetPositionRootSpace(current);
                bonesLength[loop] = startDirectionSucc[loop].magnitude;
                completeLength += bonesLength[loop];
            }

            current = current.parent;
        }

    }

    void LateUpdate()
    {
        ResolveIK();
    }

    private void ResolveIK()
    {
        if (target == null)
        {
            return;
        }

        if (bonesLength.Length != chainLength)
        {
            IKSetup();
        }

        for (loop = 0; loop < joints.Length; loop++)
        {
            jointPositions[loop] = GetPositionRootSpace(joints[loop]);
        }

        targetPosition = GetPositionRootSpace(target);
        //targetRotation = GetRotationRootSpace(target);

        //1st is possible to reach?
        if ((targetPosition - GetPositionRootSpace(joints[0])).sqrMagnitude >= completeLength * completeLength)
        {
            //just strech it
            direction = (targetPosition - jointPositions[0]).normalized;
            //set everything after root
            for (loop = 1; loop < jointPositions.Length; loop++)
            {
                jointPositions[loop] = jointPositions[loop - 1] + direction * bonesLength[loop - 1];
            }
        }
        else
        {
            for (loop = 0; loop < jointPositions.Length - 1; loop++)
            {
                jointPositions[loop + 1] = Vector3.Lerp(jointPositions[loop + 1], jointPositions[loop] + startDirectionSucc[loop], snapBackStrength);
            }

            for (loop = 0; loop < iterations; loop++)
            {
                for (innerLoop = jointPositions.Length - 1; innerLoop > 0; innerLoop--)
                {
                    if (innerLoop == jointPositions.Length - 1)
                    {
                        jointPositions[innerLoop] = targetPosition; //set it to target
                    }
                    else
                    {
                        jointPositions[innerLoop] = jointPositions[innerLoop + 1] + (jointPositions[innerLoop] - jointPositions[innerLoop + 1]).normalized * bonesLength[innerLoop]; //set in line on distance
                    }
                }

                //forward
                for (innerLoop = 1; innerLoop < jointPositions.Length; innerLoop++)
                {
                    jointPositions[innerLoop] = jointPositions[innerLoop - 1] + (jointPositions[innerLoop] - jointPositions[innerLoop - 1]).normalized * bonesLength[innerLoop - 1];
                }

                //close enough?
                if ((jointPositions[jointPositions.Length - 1] - targetPosition).sqrMagnitude < delta * delta)
                {
                    break;
                }
            }
        }

        //move towards pole
        if (pole != null)
        {
            polePosition = GetPositionRootSpace(pole);
            for (loop = 1; loop < jointPositions.Length - 1; loop++)
            {
                plane = new Plane(jointPositions[loop + 1] - jointPositions[loop - 1], jointPositions[loop - 1]);
                projectedPole = plane.ClosestPointOnPlane(polePosition);
                projectedBone = plane.ClosestPointOnPlane(jointPositions[loop]);
                angle = Vector3.SignedAngle(projectedBone - jointPositions[loop - 1], projectedPole - jointPositions[loop - 1], plane.normal);
                jointPositions[loop] = Quaternion.AngleAxis(angle, plane.normal) * (jointPositions[loop] - jointPositions[loop - 1]) + jointPositions[loop - 1];
            }
        }

        //set position & rotation
        for (loop = 0; loop < jointPositions.Length; loop++)
        {
            if (loop == jointPositions.Length - 1)
            {
                //Not setting root rotations right now (Hands, Legs)
                //Modify logic here if skeletons start getting more complicated and setup requires root rotations as well
                //SetRotationRootSpace(joints[loop], originalRotation);//Quaternion.Inverse(targetRotation) * startRotationTarget * Quaternion.Inverse(startRotationJoint[loop])
            }
            else
            {
                SetRotationRootSpace(joints[loop], Quaternion.FromToRotation(startDirectionSucc[loop], jointPositions[loop + 1] - jointPositions[loop]) * Quaternion.Inverse(startRotationJoint[loop]));
            }
            SetPositionRootSpace(joints[loop], jointPositions[loop]);
        }
    }

    private Vector3 GetPositionRootSpace(Transform current)
    {
        if (chainRoot == null)
        {
            return current.position;
        }
        return Quaternion.Inverse(chainRoot.rotation) * (current.position - chainRoot.position);
    }

    private void SetPositionRootSpace(Transform current, Vector3 position)
    {
        if (chainRoot == null)
        {
            current.position = position;
        }
        else
        {
            current.position = chainRoot.rotation * position + chainRoot.position;
        }
    }

    private Quaternion GetRotationRootSpace(Transform current)
    {
        //inverse(after) * before => rot: before -> after
        if (chainRoot == null)
        {
            return current.rotation;
        }
        return Quaternion.Inverse(current.rotation) * chainRoot.rotation;
    }

    private void SetRotationRootSpace(Transform current, Quaternion rotation)
    {
        if (chainRoot == null)
        {
            current.rotation = rotation;
        }
        else
        {
            current.rotation = chainRoot.rotation * rotation;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Transform current = this.transform;
        for (int i = 0; i < chainLength && current != null && current.parent != null; i++)
        {
            float scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;
            Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, Vector3.Distance(current.parent.position, current.position), scale));
            Handles.color = Color.green;
            Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
            current = current.parent;
        }
    }
#endif

}