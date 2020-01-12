using System.Collections;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public class MiniBossScript01 : MonoBehaviour
{
    [Header("Defeated")]
    public string BossName;
    public GameObject[] BossAllies;

    [Header("Public Variables")]
    public GameObject Player;
    public BossZoneScript BossZone;
    public float JumpSpeed;
    float StartRetreat;
    NavMeshAgent myAgent;
    EnemyHealth EH;
    GameObject[] Targets;
    Animator Anim;

    [Header("Other Variables")]
    public GameObject Target;
    public float AttackDistance = 4f;
    public float LookForTarget = 1f;
    public bool Jumping;
    float JumpTimer, AttackInterval;
    Vector3 SpawnPoint;
    bool Move2Target;

    [Header("Rage Variables")]
    public bool RoarOnce;
    public bool Rage;

    // Start is called before the first frame update
    void Start()
    {
        BossName = gameObject.name;
        if (PlayerPrefs.GetInt(BossName) == 1)
        {
            if(BossAllies.Length > 0)
            {
                foreach (GameObject G in BossAllies)
                    G.SetActive(false);
            }
            gameObject.SetActive(false);
            return;
        }
        
        Rage = false;
        RoarOnce = false;
        SpawnPoint = transform.position;
        AttackInterval = 2f;
        myAgent = GetComponent<NavMeshAgent>();
        EH = GetComponent<EnemyHealth>();
        Anim = GetComponent<Animator>();
        JumpTimer = Random.Range(7, 17);
    }

    // Update is called once per frame
    void Update()
    {
        if (!RoarOnce && EH.Health > 1700)
        {
            RoarOnce = true;
        }
        else if (RoarOnce && EH.Health <= 1700)
        {
            if (InBossZone() && !Jumping && AttackInterval > 0.1f && !Rage)
            {
                RoarOnce = false;
                Rage = true;
                StartCoroutine("RageCor");
            }
        }

        if (Rage)
        {
            //GDC.TriggerAllOff();
            return;
        }

        if(Jumping)
        {
            if(!Anim.GetBool("JumpAttack"))
                Anim.SetBool("JumpAttack", true);
            if (!EH.Invinsible || myAgent.enabled)
            {
                EH.Invinsible = true;
                myAgent.enabled = false;
            }
            if(Move2Target)
                transform.position = Vector3.MoveTowards(transform.position, GetComponent<MiniAdditionalScript01>().CurrentLockedPosition, Time.deltaTime * JumpSpeed);
            if ((transform.position - GetComponent<MiniAdditionalScript01>().CurrentLockedPosition).magnitude < 1)
            {
                Move2Target = false;
                GetComponent<MiniAdditionalScript01>().DestoyLock();
                transform.position = GetComponent<MiniAdditionalScript01>().CurrentLockedPosition;
                Camera.main.GetComponent<Animator>().SetTrigger("Shot");
                Anim.SetBool("JumpAttack", false);
                JumpTimer = Random.Range(7, 15);
                StartCoroutine("ResetControl");
            }
            return;
        }
        else
        {
            EH.Invinsible = false;
        }

        Anim.SetFloat("Speed", myAgent.desiredVelocity.magnitude);
        if(InBossZone())
        {
            if (JumpTimer > 0)
                JumpTimer -= Time.deltaTime; // Jump Ability Timer
            if (EH.Regenerate)
                EH.Regenerate = false;
            if (LookForTarget > 0)
                LookForTarget -= Time.deltaTime;
            else
            {
                Targets = GameObject.FindGameObjectsWithTag("Player");
                Target = FindBestTarget();
                if (Target != null)
                    Combat();
            }
        }
        else
        {
            myAgent.SetDestination(SpawnPoint);
            if(!EH.Regenerate)
                EH.Regenerate = true;
        }
    }

    IEnumerator ResetControl()
    {
        yield return new WaitForSeconds(1.5f);
        myAgent.enabled = true;
        Jumping = false;
    }

    bool InBossZone()
    {
        if(!BossZone.PlayerDetected)
        {
            if (StartRetreat > 0)
                StartRetreat -= Time.deltaTime;
        }
        else
        {
            StartRetreat = 5;
        }

        return (StartRetreat > 0);
    }

    void Combat()
    {
        //Debug.Log("Magnitude: "+(transform.position - Target.transform.position).magnitude);
        if ((transform.position - Target.transform.position).magnitude > AttackDistance)
        {
            if (CanJumpAttack())
            {
                Jumping = true;
            }
            else if (!CanJumpAttack() && !Jumping)
            {
                myAgent.SetDestination(Target.transform.position);
            }
        }
        else
        {
            if (AttackInterval > 0)
                AttackInterval -= Time.deltaTime;
            else
            {
                Anim.SetTrigger("Attack");
                if(JumpTimer < 1)
                    JumpTimer += 1;
                AttackInterval = 2;
            }
        }
    }

    GameObject FindBestTarget()
    {
        float Distance = 999;
        GameObject CurrentTarget = null;
        Targets = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject G in Targets)
        {
            if((transform.position - G.transform.position).magnitude < Distance)
            {
                Distance = (transform.position - G.transform.position).magnitude;
                CurrentTarget = G;
            }
        }
        return CurrentTarget;
    }

    bool CanJumpAttack()
    {
        if (JumpTimer > 0)
            return false;
        else
        {
            if ((transform.position - Target.transform.position).magnitude < 21 && (transform.position - Target.transform.position).magnitude > 7)
                return true;
            else
                return false;
        }
    }

    public void GoToTarget()
    {
        Move2Target = true;
    }

    IEnumerator RageCor()
    {
        Debug.Log("RageCor called");
        Anim.SetBool("Roar", true);
        yield return new WaitForSeconds(0.5f);
        if (Camera.main != null)
            Camera.main.GetComponent<Animator>().SetTrigger("Shake");
        GameObject[] Players;
        Players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in Players)
        {
            //if (player.GetComponent<TopDownPlayer>())
            //    player.GetComponent<TopDownPlayer>().StunCurrentPlayer();
            //if (player.GetComponent<AllyMotor>())
            //    player.GetComponent<AllyMotor>().StunCurrentPlayer();
        }
        yield return new WaitForSeconds(3f);
        Anim.SetBool("Roar", false);
        Rage = false;
    }
}
