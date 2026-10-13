using UnityEngine;
using UnityEngine.InputSystem;

public class CannonController : MonoBehaviour
{
    [SerializeField] private InputActionReference chargeAction;
    //O InputActionReference é o tipo da variável, indica que a variável vai guardar uma referência da ação do input system.
    [SerializeField] private float chargeSpeed = 1f;
    private float power = 0f;
    [SerializeField] private float maxPower = 5f;


    //Lançamento
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shotAngle = 45f;



    //OnEnable e OnDisable são nomes especiais, o próprio unity identifica automaticamente.
    //(por hedar MonoBehaviour ele consegue entender)
    private void OnEnable(){
        chargeAction.action.Enable(); //isso aqui é basicamente pra "ligar" a ação em si (o Z que configuramos lá)
    }

     private void OnDisable(){
        chargeAction.action.Disable();
    }

    private void Update()
    {
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
            power = 0f; //resetando a força
        }
    }
    
}
