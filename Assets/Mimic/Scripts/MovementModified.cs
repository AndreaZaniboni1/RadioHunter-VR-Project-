using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MimicSpace
{
    /// <summary>
    /// This is a very basic movement script, if you want to replace it
    /// Just don't forget to update the Mimic's velocity vector with a Vector3(x, 0, z)
    /// </summary>
    public class MovementModified : MonoBehaviour
    {
        [Header("Controls")]
        [Tooltip("Body Height from ground")]
        [Range(0.5f, 5f)]
        public float height = 0.8f;
        public float speed = 5f;
        Vector3 velocity = Vector3.zero;
        public float velocityLerpCoef = 4f;
        Mimic myMimic;

        public Transform player;
        private NavMeshAgent agent;
        public Transform centreOfRadius;
        public float rangePatrol; //radius of sphere
        private Vector3 meshDestination;
        public bool isSeen;

        public LayerMask playerMask;
        public LayerMask obstacleMask;
        public float viewRadius;
        public float viewAngle;
        public float attackRange;
        Vector3 oldPlayerPosition = Vector3.zero;  

        public bool seesPlayer=false;
        public bool playerInAttackRange=false;

        public float attackCooldown;
        float lastHit;
        public PlayerHP playerHP;
        public GameObject chaseMusic;


        public Material mimicColor;



        private static readonly Vector4[] s_UnitSphere = MakeUnitSphere(16);

        private void Start()
        {
            agent=GetComponent<NavMeshAgent>();
            myMimic = GetComponent<Mimic>();
            //mimicColor = sphereGO.GetComponent<Renderer>();
        }
        private static Vector4[] MakeUnitSphere(int len)
        {
            Debug.Assert(len > 2);
            var v = new Vector4[len * 3];
            for (int i = 0; i < len; i++)
            {
                var f = i / (float)len;
                float c = Mathf.Cos(f * (float)(Math.PI * 2.0));
                float s = Mathf.Sin(f * (float)(Math.PI * 2.0));
                v[0 * len + i] = new Vector4(c, s, 0, 1);
                v[1 * len + i] = new Vector4(0, c, s, 1);
                v[2 * len + i] = new Vector4(s, 0, c, 1);
            }
            return v;
        }
        public static void DrawSphere(Vector4 pos, float radius, Color color)
        {
            Vector4[] v = s_UnitSphere;
            int len = s_UnitSphere.Length / 3;
            for (int i = 0; i < len; i++)
            {
                var sX = pos + radius * v[0 * len + i];
                var eX = pos + radius * v[0 * len + (i + 1) % len];
                var sY = pos + radius * v[1 * len + i];
                var eY = pos + radius * v[1 * len + (i + 1) % len];
                var sZ = pos + radius * v[2 * len + i];
                var eZ = pos + radius * v[2 * len + (i + 1) % len];
                Debug.DrawLine(sX, eX, color);
                Debug.DrawLine(sY, eY, color);
                Debug.DrawLine(sZ, eZ, color);
            }
        }

        void Update()
        { 
            Color coloras = Color.yellow;
            DrawSphere(centreOfRadius.position, rangePatrol, coloras);

            //enemy senses
            UnityEngine.Color colora = UnityEngine.Color.red;
            UnityEngine.Color colors = UnityEngine.Color.green;

            //debug rapresentation of senses
            DrawSphere(transform.position, viewRadius, colora);
            var direction = Quaternion.AngleAxis(viewAngle/2, transform.up) * transform.forward;
            var direction2 = Quaternion.AngleAxis(-viewAngle / 2, transform.up) * transform.forward;
            Debug.DrawRay(transform.position, direction*viewRadius, colors);
            Debug.DrawRay(transform.position, direction2*viewRadius, colors);
            Vector3 playerTarget = (player.position - transform.position).normalized;
            //debug rapresentation of senses

            if (Vector3.Angle(transform.forward, playerTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, player.position);
                if (distanceToTarget < viewRadius)
                {
                    if (Physics.Raycast(transform.position, playerTarget,distanceToTarget,obstacleMask) == false)
                    {
                        Debug.DrawLine(transform.position, player.position, colors);
                        seesPlayer = true;

                    }
                    else { seesPlayer = false; }
                }
                else { seesPlayer = false; }
            }
            //enemy senses

            //Debug.Log(oldPlayerPosition);

            if (!seesPlayer && oldPlayerPosition == Vector3.zero)
                {
                    if (chaseMusic.activeSelf == true) { chaseMusic.SetActive(false); }
                    agent.isStopped = false;
                    Patroling();
                }
            else
            {
                if (chaseMusic.activeSelf == false) { chaseMusic.SetActive(true); }
                

                if (seesPlayer)
                {
                    oldPlayerPosition = player.position;
                    if (!isSeen)
                        {
                            Chasing();
                        }
                        else
                        {
                            agent.isStopped=true;
                        }
                }
                else { agent.isStopped = false; ReachingLastPosition(); }
            }


}

        private void ReachingLastPosition()
        {
            agent.SetDestination(oldPlayerPosition);

            Debug.Log("Reaching Last Position");
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                oldPlayerPosition = Vector3.zero;
            }
            else
            {
                
                transform.LookAt(oldPlayerPosition);
                myMimic.velocity = agent.velocity;
                transform.position = transform.position + velocity * Time.deltaTime;

            }

        }

        private void Chasing()
        {
            agent.SetDestination(player.position);
            transform.LookAt(player.position);
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Attack();
            }
            else
            {
                Debug.Log("Chasing");
                agent.SetDestination(player.position);
                myMimic.velocity = agent.velocity;
                transform.position = transform.position + velocity * Time.deltaTime;


            }


        }

        private void Attack()
        {
            if (Time.time-lastHit < attackCooldown)
            {
                return;
            }
            else
            {
                myMimic.ResetMimic();
                lastHit = Time.time;
                Debug.Log("Attacking");
                myMimic.LegAttack(player.position);
                myMimic.LegAttack(player.position);
                myMimic.LegAttack(player.position);
                playerHP.TakeDamage(1);
            }
        }

        public void IsTrulySeen()
        {
            RaycastHit hit;
                if (Physics.Raycast(transform.position, (player.position - transform.position), out hit))
                {
                    //Debug.DrawRay(transform.position, (player.position - transform.position), Color.red);
                    //Debug.Log(hit.transform.gameObject.layer);
                    if (hit.transform.gameObject.layer == 3)
                    {
                        isSeen = true;
                    }
                else
                {
                    isSeen = false;
                }
                }
        }

        bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {

            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range; //random point in a sphere 
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
            {
                //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
                //or add a for loop like in the documentation
                result = hit.position;
                return true;
            }

            result = Vector3.zero;
            return false;
        }

        public void Patroling()
        {

                Debug.Log("Patroling");
                if (agent.remainingDistance <= agent.stoppingDistance) //done with path
                {
                    Vector3 point;
                    if (RandomPoint(centreOfRadius.position, rangePatrol, out point)) //pass in our centre point and radius of area
                    {
                        meshDestination = point;
                        Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                        agent.SetDestination(point);
                        transform.LookAt(point);
                }
                }
            transform.position = transform.position + velocity * Time.deltaTime;
            myMimic.velocity = agent.velocity;

            //velocity = Vector3.Lerp(velocity, new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * speed, velocityLerpCoef * Time.deltaTime);
            // Assigning velocity to the mimic to assure great leg placement

            //RaycastHit hit;
            //Vector3 destHeight = transform.position;
            //if (Physics.Raycast(transform.position , -Vector3.up, out hit,Mathf.Infinity,7))
            //Debug.Log(hit.transform.gameObject.layer.ToString());
            //destHeight = new Vector3(transform.position.x, hit.point.y + height, transform.position.z);
            //transform.position = Vector3.Lerp(transform.position, destHeight, velocityLerpCoef * Time.deltaTime);
        }
    }

}