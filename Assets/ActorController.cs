using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

	public GameObject model;
	public PlayerInput pi;
	public float walkSpeed = 2.0f;
	public float runMutiplier = 2.0f;

	[SerializeField]
	private Animator anim;
	private Rigidbody rigid;
	private Vector3 movingVec; 

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
        if (pi.jump)
        {
			anim.SetTrigger("jump");
        }

		if(pi.Dmag > 0.1f)//修复停止动作后恢复正向的bug
        {
			Vector3 targetForward = Vector3.Slerp(model.transform.forward,pi.Dvec,0.1f);//平滑插值角色模型的旋转
			model.transform.forward = targetForward;
        }
		movingVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run)?runMutiplier:1.0f);//当前位置偏移量 * 移动的方向（当下模型的正面） = 移动  又* 是否run
	}

	void FixedUpdate()
    {
		//rigid.position += movingVec * Time.fixedDeltaTime;//改位置=速度 * 时间
		rigid.velocity = new Vector3(movingVec.x,rigid.velocity.y, movingVec.z);//指派速度
    }
}
