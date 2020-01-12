using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MiniAdditionalScript01 : MonoBehaviour
{
    public float RotationSpeed = 5;
    Animator Anim;
    NavMeshAgent myAgent;
    public GameObject LockedJumpPosition, CrackDecal, TargetLocker;
    public Vector3 CurrentLockedPosition;
    MiniBossScript01 MBS;

    private bool CanInstantiate,Rotate;

    [Header("EscapeVaiables")]
    public bool LowHealthEscape;
    public Transform EscapePoint;
    public GameObject Obstacle, ReplaceObstacle, ParticleSystem, Player;
    public bool Escape;
    bool LookOnce,DestroyOnce;

    // Start is called before the first frame update
    void Start()
    {
        MBS = GetComponent<MiniBossScript01>();
        if (LowHealthEscape && PlayerPrefs.GetInt(gameObject.name) == 0)
        {
            Obstacle.SetActive(true);
            ReplaceObstacle.SetActive(false);
        }
        Anim = GetComponent<Animator>();
        myAgent = GetComponent<NavMeshAgent>();
    }

    IEnumerator EscapeCor()
    {
        if (!GetComponent<EnemyHealth>().Invinsible)
            GetComponent<EnemyHealth>().Invinsible = true;
        //Camera.main.GetComponentInParent<TopDownCamScript>().target = transform;
        Obstacle.GetComponent<NavMeshObstacle>().enabled = false;
        if (PlayerPrefs.GetInt(MBS.BossName) == 0)
            PlayerPrefs.SetInt(MBS.BossName, 1);
        GetComponent<MiniBossScript01>().enabled = false;
        yield return new WaitForSeconds(15f);
        //Camera.main.GetComponentInParent<TopDownCamScript>().target = Player.transform;
        if(Obstacle.activeSelf)
        {
            StartCoroutine("DestroyCor");
        }
    }

    IEnumerator DestroyCor()
    {
        DestroyOnce = true;
        ParticleSystem.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.2f);
        Obstacle.SetActive(false);
        ReplaceObstacle.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //MBS.DisableHealthBar();
        //Camera.main.GetComponentInParent<TopDownCamScript>().target = Player.transform;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Escape)
        {
            myAgent.speed = 6.5f;
            Anim.SetFloat("Speed", myAgent.desiredVelocity.magnitude);
            Anim.SetBool("JumpAttack", false);
            myAgent.SetDestination(EscapePoint.position);
            Debug.Log((transform.position - Obstacle.transform.position).magnitude);
            if  (((transform.position - Obstacle.transform.position).magnitude < 9.5f) && !DestroyOnce)
            {
                StartCoroutine("DestroyCor");
            }
            if (!LookOnce)
            {
                LookOnce = true;
                StartCoroutine("EscapeCor");                
            }
            return;
        }

        if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Roar"))
        {
            myAgent.speed = 0f;
            Rotate = true;
            return;
        }
        else
        {
            if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                myAgent.speed = 1f;
                Rotate = true;
            }
            else
            {
                myAgent.speed = 5;
                Rotate = false;
            }
        }

        if (MBS.Jumping && MBS.Target != null)
        {
            if (CanInstantiate)
            {
                CanInstantiate = false;
                /*if(MBS.Target.GetComponent<PlayerHealth>())
                    TargetLocker = Instantiate(LockedJumpPosition, MBS.Target.transform.parent.position, Quaternion.identity, null);
                else
                    TargetLocker = Instantiate(LockedJumpPosition, MBS.Target.transform.position, Quaternion.identity, null);*/

                /*if (MBS.Target.GetComponent<PlayerHealth>())
                    G.transform.position = MBS.Target.transform.parent.position;
                else
                    G.transform.position = MBS.Target.transform.position;*/
                CurrentLockedPosition = TargetLocker.transform.position;
            }
        }
        else
        {
            CanInstantiate = true;
        }
    }

    private void LateUpdate()
    {
        if (Rotate && MBS.Target != null && !Escape)
        {
            Quaternion _lookRotation = Quaternion.LookRotation((MBS.Target.transform.position - transform.position).normalized);
            _lookRotation.x = 0; _lookRotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
        }
    }

    public void DestoyLock()
    {
        if (TargetLocker != null)
        {
            Instantiate(CrackDecal, new Vector3(TargetLocker.transform.position.x, TargetLocker.transform.position.y+0.1f, TargetLocker.transform.position.z), Quaternion.identity);
            StartCoroutine("WaitThenInstantiate");
            Destroy(TargetLocker.gameObject);
        }
    }

    IEnumerator WaitThenInstantiate()
    {
        yield return new WaitForEndOfFrame();
        //GetComponentInChildren<InstanceHit>().InstantiatePrefab(1);
    }
}
