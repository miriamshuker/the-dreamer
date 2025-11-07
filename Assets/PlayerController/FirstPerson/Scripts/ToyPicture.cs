using UnityEngine;


public class ToyPicture : MonoBehaviour
{
    public enum ToggleMode { AnimatedVsStatic, TwoSprites }

    [Header("Mode")]
    public ToggleMode mode = ToggleMode.AnimatedVsStatic;

    [Header("Common Target")]
    [Tooltip("")]
    public SpriteRenderer targetSpriteRenderer;

    [Header("AnimatedVsStatic")]
    [Tooltip("")]
    public Animator animator;
    [Tooltip("")]
    public Sprite staticSprite;
    [Tooltip("")]
    public string animationStateName;
    [Tooltip("")]
    public bool playBgmInAnimated = true;

    [Header("TwoSprites")]
    [Tooltip("A")]
    public Sprite spriteA;
    [Tooltip("B")]
    public Sprite spriteB;
    [Tooltip("")]
    public bool playBgmInStateA = true;

    [Header("Audio")]
    [Tooltip("")]
    public AudioSource bgm;

    [Header("Lights")]
    [Tooltip("")]
    public Transform lightsParent;
    [Tooltip("")]
    public bool lightsOnInPrimaryState = true;
    [Tooltip("")]
    public bool alsoToggleChildGameObjects = false;

    [Header("Initial & Debounce")]
    [Tooltip("")]
    public bool startInPrimaryState = true;
    public float toggleCooldown = 0.15f;

    [Header("State (ReadOnly)")]
    public bool isPrimaryState;  
    private float _lastToggleTime = -999f;

    private void Awake()
    {
        if (targetSpriteRenderer == null)
            targetSpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (mode == ToggleMode.AnimatedVsStatic && animator == null && targetSpriteRenderer != null)
            animator = targetSpriteRenderer.GetComponent<Animator>();

        isPrimaryState = startInPrimaryState;
        ApplyState(initial: true);
    }


    public void Toggle()
    {
        if (Time.time - _lastToggleTime < toggleCooldown) return;
        _lastToggleTime = Time.time;

        isPrimaryState = !isPrimaryState;
        ApplyState();
    }

    private void ApplyState(bool initial = false)
    {
        switch (mode)
        {
            case ToggleMode.AnimatedVsStatic:
                ApplyAnimatedVsStatic();
                break;
            case ToggleMode.TwoSprites:
                ApplyTwoSprites();
                break;
        }


        bool turnLightsOn = lightsOnInPrimaryState ? isPrimaryState : !isPrimaryState;
        SetLights(turnLightsOn);
    }

    private void ApplyAnimatedVsStatic()
    {

        if (isPrimaryState)
        {
            if (animator != null)
            {
                animator.enabled = true;
                if (!string.IsNullOrEmpty(animationStateName))
                    animator.Play(animationStateName, 0, 0f);
            }

            if (bgm != null)
            {
                if (playBgmInAnimated)
                {
                    if (!bgm.isPlaying) bgm.Play();
                }
                else
                {
                    if (bgm.isPlaying) bgm.Pause();
                }
            }
        }
        else
        {

            if (animator != null) animator.enabled = false;
            if (targetSpriteRenderer != null && staticSprite != null)
                targetSpriteRenderer.sprite = staticSprite;

            if (bgm != null && playBgmInAnimated)
            {
                if (bgm.isPlaying) bgm.Pause();
            }
        }
    }

    private void ApplyTwoSprites()
    {

        if (targetSpriteRenderer != null)
        {
            if (isPrimaryState && spriteA != null) targetSpriteRenderer.sprite = spriteA;
            else if (!isPrimaryState && spriteB != null) targetSpriteRenderer.sprite = spriteB;
        }


        if (animator != null) animator.enabled = false;


        if (bgm != null)
        {
            bool shouldPlay = isPrimaryState ? playBgmInStateA : !playBgmInStateA;
            if (shouldPlay)
            {
                if (!bgm.isPlaying) bgm.Play();
            }
            else
            {
                if (bgm.isPlaying) bgm.Pause();
            }
        }
    }

    private void SetLights(bool on)
    {
        if (lightsParent == null) return;


        var lights = lightsParent.GetComponentsInChildren<Light>(true);
        foreach (var l in lights)
        {
            l.enabled = on;
        }


        if (alsoToggleChildGameObjects)
        {
            for (int i = 0; i < lightsParent.childCount; i++)
            {
                lightsParent.GetChild(i).gameObject.SetActive(on);
            }
        }
    }
}