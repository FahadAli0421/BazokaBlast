using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCannonScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> cannon;
    [SerializeField] private int cannonIndex;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("SelectedCannonIndex"))
        {
            cannonIndex = PlayerPrefs.GetInt("SelectedCannonIndex");
            SpawnCannon(cannonIndex);
        }
        else
        {
            cannonIndex = 0;
            SpawnCannon(cannonIndex);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnCannon(int index)
    {
        for (int i = 0; i < cannon.Count; i++)
        {
            cannon[i].SetActive(false);
        }
        cannon[index].SetActive(true);
    }
}
