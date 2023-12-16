using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LJH{
    public class EffectManager : MonoBehaviour
    {
        static public EffectManager Instance;
        [SerializeField] Transform EffectGroup;
        [SerializeField]List<ParticleSystem> particles= new List<ParticleSystem>();
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        private void Start() {
            for(int i = 0;i <particles.Count;i ++){
                Instantiate(particles[i].gameObject, Vector3.zero, Quaternion.identity, EffectGroup.transform);
            }
        }
        public void PlayGrapEffect(){
            //play effect Enum Switch
            
            
        }
        

    }
}
