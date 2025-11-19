using System;
using System.Collections.Generic;
using UnityEngine;

public class MovePuzzlePiece : MonoBehaviour
{
    [SerializeField] private float moveAmount;
    [SerializeField] private PuzzleManager puzzleManager;
    [SerializeField] private List<GameObject> actuators;
    [SerializeField] private List<ParticleSystem> visualFeedback;
    [SerializeField] private List<AudioSource> audioFeedback;
    
    private bool _activated;
    private bool[] _locks;

    private void Awake()
    {
        _locks = new bool[3];
        ResetLocks();
        puzzleManager.SubscribeNewPuzzlePieceMover(this);
    }

    private void Start()
    {
        visualFeedback = new List<ParticleSystem>();
        audioFeedback = new List<AudioSource>();
        foreach (var actuator in actuators)
        {
            visualFeedback.Add(actuator.GetComponentInChildren<ParticleSystem>());
            audioFeedback.Add(actuator.GetComponentInChildren<AudioSource>());
        }
    }

    public void ResetLocks()
    {
        _locks[0] = false;
        _locks[1] = false;
        _locks[2] = false;
    }

    public void Move()
    {
        Move(moveAmount);
    }

    public void Move(float move)
    {
        if (_activated) return;
        
        transform.localPosition += new Vector3(0f, move, 0f);
        puzzleManager.UpdateAllLightCasters();
        _activated = true;
        VisualFeedback();
        AudioFeedback();
    }

    public void MoveWithTwo(int lockId)
    {
        if (lockId is < 0 or > 1) return;
        
        _locks[lockId] = true;
        
        if (!_locks[0] || !_locks[1]) return;
        
        Move();
    }


    private void VisualFeedback()
    {
        foreach (var particle in visualFeedback)
        {
            particle.Play();
        }

        foreach (var actuator in actuators)
        {
            var catcher = actuator.GetComponent<LightCatcherReference>().LightCatcher;
            catcher.SetActiveAlphaValue();
        }
    }
    private void AudioFeedback()
    {
        foreach (var player in audioFeedback)
        {
            player.Play();
        }
    }
}
