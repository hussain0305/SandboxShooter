using UnityEngine;

class IKProceduralAnimation : MonoBehaviour
{
    public Transform[] footTarget;
    public Transform lookTarget;
    public Transform handTarget;
    public Transform handPole;
    //public Transform step;
    //public Transform attraction;

    public void LateUpdate()
    {
        ////move step & attraction
        //step.Translate(Vector3.forward * Time.deltaTime * 0.7f);
        //if (step.position.z > 1f)
        //    step.position = step.position + Vector3.forward * -2f;
        //attraction.Translate(Vector3.forward * Time.deltaTime * 0.5f);
        //if (attraction.position.z > 1f)
        //    attraction.position = attraction.position + Vector3.forward * -2f;

        ////footsteps
        //for (int i = 0; i < footTarget.Length; i++)
        //{
        //    var foot = footTarget[i];
        //    var ray = new Ray(foot.transform.position + Vector3.up * 0.5f, Vector3.down);
        //    var hitInfo = new RaycastHit();
        //    if (Physics.SphereCast(ray, 0.05f, out hitInfo, 0.50f))
        //        foot.position = hitInfo.point + Vector3.up * 0.05f;
        //}

        ////hand and look
        //var normDist = Mathf.Clamp((Vector3.Distance(lookTarget.position, attraction.position) - 0.3f) / 1f, 0, 1);
        //handTarget.rotation = Quaternion.Lerp(Quaternion.Euler(90, 0, 0), handTarget.rotation, normDist);
        //handTarget.position = Vector3.Lerp(attraction.position, handTarget.position, normDist);
        //handPole.position = Vector3.Lerp(handTarget.position + Vector3.down * 2, handTarget.position + Vector3.forward * 2f, normDist);
        //lookTarget.position = Vector3.Lerp(attraction.position, lookTarget.position, normDist);
    }
}