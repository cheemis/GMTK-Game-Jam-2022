using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DiceFaceDetection : MonoBehaviour
{
    public TMP_Text UIDiceTotalText = default;
    public static Dictionary<string, int> DiceValues = new Dictionary<string, int>();
    public static Dictionary<string, bool> DiceValuesSet = new Dictionary<string, bool>();
    [HideInInspector] public string MyIndex = default;
    private Transform _transform = default;
    private DiceCharacter _diceCharacter = default;
    private bool _started = false;
    private bool _diceUpdated = false;
    public int UpFace
    {
        get
        {
            float[] directions = new float[6]
            {
                Vector3.Dot( _transform.up, Vector3.up),
                Vector3.Dot(-_transform.forward, Vector3.up),
                Vector3.Dot(-_transform.right, Vector3.up),
                Vector3.Dot( _transform.right, Vector3.up),
                Vector3.Dot( _transform.forward, Vector3.up),
                Vector3.Dot(-_transform.up, Vector3.up),
            };

            int indexOfUpFace = -1;
            float compare = -1;
            for (int i = 0; i < directions.Length; i++)
            {
                if (compare < directions[i])
                {
                    compare = directions[i];
                    indexOfUpFace = i;
                }
            }

            return indexOfUpFace + 1;
        }
    }
    [SerializeField] private int currentDieValue = 0;
    private void Start()
    {
        _transform = transform;
        _diceCharacter = GetComponent<DiceCharacter>();
        _started = true;
        DiceValuesSet[MyIndex] = false;

    }
    private void OnTriggerStay(Collider other)
    {
        if (this.enabled && this._started)
        {
            currentDieValue = UpFace;

            if (_diceCharacter.ragdolling)
            {
                DiceValues[MyIndex] = UpFace;
                UpdateText(UIDiceTotalText);

                _diceUpdated = true;
            }
            else if (_diceUpdated)
            {
                DiceValuesSet[MyIndex] = true;
                Debug.Log("Update");
            }

        }

    }

    private static void UpdateText(TMP_Text t)
    {
        if (t) t.text = DiceValues.Values.Sum().ToString();
    }
}
