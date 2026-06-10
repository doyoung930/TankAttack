using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed  = 8f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float fireForce = 1000f;
    [SerializeField] private Camera mainCamera;
    
    private InputSystem_Actions inputActions;
    private InputAction moveAction;
    private InputAction fireAction;
    
    private Vector2 moveInput;
    private Vector3 moveDir;
    
    #region 유니티 생명 주기
    
    
    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        moveAction = inputActions.Player.Move;
        fireAction = inputActions.Player.Attack;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
        
        fireAction.Enable();
        fireAction.started += OnFire;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
        moveAction.Disable();
        
        fireAction.started -= OnFire;
        fireAction.Disable();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        
    }

    private void Update()
    {
        Movement();
    }
    
    #endregion
    
    #region 이동 처리 및 발사로직

    private void Movement()
    {
        moveDir = new Vector3(moveInput.x, 0, moveInput.y);

        if (moveDir.magnitude > 0.1f)
        {
            // 카메라 기준으로 방향 계산
            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;
            
            // Y축 제거
            cameraForward.y = 0;
            cameraRight.y = 0;
            
            // 이동 방향 계산 (전진방향 * 키값) + (좌우방향 * 키값)
            moveDir = (cameraForward * moveInput.y) + (cameraRight * moveInput.x);
            
            // Transform 컴포넌트를 이용해서 이동 처리
            transform.Translate(moveDir * moveSpeed * Time.deltaTime,Space.World);
            
            // 회전 처리 (부드러운 회전처리 Slerp)
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }
    }
    #endregion
    
    #region 이벤트 핸들러
    private void OnFire(InputAction.CallbackContext obj)
    {
        Debug.Log("Fire");
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    #endregion
}
