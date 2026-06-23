using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ATMInteraction : MonoBehaviour
{
    public ATMUI atmUI; // Assign in Inspector
    public PlayerBalance playerBalance;

    private void OnMouseDown()
    {
        if (playerBalance == null)
            playerBalance = FindObjectOfType<PlayerBalance>();

        atmUI.OpenATM(playerBalance);
    }

    // Optional: For better interaction (raycast from camera)
    // Remove OnMouseDown and use this method instead if you prefer
    /*
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                atmUI.OpenATM(playerBalance);
            }
        }
    }
    */
}