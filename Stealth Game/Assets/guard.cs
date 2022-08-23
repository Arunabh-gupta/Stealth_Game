using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guard : MonoBehaviour
{
    GameObject player;
    public Transform pathHolder;
    Vector3[] waypoints;
    public float speed = 5f;
    public float rotatespeed = 5f;
    public float waitTime = 1f;

    public Light spotLight;
    public float viewDistance;
    float viewAngle;
    public LayerMask viewMask;
    Color originalSpotlightColor;
    public UiManager uiManager;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        viewAngle = spotLight.spotAngle;
        originalSpotlightColor = spotLight.color;
        waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i].y = transform.position.y;
        }
        StartCoroutine(TraversePath(waypoints));
    }

    IEnumerator TraversePath(Vector3[] waypoints)
    {
        int targetwaypointIndex = 1;
        Vector3 targetwaypoint = waypoints[targetwaypointIndex];
        transform.LookAt(targetwaypoint);
        while (true)
        {
            // moving the guard at every waypoint
            transform.position = Vector3.MoveTowards(transform.position, targetwaypoint, speed * Time.deltaTime);
            if (transform.position == targetwaypoint)
            {
                yield return new WaitForSeconds(waitTime);
                targetwaypointIndex = (targetwaypointIndex + 1) % waypoints.Length;
                targetwaypoint = waypoints[targetwaypointIndex];
                yield return StartCoroutine(turntoFace(targetwaypoint));
            }
            yield return null;
        }
    }

    IEnumerator turntoFace(Vector3 Looktarget)
    {
        Vector3 dirToLook = (Looktarget - transform.position).normalized;
        float lookAngle = 90 - Mathf.Atan2(dirToLook.z, dirToLook.x) * Mathf.Rad2Deg;
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, lookAngle)) > 0.01f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, lookAngle, rotatespeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }
    private void Update() {
        if(CanSeePlayer()){
            Debug.Log("Player Detected");
            spotLight.color = Color.red;
            uiManager.showGameLoseUI();
            uiManager.newScene();
        }
        else{
            spotLight.color = originalSpotlightColor;
        }
    }
    bool CanSeePlayer()
    {
        if ((player.transform.position - transform.position).magnitude < viewDistance)
        {
            Vector3 dirtoPlayer = player.transform.position - transform.position;
            float angleBetweenPlayerandGuard = Vector3.Angle(transform.forward, dirtoPlayer);
            if (angleBetweenPlayerandGuard < viewAngle / 2f)
            {
                if(!Physics.Linecast(transform.position, player.transform.position, viewMask)){ // to check for inline obstacles
                    return true;
                }
            }
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 prevPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.1f);
            Gizmos.DrawLine(prevPosition, waypoint.position);
            prevPosition = waypoint.position;
        }
        Gizmos.DrawLine(prevPosition, startPosition);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.forward * viewDistance);
    }
}
