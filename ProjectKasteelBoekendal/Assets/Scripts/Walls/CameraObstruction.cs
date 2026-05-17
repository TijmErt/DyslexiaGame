using System.Collections.Generic;
using UnityEngine;

public class CameraObstruction : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private Material transparentMaterial;

    private readonly List<Renderer> currentHits = new();
    private readonly Dictionary<Renderer, Material[]> originalMaterials = new();

    private void LateUpdate()
    {
        ClearOld();

        Vector3 dir = target.position - transform.position;
        float dist = dir.magnitude;

        RaycastHit[] hits = Physics.RaycastAll(
            transform.position,
            dir.normalized,
            dist,
            obstructionMask
        );

        foreach (var hit in hits)
        {
            Renderer r = hit.collider.GetComponent<Renderer>();
            if (r == null) continue;

            if (!originalMaterials.ContainsKey(r))
                originalMaterials[r] = r.materials;

            Material[] temp = new Material[r.materials.Length];
            for (int i = 0; i < temp.Length; i++)
                temp[i] = transparentMaterial;

            r.materials = temp;
            currentHits.Add(r);
        }
    }

    private void ClearOld()
    {
        foreach (var r in currentHits)
        {
            if (r == null) continue;

            if (originalMaterials.ContainsKey(r))
                r.materials = originalMaterials[r];
        }

        currentHits.Clear();
    }
}