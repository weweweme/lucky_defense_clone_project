using System;
using System.Collections.Generic;
using DG.Tweening;
using InGame.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// 도박 패널을 관리하는 View 클래스입니다.
    /// </summary>
    public sealed class VW_GamblePanel : View
    {
        [SerializeField] internal Button exitBackgroundPanel;
        [SerializeField] internal Button exitButton;
        [SerializeField] internal UnitGradeToGambleMetaDataDic gambleTryItems;

        [SerializeField] private Canvas _gambleCanvas;
        [SerializeField] private TextMeshProUGUI _baseResultText; // 원본 텍스트 (Prefab 역할)

        /// <summary>
        /// 결과 텍스트 풀
        /// </summary>
        private readonly Queue<TextMeshProUGUI> _resultTextPool = new();
        
        /// <summary>
        /// 현재 활성화된 결과 텍스트 목록 (애니메이션 진행 중)
        /// </summary>
        private readonly List<TextMeshProUGUI> _activeResultTexts = new();

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_GamblePanel), exitBackgroundPanel);
            AssertHelper.NotNull(typeof(VW_GamblePanel), exitButton);
            AssertHelper.NotNull(typeof(VW_GamblePanel), _gambleCanvas);
            AssertHelper.NotNull(typeof(VW_GamblePanel), _baseResultText);
            
            const int TRY_SLOT_COUNT = 3;
            AssertHelper.EqualsValue(typeof(VW_GamblePanel), gambleTryItems.Count, TRY_SLOT_COUNT);

            // 초기 풀 개수 (여기선 3개 미리 생성)
            for (int i = 0; i < 3; i++)
            {
                var newText = CreateResultText();
                _resultTextPool.Enqueue(newText);
            }
        }

        public void SetGambleDescription(IReadOnlyDictionary<EUnitGrade, SGambleMetaData> metaData)
        {
            foreach (var elem in metaData)
            {
                EUnitGrade grade = elem.Key;
                gambleTryItems.TryGetValue(grade, out GambleChoiceItem item);
                item!.chancePercent.SetText($"{elem.Value.SuccessProbability * 100}%");
                item.tryNeedDiamond.SetText(elem.Value.RequiredDia.ToString());
            }
        }

        public void SetGamblePanelVisible(bool value) => _gambleCanvas.enabled = value;

        /// <summary>
        /// 도박 결과를 표시하는 메서드
        /// </summary>
        /// <param name="result">도박 결과 데이터</param>
        public void ShowGambleResult(PR_GamblePanel.SGambleResult result)
        {
            GambleChoiceItem item = gambleTryItems[result.Grade];

            TextMeshProUGUI resultText;
            if (_resultTextPool.Count > 0)
            {
                // 기존 풀에서 가져오기
                resultText = _resultTextPool.Dequeue();
            }
            else
            {
                // 새로운 객체 생성 (부족한 경우)
                resultText = CreateResultText();
            }

            _activeResultTexts.Add(resultText);

            // 성공 여부에 따라 텍스트 설정
            resultText.text = result.IsSuccess ? "Success!" : "Fail!";
            resultText.gameObject.SetActive(true);

            // 부모 위치에 맞춰 배치
            resultText.transform.position = item.transform.position;

            // 위치 초기화
            resultText.transform.localScale = Vector3.one;
            
            // 텍스트 애니메이션 (쉐이크 후 반환)
            resultText.transform
                .DOShakePosition(0.2f, 10f, 10, 90, false, true)
                .OnComplete(() =>
                {
                    resultText.gameObject.SetActive(false);
                    _activeResultTexts.Remove(resultText);
                    _resultTextPool.Enqueue(resultText);
                });
        }

        /// <summary>
        /// 새로운 결과 텍스트를 생성하는 메서드
        /// </summary>
        /// <returns>새로 생성된 TextMeshProUGUI</returns>
        private TextMeshProUGUI CreateResultText()
        {
            TextMeshProUGUI resultText = Instantiate(_baseResultText, _baseResultText.transform.parent);
            resultText.text = "";
            resultText.gameObject.SetActive(false);
            return resultText;
        }
    }
}
