using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeShot : MonoBehaviour
{
    [SerializeField] private InputActionReference chargeAction;
    //O InputActionReference é o tipo da variável, indica que a variável vai guardar uma referência da ação do input system.

    //OnEnable e OnDisable são nomes especiais, o próprio unity identifica automaticamente.
    //(por hedar MonoBehaviour ele consegue entender)
    private void OnEnable(){
        chargeAction.action.Enable(); //isso aqui é basicamente pra "ligar" a ação em si (o Z que configuramos lá)
        Debug.Log("Iniciado");
    }

     private void OnDisable(){
        chargeAction.action.Disable();
    }

    private void Update()
    {
        if (chargeAction.action.WasPressedThisFrame())
        {
            Debug.Log("Z foi pressionado!");
        }

        if (chargeAction.action.IsPressed())
        {
            Debug.Log("Z foi segurado!");
        }
    }
    
}
