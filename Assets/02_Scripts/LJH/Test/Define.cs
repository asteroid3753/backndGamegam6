using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LJH{
    
    public class Define : MonoBehaviour
    {   
        public enum ItemType{
            asdf,
            asdfd,
            qwer,
            qwetwqt
        };

        public ItemType type {
            get{
                return type;
            }
            
            set{
                //서버에 올릴 타입, 스프라이트 등.
            }
        
        }

        
    }

}
