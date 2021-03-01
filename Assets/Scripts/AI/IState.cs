using UnityEngine;
namespace AI
{
    public interface IState
    {
        void Execute (Transform destination, CreatureAI creature);
    }
}