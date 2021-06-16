using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class WeaponGun : MonoBehaviour
{
    public SteamVR_Action_Boolean fireAction;
    public Transform barrelExit;
    public Transform shellEject;
    public GameObject gunShell;
    public GameObject pBulletParticle;
    public AudioClip gunFire;
    public int range;
    public int damage;
    
    private Interactable _interactable;
    private AudioSource _audioSource;
    private Animation _animation;
    private bool _grabbed = false;

    private void Start()
    {
        _interactable = GetComponent<Interactable>();
        _audioSource = GetComponent<AudioSource>();
        _animation = GetComponent<Animation>();
    }

    // Update is called once per frame
    private void Update()
    {
        
        // If gun is being held in hand
        if (_interactable.attachedToHand != null)
        {
            _grabbed = true;
            SteamVR_Input_Sources source = _interactable.attachedToHand.handType;
            if (fireAction[source].stateDown)
            {
                Fire();
            }
        }
        else
        {
            _grabbed = false;
        }

        if (_grabbed)
        {
            // Keep the object from colliding with player controller
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = LayerMask.NameToLayer("Player");
            }
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
        else if(!_grabbed)
        {
            // let the object collide with player controller again
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Fire()
    {
        // Vector3 barrelFwd = barrelExit.TransformDirection(Vector3.forward);
        Destroy(Instantiate(pBulletParticle, barrelExit), 2f);
        if (Physics.Raycast(barrelExit.position, barrelExit.forward * range, out var hit))
        {
            Debug.DrawRay(barrelExit.position, barrelExit.forward * range);
            hit.collider.SendMessage("ApplyDamage", SendMessageOptions.DontRequireReceiver);
        }
        _audioSource.PlayOneShot(gunFire);
        _animation.Play("GunFire");
    }

    private void EjectShell()
    {
        var shell = Instantiate(gunShell, shellEject);
        shell.GetComponent<Rigidbody>().AddForce(shell.transform.right * 2f, ForceMode.Impulse);
        shell.transform.SetParent(null);
        Destroy(shell, 5f);
    }
}
