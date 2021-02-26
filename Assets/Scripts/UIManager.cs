using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberOfChickenText;
    [SerializeField] private TextMeshProUGUI numberOfFoxText;
    private int numberOfFoxes;
    private int numberOfChickens;
    private void Update ()
    {
        UpdateStats ();
    }

    private void UpdateStats ()
    {
        //bunu kullanmıycaz
        //üreme stateleri falan yazıldıktan sonra eventler ile güncelliycez ui'ı
        //çünkü böyle yapmak performans açısından çok kötü
        //şimdilik sayıları görelim diye yaptım
        var foxes = GameObject.FindGameObjectsWithTag ("Fox");
        numberOfFoxes = foxes.Length;

        var chicken = GameObject.FindGameObjectsWithTag ("Chicken");
        numberOfChickens = chicken.Length;

        numberOfChickenText.text = "Chicken: " + numberOfChickens;
        numberOfFoxText.text = "Fox: " + numberOfFoxes;
    }
}