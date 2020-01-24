using UnityEngine;

public class Dust : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleDust;
    [SerializeField] private float offsetY = 0;
    
    public void EmitDust()
    {
        ParticleSystem particleSystem = Instantiate(particleDust);

        particleSystem.gameObject.transform.position = new Vector2(
            transform.position.x, transform.position.y + offsetY);
    }
}