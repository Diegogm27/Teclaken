using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour
{
    [SerializeField] public Transform kraken;
    private float rotateClockwise;
    [SerializeField] [Range(2, 9)] float minDistance;
    [SerializeField] [Range(3, 10)] float maxDistance;
    
    private float stopDistance;
    NavMeshAgent agent;
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.SetDestination(kraken.position);
        stopDistance = Random.Range(minDistance, maxDistance);
        agent.stoppingDistance = stopDistance;

        //Rotate randomly clock or counter clockwise
        rotateClockwise = Random.Range(0, 2) * 2 - 1;
    }

    // Update is called once per frame
    void Update() {

        //Rotate ship to face velocity direction
        if (Vector3.SignedAngle(transform.up, agent.velocity, Vector3.forward) > 1f) {
            transform.Rotate(transform.forward.normalized * agent.angularSpeed * Time.deltaTime);
        }
        else if (Vector3.SignedAngle(transform.up, agent.velocity, Vector3.forward) < -1f) {
            transform.Rotate(transform.forward.normalized * -agent.angularSpeed * Time.deltaTime);
        }

        //if closer than stoping distance go outwards
        if (Vector3.Distance(transform.position, kraken.position) < stopDistance) {
            var offset = kraken.transform.position - transform.position;
            agent.SetDestination(transform.position - offset);
            agent.stoppingDistance = 0;
        }
        //if close to stoping distance, orbit around
        else if (Vector3.Distance(transform.position, kraken.position) <= stopDistance + 2) {
            var offset = kraken.transform.position - transform.position;
            var dir = Vector3.Cross(rotateClockwise * offset, Vector3.forward);
            agent.SetDestination(transform.position + dir);
            agent.stoppingDistance = stopDistance;
            GetComponent<Ship>().canAttack = true;
        }
        //if farther than stoping distance, go to kraken
        else { 
            agent.SetDestination(kraken.position);
            agent.stoppingDistance = stopDistance;
        }
        
    }
}
