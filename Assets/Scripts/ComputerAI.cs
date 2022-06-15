using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerAI : MonoBehaviour {

    List<Vector3> trajectoryPoints = new List<Vector3>();

    Ball ball;

    float topLimit = 0;
    float bottomLimit = 0;
    float rightLimit = 0;

    float currentPosition;
    float targetPosition;

    float distanceToReact     = 0f;
    float uncertaintyPosition = 0.8f;
    float speed = 10f;

    float randomMove = 0;
    float randomMoveInterval = 1.0f;

    public int side = 1;

    void Start() {

        ball = FindObjectOfType<Ball>();

        topLimit = Camera.main.orthographicSize;
        bottomLimit = -Camera.main.orthographicSize;
        rightLimit = Camera.main.orthographicSize * Camera.main.aspect + transform.localScale.y / 2;

        randomMove = Time.time + Random.Range(0, 1.5f);
    }

    public void _Update() {

        //IA 1
        //Só pra constar, o "side" é a variável que distingue as IA's pq elas não podem ter movimento espelhado
        if (side == 1) {
            if (Time.time >= randomMove && ball.transform.position.x <= distanceToReact) {
                targetPosition = ball.transform.position.y;
                randomMove = Time.time + randomMoveInterval;
            }
        //IA 2
        } else {
            if (Time.time >= randomMove && ball.transform.position.x >= -distanceToReact) {
                targetPosition = ball.transform.position.y;
                randomMove = Time.time + randomMoveInterval;
            }
        }
        // Aplica o movimento
        GoToPosition();

        // calcula a trajetória
        SimulateBallTrajectory();

        currentPosition = transform.position.y;
        
        GoToPosition();
    }

    // Faz a raquete se mover para a posição alvo, respeitando os limites da tela
    void GoToPosition() {

        currentPosition = Mathf.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, currentPosition);

        transform.position = new Vector3(transform.position.x,
                                Mathf.Clamp(transform.position.y,
                                bottomLimit + transform.localScale.y / 2,
                                topLimit - transform.localScale.y / 2));
    }

    void SimulateBallTrajectory() {

        int iterations = 400;
        float step     = 0.01f;

        Vector2 position = ball.transform.position;
        Vector2 velocity = ball.velocity;

        while (iterations > 0) {

            position += velocity * step;

            if(position.y >= topLimit || position.y <= bottomLimit) {
                velocity.y *= -1;
            }

            //IA 1
            if (side == 1) {
                if (position.x >= 8.2f) {
                    targetPosition = position.y;
                    break;
                }
            //IA 2
            } else {
                if (position.x <= -8.2f) {
                    targetPosition = position.y;
                    break;
                }
            }
            --iterations;
        }
    }

    public void Reset() {
        transform.position = new Vector3(transform.position.x, 0);
    }
}
