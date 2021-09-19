using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

/*
 * このスクリプトをアタッチするには、
 * NavMeshAgentとThirdPersonCharactorがアタッチされている必要がある。
 * NavMeshAgentは経路探索のため、ThirdPersonCharactorはオブジェクトを移動させるために必要
 */
[RequireComponent(typeof(NavMeshAgent), typeof(ThirdPersonCharacter))]
public class MoveToTarget : MonoBehaviour {
    [SerializeField] private GameObject destination;    //目的地となるGameObject
    [SerializeField] private bool animation = true;     //移動の際にアニメーションを使用するかどうか
    [SerializeField] private float speed = 1.5f;        //移動速度
    private NavMeshAgent _navAgent;
    private ThirdPersonCharacter _character;
    void Start() {
        _navAgent = gameObject.GetComponent<NavMeshAgent>();
        _character = gameObject.GetComponent<ThirdPersonCharacter>();
        _navAgent.updatePosition = !animation;  //アニメーションを付ける場合は、NavMeshAgent側でGemeObjectを移動させる機能を止める
        _navAgent.speed = speed;                //NavMeshAgentの速度を設定している。
    }

    void Update() {
        _navAgent.SetDestination(destination.transform.position);   //NavMeshAgentに目的地を設定している。毎フレームやっているので移動する目標にも対応できる
        if (animation && !_navAgent.isStopped) {
            //GameObjectからNavMeshAgentまでのベクトルを求めている
            Vector3 moveVec = (_navAgent.nextPosition - transform.position);
            //ThirdPersonCharactorのMove関数に移動したいベクトルを渡すと移動＋アニメーション再生をしてくれる。
            _character.Move(moveVec, false, false);
        }
    }
    
    private void OnCollisionEnter(Collision other) {
        //何かにぶつかった時、NavMeshAgentとGameObjectの位置がずれていってしまうのを防ぐためにAgentとGameObjectの位置を合わせている。
        _navAgent.Warp(transform.position);
    }
}
