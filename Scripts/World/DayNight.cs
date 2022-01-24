﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    public MenuManager menuManager;

    public Gradient DayGradient;
    public Gradient NightGradient;

    public float audioMultiplier;
    public AnimationCurve nightCurve;
    public AnimationCurve dayCurve;
    public AnimationCurve audioDampen;

    public SpriteRenderer[] daySprites;
    public SpriteRenderer[] nightSprites;

    public AudioSource dayMusic;
    public AudioSource nightMusic;

    [SerializeField] private AudioRandomizer dayThemeRandomizer;
    [SerializeField] private AudioRandomizer nightThemeRandomizer;

    public AudioSource dayDeathJingle;
    public AudioSource nightDeathJingle;

    public Transform CelestialBodies;

    public float time;

    public float daySpeed = 1;
    public float nightSpeed = 1;

    public float cycleTime = 180;

    public bool isNotDay;

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(.01f);
        dayMusic = dayThemeRandomizer.audioSource;
        nightMusic = nightThemeRandomizer.audioSource;
    }

    // Update is called once per frame
    void Update()
    {
        isNotDay = time > cycleTime / 4 * 1 && time < cycleTime / 4 * 3;

        if(time >= cycleTime)
        {
            time = 0;
        }

        if (!isNotDay)
        {
            time += Time.deltaTime * daySpeed;
            nightThemeRandomizer.RandomizeClips();
            nightMusic = nightThemeRandomizer.audioSource;
        }
        else
        {
            time += Time.deltaTime * nightSpeed;
            dayThemeRandomizer.RandomizeClips();
            dayMusic = dayThemeRandomizer.audioSource;
        }

        for (int i = 0; i < daySprites.Length; i++)
        {
            daySprites[i].color = DayGradient.Evaluate(time / cycleTime);
        }

        for (int i = 0; i < nightSprites.Length; i++)
        {
            nightSprites[i].color = NightGradient.Evaluate(time / cycleTime);
        }

        MusicAdjustment(menuManager.jinglePlaying);

        CelestialBodies.rotation = Quaternion.Euler(0, 0, -time / cycleTime * 360);
    }

    public void MusicAdjustment(bool deathJingle)
    {
        if(!deathJingle)
        {
            dayMusic.volume = nightCurve.Evaluate(time / cycleTime) * audioMultiplier;
            nightMusic.volume = dayCurve.Evaluate(time / cycleTime) * audioMultiplier;
            dayDeathJingle.volume = nightCurve.Evaluate(time / cycleTime) * audioMultiplier;
            nightDeathJingle.volume = dayCurve.Evaluate(time / cycleTime) * audioMultiplier;
        }
        else
        {
            if(!isNotDay)
            {
                dayMusic.volume = 0;
            }
            else
            {
                nightMusic.volume = 0;
            }
        }
    }
}
