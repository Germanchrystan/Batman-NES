using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
	public bool performFlip(bool facingLeft, int direction)
	{
		float localScaleX = transform.localScale.x; //Obtenemos la escala del personaje en X
		localScaleX = localScaleX * -1f; //Invertimos la escala en X
		transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z); //Se asigna el nuevo valor de X a la escala
        return !facingLeft;
    }
}
