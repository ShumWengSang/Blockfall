using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AdvancedInspector;

public class WaitForSecondPair
{
    public WaitForSeconds wait;
    public float time;

    public WaitForSecondPair(float time)
    {
        wait = new WaitForSeconds(time);
    }
}

public class ShowSolutionScript : MonoBehaviour {
    public Transform camera_transform_solution;
    private Vector3 camera_pos_original;
    private Vector3 camera_pos_solution;

    private GameObject firstCamera_gameobject;
    private GameObject secondCamera_gameoject;
    public Camera secondCamera;
    public Camera firstCamera;

    public float timeToMove = 0.5f;
    CameraPos currentPos;

    private bool tweening;

    public RotateGrid grid;
    public ReadAnswerFile read_file;

    Stack<string> Answers;

    private WaitForSecondPair oneSec;
    public float TimeToWaitBetweenMove = 5f;

    enum CameraPos
    {
        Left,
        Right
    }

    private void OnEnable()
    {
        oneSec = new WaitForSecondPair(TimeToWaitBetweenMove);

        firstCamera_gameobject = firstCamera.gameObject;
        secondCamera_gameoject = secondCamera.gameObject;
        camera_pos_original = firstCamera_gameobject.transform.position;
        camera_pos_solution = camera_transform_solution.position;

        secondCamera.transform.position = Camera.main.transform.position;
        secondCamera_gameoject.SetActive(false);
        currentPos = CameraPos.Left;
        tweening = false;
    }

    public void ChangeCamera()
    {
        if(firstCamera_gameobject.activeInHierarchy)
        {
            FlipCamera(false);
        }
        else
        {
            FlipCamera(true);
        }
    }

    void ChangeCameraPos()
    {
        if (tweening)
            return;
        tweening = true;
        if (currentPos == CameraPos.Left)
        {
            secondCamera_gameoject.transform.DOMove(camera_pos_solution, timeToMove).OnComplete(() => { tweening = false;  });
            currentPos = CameraPos.Right;
        }
        else
        {
            secondCamera_gameoject.transform.DOMove(camera_pos_original, timeToMove).OnComplete(() => { tweening = false;  }); ;
            currentPos = CameraPos.Left;
        }
    }

    void FlipCamera(bool flip)
    {
        firstCamera_gameobject.SetActive(flip);
        secondCamera_gameoject.SetActive(!flip);
    }

    public void StartShowingSolution()
    {
        StartCoroutine(Solution_Coroutine());
    }

    IEnumerator Solution_Coroutine()
    {
        Answers = read_file.List_Answer;
        ChangeCamera();
        yield return new WaitForSeconds(2.0f);
        ChangeCameraPos();
        while(tweening)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2.0f);
        
        while(Answers.Count != 0)
        {
            string nextMove = Answers.Pop();
            if(nextMove.Contains("Left"))
            {
                grid.ButtonLeft();
            }
            else
            {
                grid.ButtonRight();
            }
            yield return oneSec.wait;
        }
    }
}
