using UnityEngine;

public class SpookPlayer : MonoBehaviour
{
    [SerializeField] SoundPlayer soundPlayer;
    [SerializeField] float spookTime;
    [SerializeField] float randomIncrement;

    float timer;
    float currentSpookChance;

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > spookTime)
        {
            timer = 0;
            currentSpookChance += randomIncrement;

            if(Random.Range(0f,1f) < currentSpookChance)
            {
                currentSpookChance = 0f;
                soundPlayer.Play();
            }
        }
    }
}
