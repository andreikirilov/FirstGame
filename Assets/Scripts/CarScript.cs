using UnityEngine;
using UnityEngine.UI;

public class CarScript : MonoBehaviour
{
    public AudioSource accelerationSound, brakeSound, buttonSound, jumpSound, gasCanister, rublesSound;
    public ClickScript[] controlCar;
    public GameObject car, centerOfMass, failPanel, finishPanel, gasProgressBar;
    public JointMotor2D frontWheel, rearWheel;
    public LayerMask level;
    public Text coinsText, moneyCount;
    public Transform fWheel, rWheel;
    public WheelJoint2D[] wheelJoints;

    public char del = 'd';
    public float wheelSize = 0.080f;
    public int coinsInt = 0;
    public int carNumber;
    public string[] tuning;

    public float acceleration = -500f;
    public float deacceleration = 250f;
    public float brakeForce = 2500f;
    public float maxBackwardSpeed = 1000f;
    public float maxForwardSpeed = -1500f;
    public float jumpForce = 3000;

    public float gasSize = 1f;
    public float gasUsage = 0.1f;
    public float gasCurrent = 1f;
    public float gasAdd = 0.5f;

    void Awake()
    {
        accelerationSound = GetComponent<AudioSource>();
        GetComponent<Rigidbody2D>().centerOfMass = centerOfMass.transform.localPosition;
        moneyCount.text = PlayerPrefs.GetInt("money").ToString();
        wheelJoints = gameObject.GetComponents<WheelJoint2D>();
        frontWheel = wheelJoints[0].motor;
        rearWheel = wheelJoints[1].motor;
        tuning = PlayerPrefs.GetString("tuning1").Split(del);
        acceleration -= 250f * int.Parse(tuning[0]);
        maxForwardSpeed -= 250f * int.Parse(tuning[0]);
        deacceleration += 250f * int.Parse(tuning[1]);
        maxBackwardSpeed += 250f * int.Parse(tuning[1]);
        jumpForce += 250f * int.Parse(tuning[2]);
    }
    void FixedUpdate()
    {
        if (gasCurrent <= 0)
        {
            for (int i = 0; i < controlCar.Length; i++)
            {
                controlCar[i].clickedIs = false;
                controlCar[i].gameObject.SetActive(false);
            }
            if (rearWheel.motorSpeed == 0)
            {
                failPanel.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < controlCar.Length; i++)
            {
                controlCar[i].gameObject.SetActive(true);
            }
        }
        accelerationSound.pitch = Mathf.Clamp(-rearWheel.motorSpeed / 3000f, 0.3f, 3f);
        coinsText.text = (10 * coinsInt).ToString();
        wheelJoints[0].motor = frontWheel;
        wheelJoints[1].motor = rearWheel;
        frontWheel.motorSpeed = rearWheel.motorSpeed;
        gasProgressBar.transform.localScale = new Vector2(gasCurrent / gasSize, 1);
        if (Physics2D.OverlapCircle(fWheel.transform.position, wheelSize, level) && Physics2D.OverlapCircle(rWheel.transform.position, wheelSize, level))
        {
            if (controlCar[2].clickedIs == true || Input.GetKeyDown(KeyCode.Space))
            {
                gasCurrent -= gasUsage;
                car.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                jumpSound.Play();
            }
            // Нажата педаль газа
            if (controlCar[0].clickedIs == true)
            {
                gasCurrent -= gasUsage * Time.deltaTime;
                rearWheel.motorSpeed = Mathf.Clamp(rearWheel.motorSpeed + acceleration * Time.deltaTime, maxForwardSpeed, 0);
                if (controlCar[2].clickedIs == true || Input.GetKeyDown(KeyCode.Space))
                {
                    gasCurrent -= gasUsage;
                    car.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    jumpSound.Play();
                }
            }
            // Остаточное движение вперед
            if (controlCar[0].clickedIs == false && rearWheel.motorSpeed < 0)
            {
                rearWheel.motorSpeed = Mathf.Clamp(rearWheel.motorSpeed + deacceleration * Time.deltaTime, maxForwardSpeed, 0);
            }
            // Нажата педаль тормоза
            if (controlCar[1].clickedIs == true && rearWheel.motorSpeed < 0)
            {
                rearWheel.motorSpeed = Mathf.Clamp(rearWheel.motorSpeed + brakeForce * Time.deltaTime, maxForwardSpeed, 0);
                brakeSound.Play();
            }
            // Нажата педаль заднего хода
            if (controlCar[1].clickedIs == true && rearWheel.motorSpeed >= 0)
            {
                gasCurrent -= gasUsage * Time.deltaTime;
                rearWheel.motorSpeed = Mathf.Clamp(rearWheel.motorSpeed + deacceleration * Time.deltaTime, 0, maxBackwardSpeed);
            }
            // Остаточное движение назад
            if (controlCar[1].clickedIs == false && rearWheel.motorSpeed > 0)
            {
                rearWheel.motorSpeed = Mathf.Clamp(rearWheel.motorSpeed - deacceleration * Time.deltaTime, 0, maxBackwardSpeed);
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Gas")
        {
            gasCanister.Play();
            gasCurrent = Mathf.Clamp(gasCurrent + gasAdd, 0, 1f);
            Destroy(trigger.gameObject);
        }
        if (trigger.gameObject.tag == "Coin")
        {
            coinsInt++;
            rublesSound.pitch = Random.Range(0.75f, 1.25f);
            rublesSound.Play();
            Destroy(trigger.gameObject);
            moneyCount.text = (int.Parse(moneyCount.text) + 10).ToString();
            PlayerPrefs.SetInt("money", int.Parse(moneyCount.text));
            PlayerPrefs.Save();
        }
        if (trigger.gameObject.tag == "Finish")
        {
            finishPanel.SetActive(true);
        }

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rWheel.transform.position, wheelSize);
    }
}