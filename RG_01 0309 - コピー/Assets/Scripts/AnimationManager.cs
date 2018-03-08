using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]　protected Animator _animator;      // アニメーター

    // アニメ―ションの再生
    protected void PlayAnimation(string tag)
    {
        _animator.SetTrigger(tag);
    }

    // 指定されたアニメ―ションが再生状態かどうか
    protected bool IsPlayingAnimation(string anim)
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(anim)) return true;
        return false;
    }


    protected bool IsEndAnimation()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime > 1.0f) return true;
        return false;
    }
}
