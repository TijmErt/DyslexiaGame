using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionTracker : MonoBehaviour
{
    public Transform objectiveList;
    public GameObject objectiveText;
    public GameObject markerPrefab;
    private Dictionary<string, GameObject> objectives = new Dictionary<string, GameObject>(); 
    private Dictionary<string, GameObject> markers = new Dictionary<string, GameObject>(); 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddObjective(string objectiveDescription, GameObject target)
    {
        if (objectives.ContainsKey(objectiveDescription)) return;

        GameObject objEntry = Instantiate(objectiveText, objectiveList);

        objEntry.GetComponent<TMP_Text>().text = objectiveDescription;
        objectives[objectiveDescription] = objEntry;

        if (target != null)
        {
            Vector3 spawnPos = target.transform.position + Vector3.up * 4f;
            GameObject marker = Instantiate(markerPrefab, spawnPos, Quaternion.Euler(0, 180, 0));
            marker.transform.SetParent(target.transform);
            markers[objectiveDescription] = marker;
        }
    }

    public void RemoveObjective(string objectiveDescription)
    {
        if (!objectives.ContainsKey(objectiveDescription)) return;

        GameObject objEntry = objectives[objectiveDescription];
        Destroy(objEntry);
        objectives.Remove(objectiveDescription);

        if (markers.TryGetValue(objectiveDescription, out GameObject marker))
        {
            Destroy(marker);
            markers.Remove(objectiveDescription);
        }
    }
}
