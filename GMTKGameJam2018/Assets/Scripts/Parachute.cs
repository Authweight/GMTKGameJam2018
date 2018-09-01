using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Parachute : MonoBehaviour
    {
        private Rigidbody2D rb;
        private enum ParachuteState
        {
            Dropping,
            FloatingAway
        }

        private ParachuteState state;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            state = ParachuteState.Dropping;
        }

        private void FixedUpdate()
        {
            if (state == ParachuteState.Dropping)
                rb.velocity = new Vector2(0, .5f);
            if (state == ParachuteState.FloatingAway)
                rb.velocity = new Vector2(0, 3f);
        }

        public void CutLoose()
        {
            state = ParachuteState.FloatingAway;
        }
    }
}
