using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CannonController : MonoBehaviour
{
    //O InputActionReference é o tipo da variável, indica que a variável vai guardar uma referência da ação do input system.
    [SerializeField] private InputActionReference chargeAction;
    [SerializeField] private InputActionReference aimUp;
    [SerializeField] private InputActionReference aimDown;


    [SerializeField] private float aimSpeed = 30f;
    [SerializeField] private float chargeTime = 6.5f;
    private float power = 0f;
    [SerializeField] private float maxPower = 100f;
    [SerializeField] private float maxAngle = 90;

    //Lançamento
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shotAngle = 1f;

    //Mira visual
    [SerializeField] private TMP_Text angleText;
    [SerializeField] private LineRenderer aimLine;
    //Delay entre segurar/pressionar rápido
    private float holdDelay = 0.3f;
    private float aimHoldTime = 0f;

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
        HandleAim(); //Alterar o ângulo da mira
        UpdateAimLine();//Update da mira
        ChargeCannonShot();
    }
    

    private void HandleAim()
    {
        //alterar ângulo de mira para cima
        if (aimUp.action.WasPressedThisFrame())
        {
            aimHoldTime = 0f;
            shotAngle = Mathf.Clamp(shotAngle + 1, 1f, maxAngle);
            angleText.text = $"{Mathf.RoundToInt(shotAngle)}°";
        }
        else if (aimUp.action.IsPressed())
        {
            aimHoldTime += Time.deltaTime;

            if (aimHoldTime >= holdDelay) // só gira contínuo depois do delay
            {
                shotAngle = Mathf.Clamp(shotAngle + aimSpeed * Time.deltaTime, 1f, maxAngle);
                angleText.text = $"{Mathf.RoundToInt(shotAngle)}°";
            }
        }


        //alterar ângulo de mira para baixo
        if (aimDown.action.WasPressedThisFrame())
        {
            aimHoldTime = 0f;
            shotAngle = Mathf.Clamp(shotAngle -1f, 1f, maxAngle);
            angleText.text = $"{Mathf.RoundToInt(shotAngle)}°";
        } 
        else if (aimDown.action.IsPressed())
        {
            aimHoldTime += Time.deltaTime;

            if (aimHoldTime >= holdDelay) // só gira contínuo depois do delay
            {
                shotAngle = Mathf.Clamp(shotAngle - aimSpeed * Time.deltaTime, 1f, maxAngle);
                angleText.text = $"{Mathf.RoundToInt(shotAngle)}°";
            }
        }

    }


    private void ChargeCannonShot()
    {
        //carregar tiro
        if (chargeAction.action.IsPressed()) //segurando
        {
            power = Mathf.Clamp(power + (maxPower / chargeTime) * Time.deltaTime, 0, maxPower); 
        }

        if (chargeAction.action.WasReleasedThisFrame()) //Quando solta
        {
            Debug.Log($"Potência acumulada: {power:F2}");
            FireProjectile(power);

            power = 0f; //resetando a força
        }     
    }
    private void FireProjectile(float shotPower)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity); //Aqui nós criamos uma cópia do prefab na cena
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        //direção
        float cos = Mathf.Cos(shotAngle * Mathf.Deg2Rad);
        float sin = Mathf.Sin(shotAngle * Mathf.Deg2Rad);

        Vector2 direction = new Vector2(cos, sin);

        rb.AddForce(direction * shotPower, ForceMode2D.Impulse);
    }

    //Linha de mira
    private void UpdateAimLine()
    {
        float aimCurve = 0.00f; // 0 = reta
        float aimLength = 1.6f;
        int aimPoints = 20;

        float radians = shotAngle * Mathf.Deg2Rad;

        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        Vector3 start = firePoint.position;
        
        aimLine.positionCount = aimPoints;
        for (int i = 0; i < aimPoints; i++)
        {
            float dist = aimLength * i / (aimPoints - 1);
            Vector2 offset = direction * dist + Vector2.down * (aimCurve * dist * dist);
            aimLine.SetPosition(i, start + (Vector3)offset);
        }
    }
}
