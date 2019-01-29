using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ParticleManager : MonoBehaviour {


    private List<GameObject> particles;


	public ParticleManager()
    {
        particles = new List<GameObject>();
        LoadParticles();
    }


    private void LoadParticles()
    {
        TextAsset txt = (TextAsset)Resources.Load("Particles/loadParticles", typeof(TextAsset));
        string[] lines = Regex.Split(txt.text, "\n|\r|\r\n");

        foreach(string line in lines)
        {
            particles.Add(Resources.Load<GameObject>("Particles/" + line));
        }
    }


    public GameObject GetParticleByID(int i)
    {
        return particles[i-1];
    }


    // TODO: Implement if needed
    public void SpawnParticleAtPointByName(Vector3 pos, string name)
    {

    }

    public void SpawnParticleAtPointByID(Vector3 pos, int i)
    {
        GameObject p = Instantiate(particles[i], pos, Quaternion.identity);
        IEnumerator c = HandleParticleSpawn(p);
        StartCoroutine(c);
    }


    public IEnumerator HandleParticleSpawn(GameObject p)
    {
        yield return new WaitForSeconds(10);
        Destroy(p);
    }
}
