using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rb;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;

    private void FixedUpdate()
    {
        float liquidHeight = LiquidManager.instance.GetLiquidHeight(transform.position.x);
        if (transform.position.y < liquidHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((liquidHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rb.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
        }
    }
}
