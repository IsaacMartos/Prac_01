using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] AudioClips;

    private AudioSource ControlAudio;

	private void Awake()
	{
		ControlAudio = GetComponent<AudioSource>();
	}

	public void SeleccionAudio(int indice, float volumen)
	{
		ControlAudio.PlayOneShot(AudioClips[indice], volumen);
	}
}
