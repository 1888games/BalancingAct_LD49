using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviourSingleton<GameController>
{

    public List<Material> MaterialsList;

    public int[] ForceTotals = new int[2];

    public float Balance = 100f;

    public List<TextMeshProUGUI> ForceText;
    public TextMeshProUGUI BestText;

    public Camera MainCamera;

    public TextMeshProUGUI BalanceText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI CurrentText;
    public TextMeshProUGUI YouScored;

    public GameObject GameOver;
    public GameObject TitleScreen;

    public List<Cube> Cubes;

    public const int Columns = 10;
    public const int Rows = 5;

    public GameObject CubePrefab;
    public Transform TiltingWorld;

    public float DropSpeed = 0.6f;

    public Vector3 SpawnPosition;



    public float MaxTilt = 8.37f;
    public float TargetTilt = 0f;
    public float CurrentTilt = 0f;
    public float MaxRatio = 2f;

    public int Score = 0;
    public int CurrentScore = 0;
    public int Best = 250000;

    
    // Start is called before the first frame update
    void Start()
    {

        //   Reset();
        //  NewCube();

        //QualitySettings.vSyncCount = 0;  // VSync must be disabled
        //Application.targetFrameRate = 60;

        //http://dreamlo.com/lb/9aQLCPQnfUeheMdJ1g4bwQG7oG8hBE3k6MwC_SKkGSBQ

        //   9aQLCPQnfUeheMdJ1g4bwQG7oG8hBE3k6MwC_SKkGSBQ

        // 61584a088f40bb0e288097f5
    }

    void Reset()
    {
        Score = 0;
        DropSpeed = 0.6f;
        TiltingWorld.localEulerAngles = new Vector3(0f, 0f, 0f);
        CurrentTilt = 0f;
        TargetTilt = 0f;

        GameOver.SetActive(false);
        TitleScreen.SetActive(false);


        string fmt = "0000000.##";

        BestText.text = "<mspace=0.7em>" + Best.ToString(fmt);

        foreach (Transform transform in TiltingWorld)
        {
            if (transform.GetComponent<Cube>() != null)
            {
                Destroy(transform.gameObject);
            }
        }

        Cubes = new List<Cube>();

        NewCube();
    }


    public void ShowGameOver()
    {

        
        GameOver.SetActive(true);
        YouScored.text = "YOU SCORED " + Score.ToString("N0");



    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if (GameOver.activeInHierarchy || TitleScreen.activeInHierarchy)
        {

            if (Input.GetMouseButtonDown(0))
            {
                Reset();
            }

            return;

        }

        ForceTotals[0] = 0;
        ForceTotals[1] = 0;

        string fmt = "0000000.##";

        ScoreText.text = "<mspace=0.7em>" + Score.ToString(fmt);

        foreach (Cube cube in Cubes)
        {
            ProcessCube(cube);
        }


        //ForceTotals[i] = Random.Range(0, 100);
        ForceText[0].text = ForceTotals[0].ToString();
        ForceText[1].text = ForceTotals[1].ToString();

        ForceTotals[0] = Mathf.Max(1, ForceTotals[0]);
        ForceTotals[1] = Mathf.Max(1, ForceTotals[1]);

        bool tiltLeft = false;

        int diff = ForceTotals[0] - ForceTotals[1];
        
        if (diff < 0)
        {
            tiltLeft = true;
            diff = ForceTotals[1] - ForceTotals[0];
        }

        float ratio = Mathf.Clamp((float)diff / 20,0,1f);
        if (ForceTotals[0] == 0 || ForceTotals[1] == 0) ratio = 1f;
        float tilt = ratio * MaxTilt;

        if (tiltLeft) tilt = -tilt;


        TargetTilt = tilt;

        if (CurrentTilt < TargetTilt)
        {
            CurrentTilt += 0.06f;

            if (CurrentTilt > TargetTilt) CurrentTilt = TargetTilt;
        }

        if (CurrentTilt > TargetTilt)
        {
            CurrentTilt -= 0.06f;

            if (CurrentTilt < TargetTilt) CurrentTilt = TargetTilt;
        }

        TiltingWorld.localEulerAngles = new Vector3(0f, 0f, CurrentTilt);

        CurrentScore = 0;

        if (Cubes.Count > 1)
        {

            if (tilt == 0)
            {
                CurrentScore = 100;
            }

            else
            {
                CurrentScore = Mathf.Clamp(100 - Mathf.RoundToInt(100f * Mathf.Abs(tilt) / MaxTilt), 0, 100);
            }



            Score += CurrentScore;

            BalanceText.text = "<mspace=0.7em>" + CurrentScore.ToString() + " % ";

            if (CurrentScore == 100) Score += CurrentScore;

            if (Score > Best)
            {
                Best = Score;
                string fmt2 = "0000000.##";
                BestText.text = "<mspace=0.7em>" + Best.ToString(fmt2);
            }

        }


        else
        {
            BalanceText.text = "";
        }


    }

    void ProcessCube(Cube cube)
    {
        if (cube == null) return;
        
        
        ForceTotals[cube.Side] += cube.Value * cube.Distance;
        

    }



    public void NewCube()
    {


        GameObject Cube = GameObject.Instantiate(CubePrefab, Vector3.zero, Quaternion.identity);

        Cube.transform.parent = TiltingWorld;

        Cube cube = Cube.GetComponent<Cube>();
   
        int chanceOfLow = Random.RandomRange(0, 100);

        if (chanceOfLow < 70)
        {
            cube.Value = Random.RandomRange(1, 5);
        }
        else
        {
            cube.Value = Random.RandomRange(6, 11);
        }


        Cube.transform.localPosition = SpawnPosition;
        cube.Settled = false;

       
        Cube.transform.localEulerAngles = Vector3.zero;

        DropSpeed += 0.0015f;
    }



}
