using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // 데미지를 받을 수 있는 오브젝트를 위한 인터페이스
    void OnDamage(Vector2 targetPos);
}
