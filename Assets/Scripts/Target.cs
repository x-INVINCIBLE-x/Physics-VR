using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float offset = 0.1f;
    [SerializeField] private int currPoint = 0;
    [SerializeField] private float speed;
    private Vector3 initialPosition;
    private float lastPointChanged;
    private float pointChangeCooldown = 2f;

    private float scale;

    //[SerializeField] private GameObject bulletImpactprefab;
    public bool oneHitTarget = false;

    [Header("Scoring")]
    public bool hasScoring;

    /// <summary>
    /// If the item is one hit then should it ise 1 - 10 scoring or only one score whereever it hits
    /// </summary>
    public int oneHitScore;

    private void Start()
    {
        initialPosition = transform.position;
        scale = transform.localScale.x;
    }

    private void Update()
    {
        MoveTowardPatrolPoint();
    }

    private void MoveTowardPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        Vector3 translation = speed * Time.deltaTime * (patrolPoints[currPoint].position - transform.position).normalized;
        transform.Translate(translation.x, translation.y, 0);
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        float distance = Vector3.Distance(transform.position, patrolPoints[currPoint].position);

        if (distance > offset || lastPointChanged > Time.time + pointChangeCooldown)
            return;

        lastPointChanged = Time.time;
        currPoint = currPoint + 1 == patrolPoints.Length ? 0 : currPoint + 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            //GameObject newImpact = Instantiate(bulletImpactprefab, collision.contacts[0].point, bulletImpactprefab.transform.rotation);
            //newImpact.transform.parent = transform;

            //float dist = (transform.position - collision.contacts[0].point).magnitude;

            ////Debug.Log((transform.position - collision.contacts[0].point).magnitude);
            ////0.3 distancxe on 1 Scale

            //int score;
            //if (hasScoring)
            //{
            //    score = Mathf.Max((10 - Mathf.FloorToInt((dist / (0.3f * scale)) * 10)), 0);
            //}
            //else
            //{
            //    score = oneHitScore;
            //}

            ////Debug.Log("Score: " + score);
        }
    }
}
