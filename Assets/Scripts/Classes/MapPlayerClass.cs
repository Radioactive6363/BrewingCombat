using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;

public class MapPlayerClass : MonoBehaviour
{
    private Dictionary<Vector3, BlockType> blockCoordinateSystem = new Dictionary<Vector3, BlockType>();
    private Vector3 playerSpawnOffset = Vector3.zero;
    private Vector3 currentBlock = Vector3.zero;
    private float dirtSeparationAmount;

    private Vector3 movementXZ = Vector2.zero;

    private bool playerMoving = false;

    private Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();

        if (playerAnimator == null)
        {
            Debug.LogWarning("Couldn't get player animator!");
        }

        if (BlockSpawnManager.Instance != null)
        {
            blockCoordinateSystem = BlockSpawnManager.Instance.blockCoordinateSystem;
            playerSpawnOffset = BlockSpawnManager.Instance.playerSpawnOffset;
            currentBlock = BlockSpawnManager.Instance.playerSpawnBlock;
            dirtSeparationAmount = BlockSpawnManager.Instance.dirtSeparationAmount;
        }
        else
        {
            Debug.LogWarning("Block Spawn Manager Not Found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerMoving)
        {
            movementXZ = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                movementXZ.z += 1;
                movementXZ.z = Mathf.Clamp(movementXZ.z, -1f, 1f);
            }

            if (Input.GetKey(KeyCode.S))
            {
                movementXZ.z -= 1;
                movementXZ.z = Mathf.Clamp(movementXZ.z, -1f, 1f);
            }
        
            if (Input.GetKey(KeyCode.D))
            {
                movementXZ.x += 1;
                movementXZ.x = Mathf.Clamp(movementXZ.x, -1f, 1f);
            }

            if (Input.GetKey(KeyCode.A))
            {
                movementXZ.x -= 1;
                movementXZ.x = Mathf.Clamp(movementXZ.x, -1f, 1f);
            }

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, currentBlock + playerSpawnOffset, Time.deltaTime * 20f);
        }

        if (movementXZ != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementXZ, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15f);
        }

        if (movementXZ != Vector3.zero && !playerMoving)
        {
            StartCoroutine(MovePlayer());
        }
    }

    private IEnumerator MovePlayer()
    {
        playerMoving = true;

        if (blockCoordinateSystem != null && dirtSeparationAmount != 0f && currentBlock != null)
        {
            Vector3 movementDirection = movementXZ * dirtSeparationAmount + transform.position - playerSpawnOffset;
            Debug.DrawLine(transform.position, movementDirection, Color.red, 1f);

            Vector3 closestBlock = Vector3.one * Mathf.Infinity;
            BlockType blockType = BlockType.Normal;
            float closestBlockDistance = Mathf.Infinity;
            foreach (var block in blockCoordinateSystem)
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
                    currentBlock = closestBlock;

                    if (playerAnimator != null)
                    {
                        playerAnimator.Play("Move_Forward");
                    }
                }
                else
                {
                    if (playerAnimator != null)
                    {
                        playerAnimator.Play("Attack");

                        yield return new WaitForSeconds(1f);

                        SceneManager.LoadScene("SampleScene");
                    }
                }
            }
        }

        yield return new WaitForSeconds(.4f);

        playerMoving = false;
    }
}
