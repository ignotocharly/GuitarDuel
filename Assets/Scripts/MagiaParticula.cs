using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagiaParticula : MonoBehaviour
{
    //GameObject particulaGO;
    public Material materialParticula;



    public void SetParticulaColor( Color color )
    {

        materialParticula.color = color;
    }



}
