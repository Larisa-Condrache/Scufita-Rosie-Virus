using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public Button knifeButton;
    public Button pistolButton;
    public Button rifleButton;

    private int currentWeapon = 3;

    void Start()
    {
        SelectWeapon(3);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectWeapon(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectWeapon(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SelectWeapon(2);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            SelectWeapon(3);
    }

    void SelectWeapon(int index)
    {
        currentWeapon = index;

        knifeButton.image.color = Color.white;
        pistolButton.image.color = Color.white;
        rifleButton.image.color = Color.white;

        Color selected = new Color(0.6f, 0.6f, 0.6f);

        if (index == 0)
            knifeButton.image.color = selected;

        if (index == 1)
            pistolButton.image.color = selected;

        if (index == 2)
            rifleButton.image.color = selected;
    }
}