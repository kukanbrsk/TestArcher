using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform origin;
    [SerializeField] private float timeStep = 0.05f;
    [SerializeField] private int maxDots = 15;
    [SerializeField] private float minDotScale = 0.5f;
    private readonly List<GameObject> dots = new();

    public void InitializeDots()
    {
        for (int i = 0; i < maxDots; i++)
        {
            var dot = Instantiate(dotPrefab, transform);
            var progress = i / (float)maxDots;
            var scale = Mathf.Lerp(1f, minDotScale, progress);
            dot.transform.localScale = Vector3.one * scale;
            dot.SetActive(false);
            dots.Add(dot);
        }
    }

    public void UpdateTrajectory(Vector3 velocity)
    {
        Vector3 gravity = Physics2D.gravity;

        for (int i = 0; i < dots.Count; i++)
        {
            var time = i * timeStep;

            dots[i].transform.position = origin.position + velocity * time + gravity * (0.5f * time * time);
        }
    }

    public void Show()
    {
        foreach (var dot in dots)
        {
            dot.SetActive(true);
        }
    }

    public void Hide()
    {
        foreach (var dot in dots)
        {
            dot.SetActive(false);
        }
    }
}