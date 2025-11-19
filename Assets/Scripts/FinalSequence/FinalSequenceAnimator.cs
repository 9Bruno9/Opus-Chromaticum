using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSequenceAnimator : MonoBehaviour
{
    [Header("Triggering")]
    [SerializeField] private List<LightCatcher> actuators;
    private List<ParticleSystem> _visualFeedback;
    private List<AudioSource> _audioFeedback;

    [Header("Scene audio manipulation")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private float musicMuffleTime;
    
    [Header("Puzzle removal")]
    [SerializeField] private PuzzleManager puzzleManager;
    [SerializeField] private Transform puzzlePieces;
    [SerializeField] private float puzzleVanishTime;
    
    [Header("Assets to animate")]
    [SerializeField] private Transform spinningContainer;
    [SerializeField] private List<Transform> colorSpheres;
    [SerializeField] private Transform alchemicalRedSphere;
    [SerializeField] private AudioSource materialSpawnSoundSource;
    [SerializeField] private AudioSource windSoundSource;
    [SerializeField] private AudioSource redSpawnSoundSource;
    
    [Header("Animation timings")]
    [SerializeField] private float timeToStart = 1f;
    [SerializeField] private float timeBetweenSpawn = 1f;
    [SerializeField] private float timeBeforeMerging = 1f;
    [SerializeField] private float mergingSpeed = .01f;
    
    private bool _spinning;
    private float _spinningSpeed;
    private bool _spinUpCompleted;
    private bool[] _locks;
    
    private void Start()
    {
        _locks = new[] { false, false, false };
        
        _visualFeedback = new List<ParticleSystem>();
        _audioFeedback = new List<AudioSource>();
        foreach (var actuator in actuators)
        {
            _visualFeedback.Add(actuator.transform.parent.parent.GetComponentInChildren<ParticleSystem>());
            _audioFeedback.Add(actuator.transform.parent.parent.GetComponentInChildren<AudioSource>());
        }
        
        foreach (var colorSphere in colorSpheres)
        {
            colorSphere.gameObject.SetActive(false);
        }
        alchemicalRedSphere.gameObject.SetActive(false);
    }

    public void ResetLocks()
    {
        _locks[0] = false;
        _locks[1] = false;
        _locks[2] = false;
    }
    
    public void AttemptSequenceStart(int lockId)
    {
        if (lockId is > 2 or < 0) return;
        
        _locks[lockId] = true;
        var start = true;
        foreach (var startLock in _locks)
        {
            start &= startLock;
        }

        if (!start) return;
        foreach (var lightCatcher in actuators)
        {
            lightCatcher.enabled = false;   
        }



        foreach (var source in _audioFeedback)
        {
            source.Play();
        }
        foreach (var source in _visualFeedback)
        {
            source.Play();
        }
        foreach (var actuator in actuators)
        {
            actuator.SetActiveAlphaValue();
        }
        StartFinalSequence();
        
    }

    [ContextMenu("Start!")]
    public void StartFinalSequence()
    {
        StartCoroutine(RemovePuzzlePieces());
        StartCoroutine(AnimateMusic());
        StartCoroutine(FinalSequenceCoroutine());
    }

    private IEnumerator RemovePuzzlePieces()
    {
        puzzleManager.DisableAllLightCasters();
        var newPos = puzzlePieces.localPosition;
        var decrement = 6 / (puzzleVanishTime / Time.fixedDeltaTime);
        while (newPos.y > -6)
        {
            newPos.y -= decrement;
            puzzlePieces.localPosition = newPos;
            //puzzleManager.UpdateAllLightCasters();
            yield return new WaitForFixedUpdate();
        }
    }
    
    private IEnumerator AnimateMusic()
    {
        var musicNewVol = musicSource.volume;
        var ambientNewVol = ambientSource.volume;
        var decrement = .7f / (musicMuffleTime / Time.fixedDeltaTime);
        while (musicNewVol > .3f)
        {
            
            musicNewVol = Mathf.Max(musicNewVol - decrement, .3f);
            ambientNewVol = Mathf.Max(ambientNewVol - decrement, .3f);
            musicSource.volume = musicNewVol;
            ambientSource.volume = ambientNewVol;

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator FinalSequenceCoroutine()
    {
        yield return new WaitForSecondsRealtime(timeToStart);
        foreach (var colorSphere in colorSpheres)
        {
            colorSphere.gameObject.SetActive(true);
            materialSpawnSoundSource.Play(0);
            yield return new WaitForSecondsRealtime(timeBetweenSpawn);
            materialSpawnSoundSource.pitch += 0.05f;
        }

        _spinning = true;
        _spinningSpeed = 0.001f;
        var sc = StartCoroutine(SpinningCoroutine());

        //Wait for spin-up
        while (!_spinUpCompleted)
        {
            yield return new WaitForFixedUpdate();
        }
        
        yield return new WaitForSecondsRealtime(timeBeforeMerging);

        var newPosition = colorSpheres[0].localPosition.z;
        var positionSet = new Vector3(0, 0, newPosition);
        while (newPosition > 0)
        {
            foreach (var colorSphere in colorSpheres)
            {
                colorSphere.SetLocalPositionAndRotation(positionSet, Quaternion.identity);
            }
            newPosition = Mathf.Max(newPosition-mergingSpeed, 0f);
            positionSet.z = newPosition;
            yield return new WaitForFixedUpdate();
        }
        musicSource.volume = 0;
        ambientSource.volume = 0;
        
        _spinning = false;
        redSpawnSoundSource.Play(0);
        alchemicalRedSphere.gameObject.SetActive(true);
        spinningContainer.gameObject.SetActive(false);
        
        newPosition = alchemicalRedSphere.localPosition.z;
        positionSet = new Vector3(0, newPosition, 0);
        while (newPosition > -.75f)
        {
            alchemicalRedSphere.SetLocalPositionAndRotation(positionSet, Quaternion.identity);
            newPosition = Mathf.Max(newPosition-0.01f, -.75f);
            positionSet.y = newPosition;
            yield return new WaitForFixedUpdate();
        }
        
        yield return null;
    }

    private IEnumerator SpinningCoroutine()
    {
        windSoundSource.pitch = 0;
        windSoundSource.volume = 0;
        windSoundSource.Play(0);
        while (_spinning)
        {
            spinningContainer.Rotate(Vector3.up, _spinningSpeed);
            if (_spinningSpeed < 30)
            {
                _spinningSpeed *= 1.02f;
            }
            else
            {
                _spinUpCompleted = true;
            }
            yield return new WaitForFixedUpdate();
            windSoundSource.pitch = Remap(_spinningSpeed, 0.01f, 30f, 0f, 7f);
            windSoundSource.volume = Remap(_spinningSpeed, 0.01f, 30f, 0f, 1f);
        }
        windSoundSource.Stop();
    }

    private static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (value - fromMin) * (toMax - toMin) / (fromMax - fromMin) + toMin;
    }
}
