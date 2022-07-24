using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof ( AudioSource ) )]
[RequireComponent( typeof( Collider ) )]
public class TriggerZone : MonoBehaviour
{
    private const float VolumePerStep = 0.01f;

    private AudioSource _audioSource;
    private Coroutine _signalCoroutine;
    private float _volumeTarget = 1f;

    private void Awake()
    {
        const float StartVolume = 0.01f;

        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = StartVolume;
    }

    private void OnTriggerEnter( Collider other )
    {
        if( other.TryGetComponent(out RogueBehavior rogueBehavior) == true )
        {
            rogueBehavior.OnEnterAntiRogueZone();

            SignalWithSmoothVolume(1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ( other.TryGetComponent(out RogueBehavior rogueBehavior) == true )
        {
            rogueBehavior.OnLeaveAntiRogueZone();

            SignalWithSmoothVolume(-1f);
        }
    }

    private void SignalWithSmoothVolume(float volumeTarget)
    {
        if( _signalCoroutine != null)
        {
            StopCoroutine(_signalCoroutine);
        }

        _volumeTarget = Mathf.Clamp(volumeTarget, 0f, 1f);

        _signalCoroutine = StartCoroutine( TuneSignalVolumeCoroutine() );  
    }

    private IEnumerator TuneSignalVolumeCoroutine()
    {
        const float WaitSeconds = 0.5f;

        while( _audioSource.volume > 0f && _audioSource.volume < 1f )
        {
            if( _audioSource.isPlaying == false )
            {
                _audioSource.Play();
            }

            _audioSource.volume = Mathf.MoveTowards( _audioSource.volume, _volumeTarget, VolumePerStep );

            yield return new WaitForSeconds( WaitSeconds );
        }
    }
}
