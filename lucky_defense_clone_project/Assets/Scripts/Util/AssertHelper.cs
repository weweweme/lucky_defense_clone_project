using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Util
{
    /// <summary>
    /// 유니티 객체의 null 체크를 도와주는 헬퍼 클래스입니다.
    /// 
    /// Unity에서 `null`이거나 `Destroy()`된 오브젝트를 감지하여 경고를 출력합니다.
    /// 특정 필드가 에디터에서 정상적으로 할당되었는지 검증하는 용도로 사용됩니다.
    /// `#if UNITY_EDITOR`를 사용하여 런타임 빌드에는 영향을 주지 않습니다.
    /// </summary>
    public static class AssertHelper
    {
        /// <summary>
        /// 주어진 필드가 null인지 검사하고, null이면 Debug.Assert 메시지를 출력합니다.
        /// (이 코드는 **유니티 에디터에서만 실행됩니다.**)
        ///
        /// 특이 사항:  
        /// Unity에서는 `Destroy()`된 `GameObject`가 `!= null` 비교 시 `false`를 반환하는 특수한 동작을 합니다.
        /// 따라서 `UnityEngine.Object`를 따로 체크하여 Destroy된 오브젝트를 감지합니다.
        /// </summary>
        /// <typeparam name="T">참조 타입(Reference Type)만 허용</typeparam>
        /// <param name="ownerType">이 필드를 가지고 있는 클래스의 Type (예: typeof(MyClass))</param>
        /// <param name="field">검사할 필드 값</param>
        /// <param name="fieldName">필드 이름</param>
        public static void NotNull<T>(System.Type ownerType, T field) where T : class
        {
#if UNITY_EDITOR
            // UnityEngine.Object 기반의 필드라면 Destroy된 상태인지 추가 체크
            if (field is Object unityObject && unityObject == null)
            {
                Debug.Assert(false, $"[{ownerType.Name}] {nameof(field)} 게임 오브젝트가 Destroy된 상태입니다.");
                return;
            }

            // 일반적인 null 체크
            Debug.Assert(field != null, $"[{ownerType.Name}] {nameof(field)} 게임 오브젝트가 할당되지 않았습니다.");
#endif
        }
        
        /// <summary>
        /// 지정된 열거형 필드가 특정 잘못된 값과 일치하지 않는지 검증합니다.
        /// 
        /// 주로 필드가 초기화되지 않았거나, 유효하지 않은 기본값(None 등)을 가지고 있을 때
        /// 디버그 환경에서 경고를 출력하여 빠르게 문제를 감지하는 용도로 사용됩니다.
        /// 
        /// (이 메서드는 유니티 에디터 환경에서만 동작합니다)
        /// </summary>
        /// <typeparam name="T">검사할 열거형 타입</typeparam>
        /// <param name="ownerType">검사를 수행하는 클래스 타입 (예: typeof(MyClass))</param>
        /// <param name="field">검사할 열거형 필드</param>
        /// <param name="invalidValue">유효하지 않은 값으로 간주할 열거형 값 (예: None, Invalid 등)</param>
        public static void NotEqualsEnum<T>(System.Type ownerType, T field, T invalidValue) where T : Enum
        {
#if UNITY_EDITOR
            Debug.Assert(!EqualityComparer<T>.Default.Equals(field, invalidValue), $"[{ownerType.Name}] {nameof(field)} 필드가 잘못된 값({invalidValue})입니다.");
#endif
        }
        
        /// <summary>
        /// 값 타입 필드가 지정된 잘못된 값과 일치하지 않는지 검증합니다.
        /// 
        /// 주로 숫자형 값(int, float 등)이나 구조체 타입에서 초기화되지 않았거나
        /// 유효하지 않은 기본값과 비교해, 디버그 환경에서 경고를 출력하는 용도로 사용됩니다.
        /// 
        /// 이 메서드는 유니티 에디터 환경에서만 동작합니다.
        /// </summary>
        /// <typeparam name="T">검사할 값 타입 (int, float 등)</typeparam>
        /// <param name="ownerType">검사를 수행하는 클래스 타입 (예: typeof(MyClass))</param>
        /// <param name="field">검사할 값 타입 필드</param>
        /// <param name="invalidValue">유효하지 않은 값으로 간주할 값 (예: -1, 0 등)</param>
        public static void NotEqualsValue<T>(System.Type ownerType, T field, T invalidValue) where T : struct, IComparable<T>
        {
#if UNITY_EDITOR
            if (EqualityComparer<T>.Default.Equals(field, invalidValue))
            {
                Debug.Assert(false, $"[{ownerType.Name}] 필드가 잘못된 값({invalidValue})입니다.");
            }
#endif
        }
        
        /// <summary>
        /// 값 타입 필드가 지정된 값과 일치하는지 검증합니다.
        /// 
        /// 주로 숫자형 값(int, float 등)이나 구조체 타입에서 초기화되지 않았거나
        /// 유효하지 않은 기본값과 비교해, 디버그 환경에서 경고를 출력하는 용도로 사용됩니다.
        /// 
        /// 이 메서드는 유니티 에디터 환경에서만 동작합니다.
        /// </summary>
        /// <typeparam name="T">검사할 값 타입 (int, float 등)</typeparam>
        /// <param name="ownerType">검사를 수행하는 클래스 타입 (예: typeof(MyClass))</param>
        /// <param name="field">검사할 값 타입 필드</param>
        /// <param name="invalidValue">유효하지 않은 값으로 간주할 값 (예: -1, 0 등)</param>
        public static void EqualsValue<T>(System.Type ownerType, T field, T value) where T : struct, IComparable<T>
        {
#if UNITY_EDITOR
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                Debug.Assert(false, $"[{ownerType.Name}] 필드가 잘못된 값({value})입니다.");
            }
#endif
        }
    }
}
