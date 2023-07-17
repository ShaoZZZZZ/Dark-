using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

	public GameObject model;
	public PlayerInput pi;
	public float walkSpeed = 2.0f;

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
		anim.SetFloat("forward",pi.Dmag);
		if(pi.Dmag > 0.1f)//修复停止动作后恢复正向的bug
        {
			model.transform.forward = pi.Dvec;
        }
		movingVec = pi.Dmag * model.transform.forward * walkSpeed;//当前位置偏移量 * 移动的方向（当下模型的正面） = 移动
	}

	void FixedUpdate()
    {
		//rigid.position += movingVec * Time.fixedDeltaTime;//改位置=速度 * 时间
		rigid.velocity = new Vector3(movingVec.x,rigid.velocity.y, movingVec.z);//指派速度
    }
}
