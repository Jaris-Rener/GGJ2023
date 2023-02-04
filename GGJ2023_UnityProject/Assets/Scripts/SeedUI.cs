namespace LemonBerry
{
    using System;
    using TMPro;
    using UnityEngine;

    public class SeedUI : MonoBehaviour
    {
        [SerializeField] private Seed _seed;
        [SerializeField] private TextMeshProUGUI _label;

        private void Start()
        {
            _seed.OnAddWater += UpdateUI;
            _seed.OnRemoveWater += UpdateUI;
        }

        private void OnDestroy()
        {
            _seed.OnAddWater -= UpdateUI;
            _seed.OnRemoveWater -= UpdateUI;
        }

        private void UpdateUI(Seed obj)
        {
            _label.text = $"{obj.Water}/{obj.GrowCost}";
        }
    }

}