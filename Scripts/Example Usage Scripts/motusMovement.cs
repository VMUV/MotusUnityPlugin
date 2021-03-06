﻿using UnityEngine;
using System.Collections;
using Valve.VR;
 
[RequireComponent(typeof(CharacterController))]
public class motusMovement : MonoBehaviour
{
    //player is the camerarig
    public GameObject player;
    private CharacterController playerController;
    SteamVR_Controller.Device device;
    SteamVR_TrackedObject controller;
    public float speedMultiplier = 10f;
    public float playerGravity = 10f;
    private AutoOrienter _autoOrienter = new AutoOrienter();
    Vector2 touchpad;

    void Start()
    {
        //Call to initialize the Motus controller
        MotusInput.Start();
        playerController = player.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MotusInput.Update();

        Quaternion tracker = RotationTracker.GetRotation();
        if (!_autoOrienter.IsOriented())
            _autoOrienter.Orient(tracker);

        Vector3 trans = MotusInput.GetNormalizedTranslation();
        if (trans.magnitude == 0.0f)
            MotusInput.SetNewSteeringOffset(tracker);

        Quaternion rot = MotusInput.GetPlayerRotation(tracker);
        trans = MotusInput.GetTrim(trans) * trans;
        trans = rot * trans;
        trans *= speedMultiplier;
        trans += new Vector3(0, -1 * playerGravity, 0);

        if (playerController.isGrounded)
        {
            playerController.Move(trans * Time.deltaTime);
            player.transform.localRotation = new Quaternion(0, 0, 0, 1);
        }
        else
        {
            makePlayerFall();
        }
    }

    private void makePlayerFall()
    {
        playerController.Move(new Vector3(0, -1 * playerGravity * Time.deltaTime, 0));
    }
}
