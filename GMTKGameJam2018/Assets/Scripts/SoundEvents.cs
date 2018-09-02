using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class SoundEvents
    {
        private static List<AudioSource> audioSources;

        public static void AddSources(AudioSource[] sources)
        {
            if (audioSources == null)
                audioSources = new List<AudioSource>();
            if (audioSources.Any(x => x == null))
                audioSources = audioSources.Where(x => x != null).ToList();

            audioSources.AddRange(sources);
        }

        public static void Play(string sound)
        {
            audioSources.First(x => x.gameObject.name == sound).Play();
        }
    }
}
