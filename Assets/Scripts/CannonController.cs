using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System; //reconhecer os tipos fornecidos pelo textmeshpro.

public class CannonController : MonoBehaviour
{
    //O InputActionReference é o tipo da variável, indica que a variável vai guardar uma referência da ação do input system.
    [SerializeField] private InputActionReference chargeAction;
    [SerializeField] private InputActionReference aimUp;
    [SerializeField] private InputActionReference aimDown;
    [SerializeField] private float aimSpeed = 20f;

    [SerializeField] private float chargeSpeed = 1f;
    private float power = 0f;
    [SerializeField] private float maxPower = 50f;
    [SerializeField] private float maxAngle = 90;

    //Lançamento
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shotAngle = 1f;
    [SerializeField] private TMP_Text angleText;


    //OnEnable e OnDisable são nomes especiais, o próprio unity identifica automaticamente.
    //(por hedar MonoBehaviour ele consegue entender)
    private void OnEnable(){
        chargeAction.action.Enable(); //isso aqui é basicamente pra "ligar" a ação em si (o Z que configuramos lá)
        aimUp.action.Enable();
        aimDown.action.Enable();
    }

     private void OnDisable(){
        chargeAction.action.Disable();
        aimUp.action.Disable();
        aimDown.action.Disable();
    }

    private void Update()
    {
        //carregar tiro
        if (chargeAction.action.WasPressedThisFrame()) //pressionado
        {
            Debug.Log("Z foi pressionado!");
        }

        if (chargeAction.action.IsPressed()) //segurando
        {
            power = Mathf.Clamp(power + (chargeSpeed * Time.deltaTime), 0, maxPower); 

        }

        if (chargeAction.action.WasReleasedThisFrame()) //Quando solta
        {
            Debug.Log($"Potência acumulada: {power:F2}");
            FireProjectile(power);

            power = 0f; //resetando a força
        }

        //alterar ângulo de mira para cima
        if (aimUp.action.IsPressed())
        {
            shotAngle = Mathf.Clamp(shotAngle + aimSpeed * Time.deltaTime, 1f, maxAngle);
            angleText.text = $"{Mathf.RoundToInt(shotAngle)}°";
        }
        //alterar ângulo de mira para baixo
        if (aimDown.action.IsPressed())
        {
            shotAngle = Mathf.Clamp(shotAngle - aimSpeed * Time.deltaTime, 1f, maxAngle);
            angleText.text = $"{Mathf.RoundToInt(shotAngle)}°";
        }
    }
    
    private void FireProjectile(float power)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity); //Aqui nós criamos uma cópia do prefab na cena
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        //direção
        float cos = Mathf.Cos(shotAngle * Mathf.Deg2Rad);
        float sin = Mathf.Sin(shotAngle * Mathf.Deg2Rad);

        Vector2 direction = new Vector2(cos, sin);

        rb.AddForce(direction * power, ForceMode2D.Impulse);
    }
}
