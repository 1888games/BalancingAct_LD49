using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using System.Linq;

public class Cube : MonoBehaviour
{
    public TextMeshProUGUI Number;
    public Material Material;
    public int Value;
    public int Distance;
    public int Row;
    public int Column;
    public int Side;
    public bool Settled = true;

    

    public MeshRenderer MeshRenderer;

    float cooldown = 0f;

    // Start is called before the first frame update
    void Start()
    {

        Number = GetComponentInChildren<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Number == null || GameController.Instance == null) return;

        Number.text = Value.ToString();

        Row = Mathf.FloorToInt(transform.localPosition.y - 1.8f);

        if (Settled == false)
        {
            MoveDown(GameController.Instance.DropSpeed * Time.deltaTime);
            CheckControls();

        }


        Column = Mathf.RoundToInt(transform.localPosition.x + 5f);

        Side = Column < 5 ? 0 : 1;
        Distance = Column < 5 ? 5 - Column : Column - 5;
        Row = Mathf.FloorToInt(transform.localPosition.y - 1.8f);


        MeshRenderer.material = GameController.Instance.MaterialsList[Value-1];
    }



    void CheckControls()
    {
        if (Settled) return;

        if (Input.GetAxisRaw("Vertical") == -1f)
        {

            MoveDown(6f * Time.deltaTime);
            GameController.Instance.Score += 50;
            return;
       
        }

        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
            return;
        }


     

        if (Input.GetAxisRaw("Horizontal") == - 1f && Column > 0)
        {
            int MoveAlong = 1;

            if (Column == 6) MoveAlong = 2;

            Cube cube = GameController.Instance.Cubes.Where(p => p.Column == Column - MoveAlong && p.Row == Row).FirstOrDefault();


            if (cube != null)
            {
                Debug.Log("Cube in way");
            }

            else


            {

                transform.localPosition = new Vector3(transform.localPosition.x - (float)MoveAlong, transform.localPosition.y, 0f);
            }

            cooldown = 0.1f;
          

        }

        if (Input.GetAxisRaw("Horizontal") == 1f && Column < 10)
        {
            int MoveAlong = 1;

            if (Column == 4) MoveAlong = 2;
            Cube cube = GameController.Instance.Cubes.Where(p => p.Column == Column + MoveAlong && p.Row == Row).FirstOrDefault();

            if (cube != null)
            {
                Debug.Log("Cube in way");
            }

            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x + (float)MoveAlong, transform.localPosition.y, 0f);
            }

            cooldown = 0.1f;


        }


   

    }

    void CubeSettled()
    {


        Settled = true;
        GameController.Instance.Cubes.Add(this);


        if (Row <= 4) {
            GameController.Instance.NewCube();
        
        }
        else
        {
            GameController.Instance.ShowGameOver();
        }





    }
    void MoveDown(float speed)
    {

        float y = transform.localPosition.y - speed;

        if (y <= 1.8f)
        {
            y = 1.8f;
            Row = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, y, 0f);

            CubeSettled();
    
            return;
        }

        transform.localPosition = new Vector3(transform.localPosition.x, y, 0f);

        int NewRow = Mathf.FloorToInt(transform.localPosition.y - 1.8f);

        if (NewRow < Row)
        {

         //   Debug.Log("New Row: " + NewRow);


            Cube cube = GameController.Instance.Cubes.Where(p => p.Column == Column && p.Row == NewRow).FirstOrDefault();

            Row = NewRow;

            if (cube != null)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, cube.transform.localPosition.y + 1f, 0f);
                CubeSettled();

   
            }

           

        }


       
    }
   
}
