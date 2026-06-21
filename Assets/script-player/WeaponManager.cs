using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public Button knifeButton;
    public Button pistolButton;
    public Button rifleButton;

    public int currentWeapon = 0;
    // 0 = none
    // 1 = knife
    // 2 = pistol
    // 3 = rifle

    public static WeaponManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SelectWeapon(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectWeapon(1);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectWeapon(2);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SelectWeapon(3);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            SelectWeapon(0);
    }

    public void SelectWeapon(int index)
    {
        currentWeapon = index;

        knifeButton.image.color = Color.white;
        pistolButton.image.color = Color.white;
        rifleButton.image.color = Color.white;

        Color selected = new Color(0.6f, 0.6f, 0.6f);

        if (index == 1)
            knifeButton.image.color = selected;

        if (index == 2)
            pistolButton.image.color = selected;

        if (index == 3)
            rifleButton.image.color = selected;
    }

    public void SelectKnife()
    {
        SelectWeapon(1);
    }

    public void SelectPistol()
    {
        SelectWeapon(2);
    }

    public void SelectRifle()
    {
        SelectWeapon(3);
    }

    public void SelectNoWeapon()
    {
        SelectWeapon(0);
    }
}