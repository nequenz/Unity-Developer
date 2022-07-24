using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( AudioAlarm ) )]
[RequireComponent( typeof( Collider ) )]
public class TriggerZone : MonoBehaviour
{
    private AudioAlarm _zoneAudioAlarm;

    private void Awake()
    {
        _zoneAudioAlarm = GetComponent<AudioAlarm>();
    }

    private void OnTriggerEnter( Collider other )
    {
        if( other.TryGetComponent(out Rogue rogueBehavior) == true )
        {
            rogueBehavior.OnEnterAntiRogueZone();
            _zoneAudioAlarm.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ( other.TryGetComponent(out Rogue rogueBehavior) == true )
        {
            rogueBehavior.OnLeaveAntiRogueZone();
            _zoneAudioAlarm.Finish();
        }
    }
}