using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

	public GameObject model;
	public PlayerInput pi;
	public float walkSpeed = 2.4f;
	public float runMutiplier = 2.0f;
	public float jumpVelocity = 3;
	public float rollVelocity = 1.0f;


	[SerializeField]
	private Animator anim;
	private Rigidbody rigid;
	private Vector3 planarVec;
	private Vector3 thrustVec;

	private bool lockPlanar = false;

	// Use this for initialization
	void Awake () {
		pi = GetComponent<PlayerInput>();
		anim = model.GetComponent<Animator>();
		rigid = GetComponent<Rigidbody> ();

	}
	
	// Update is called once per frame
	void Update () {
		//print(pi.Dup);
		float targetRunMulti = ((pi.run) ? 2.0f: 1.0f);
		//使得run和walk线性过渡
		anim.SetFloat("forward",pi.Dmag * Mathf.Lerp( anim.GetFloat("forward"),targetRunMulti,0.5f));
		if(rigid.velocity.magnitude > 1.0f)
        {
			anim.SetTrigger("roll");
        }
        if (pi.jump)
        {
			anim.SetTrigger("jump");
        }

		if(pi.Dmag > 0.1f)//修复停止动作后恢复正向的bug
        {
			Vector3 targetForward = Vector3.Slerp(model.transform.forward,pi.Dvec,0.1f);//平滑插值角色模型的旋转
			model.transform.forward = targetForward;
        }
		if(lockPlanar == false)
        {
			planarVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run) ? runMutiplier : 1.0f);//当前位置偏移量 * 移动的方向（当下模型的正面） = 移动  又* 是否run
		}
		
	}

	void FixedUpdate()
    {
		//rigid.position += planarVec * Time.fixedDeltaTime;//改位置=速度 * 时间
		rigid.velocity = new Vector3(planarVec.x,rigid.velocity.y, planarVec.z) + thrustVec;//指派速度
		thrustVec = Vector3.zero;
    }

	//
	//Message processing block 处理信号
	//
	public void OnJumpEnter()
    {
		//print("on jump enter");
		pi.inputEnabled = false;
		lockPlanar = true;
		thrustVec = new Vector3(0, jumpVelocity, 0);
    }
    public void OnJumpExit()
    {
        //print("on jump exit");
        pi.inputEnabled = true;
        lockPlanar = false;
    }

    public void IsGround()
    {
		//print("is on ground");
		anim.SetBool("isGround", true);
    }

	public void IsNotGround()
    {
		//print("is not on ground");
		anim.SetBool("isGround", false);

	}

	public void OnGroundEnter()
    {
		pi.inputEnabled = true;
		lockPlanar = false;

    }
	public void OnFallEnter()
    {
		pi.inputEnabled = true;
		lockPlanar = false;
	}

	public void OnRollEnter()
    {
		thrustVec = new Vector3(0, rollVelocity, 0);
		pi.inputEnabled = false;
		lockPlanar = false;
    }

	public void OnJabEnter()
    {
		pi.inputEnabled = false;
		lockPlanar = false;
	}

	public void OnJabUpdate()
    {
		thrustVec = model.transform.forward *  anim.GetFloat("jabVelocity") ;
    }
}
