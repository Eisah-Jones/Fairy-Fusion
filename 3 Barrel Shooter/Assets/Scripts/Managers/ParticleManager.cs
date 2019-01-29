using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ParticleManager : MonoBehaviour {


    private List<ParticleSystem> particles;


	public ParticleManager()
    {
        particles = new List<ParticleSystem>();
        LoadParticles();
    }


    private void LoadParticles()
    {
        TextAsset txt = (TextAsset)Resources.Load("Particles/loadParticles", typeof(TextAsset));
        string[] lines = Regex.Split(txt.text, "\n|\r|\r\n");

        foreach(string line in lines)
        {
            particles.Add(Resources.Load<ParticleSystem>("Particles/" + line));
        }
    }

    // TODO: Implement if needed
    public void SpawnParticleAtPointByName(Vector3 pos, string name)
    {

    }

    public void SpawnParticleAtPointByID(Vector3 pos, int i)
    {
        ParticleSystem p = Instantiate(particles[i], pos, Quaternion.identity);
        IEnumerator c = HandleParticleSpawn(p);
        StartCoroutine(c);
    }


    public IEnumerator HandleParticleSpawn(ParticleSystem p)
    {
        yield return new WaitForSeconds(p.main.duration);
        Destroy(p);
    }
}
