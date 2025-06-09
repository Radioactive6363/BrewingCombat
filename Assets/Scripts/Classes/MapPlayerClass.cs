using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;

public class MapPlayerClass : MonoBehaviour
{
    public bool canMove = true;

    private Dictionary<Vector3, BlockType> _blockCoordinateSystem = new Dictionary<Vector3, BlockType>();
    private Vector3 _playerSpawnOffset = Vector3.zero;
    private Vector3 _currentBlock = Vector3.zero;
    private float _dirtSeparationAmount;

    private Vector3 _movementXZ = Vector2.zero;

    private bool _playerMoving = false;

    private Animator _playerAnimator;

    private void Start()
    {
        _playerAnimator = GetComponentInChildren<Animator>();

        if (_playerAnimator == null)
        {
            Debug.LogWarning("Couldn't get player animator!");
        }

        if (BlockSpawnManager.Instance != null)
        {
            _blockCoordinateSystem = BlockSpawnManager.Instance.BlockCoordinateSystem;
            _playerSpawnOffset = BlockSpawnManager.Instance.playerSpawnOffset;
            _currentBlock = BlockSpawnManager.Instance.playerSpawnBlock;
            _dirtSeparationAmount = BlockSpawnManager.Instance.dirtSeparationAmount;
        }
        else
        {
            Debug.LogWarning("Block Spawn Manager Not Found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;

        if (!_playerMoving)
        {
            _movementXZ = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                _movementXZ.z += 1;
                _movementXZ.z = Mathf.Clamp(_movementXZ.z, -1f, 1f);
            }

            if (Input.GetKey(KeyCode.S))
            {
                _movementXZ.z -= 1;
                _movementXZ.z = Mathf.Clamp(_movementXZ.z, -1f, 1f);
            }
        
            if (Input.GetKey(KeyCode.D))
            {
                _movementXZ.x += 1;
                _movementXZ.x = Mathf.Clamp(_movementXZ.x, -1f, 1f);
            }

            if (Input.GetKey(KeyCode.A))
            {
                _movementXZ.x -= 1;
                _movementXZ.x = Mathf.Clamp(_movementXZ.x, -1f, 1f);
            }

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _currentBlock + _playerSpawnOffset, Time.deltaTime * 20f);
        }

        if (_movementXZ != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_movementXZ, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15f);
        }

        if (_movementXZ != Vector3.zero && !_playerMoving)
        {
            StartCoroutine(MovePlayer());
        }
    }

    private IEnumerator MovePlayer()
    {
        _playerMoving = true;

        if (_blockCoordinateSystem != null && _dirtSeparationAmount != 0f && _currentBlock != null)
        {
            Vector3 movementDirection = _movementXZ * _dirtSeparationAmount + transform.position - _playerSpawnOffset;
            Debug.DrawLine(transform.position, movementDirection, Color.red, 1f);

            Vector3 closestBlock = Vector3.one * Mathf.Infinity;
            BlockType blockType = BlockType.Normal;
            float closestBlockDistance = Mathf.Infinity;
            foreach (var block in _blockCoordinateSystem)
            {
                float distanceToDirection = Vector3.Distance(block.Key, movementDirection);

                if (distanceToDirection < closestBlockDistance && distanceToDirection < 0.25f && block.Value != BlockType.Obstructed)
                {
                    closestBlockDistance = distanceToDirection;
                    closestBlock = block.Key;
                    blockType = block.Value;
                }
            }

            if (closestBlockDistance != Mathf.Infinity)
            {
                if (blockType != BlockType.NavigationPoint)
                {
                    _currentBlock = closestBlock;

                    if (_playerAnimator != null)
                    {
                        _playerAnimator.Play("Move_Forward");
                    }
                }
                else
                {
                    if (_playerAnimator != null)
                    {
                        _playerAnimator.Play("Attack");

                        yield return new WaitForSeconds(1f);

                        SceneManager.LoadScene("SampleScene");
                    }
                }
            }
        }

        yield return new WaitForSeconds(.4f);

        _playerMoving = false;
    }

    public void PlayAnimation(string animationName)
    {
        if (_playerAnimator != null)
        {
            _playerAnimator.Play(animationName);
        }
    }
}
