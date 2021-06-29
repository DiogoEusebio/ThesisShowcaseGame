using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Agent : MonoBehaviour
{
    protected List<Goal> GoalList = new List<Goal>();
    protected List<Action> ActionList = new List<Action>();
    public List<Role> RoleList = new List<Role>();
    protected Goal GoalBeingPursued;
    protected Action ActionToExecute;
    private float MaxHP;
    private float CurrentHP;
    public bool isDead = false;
    protected float respawnTimer;
    protected Transform capturedFlag;
    private Slider HealthBar;
    private int[,] HeatMap;
    private int AgentID;

    float MinX = -20.0f; //rough Coordinates of the arena for a specefic unity scene
    float MaxX = 20.0f;
    float MinZ = -10.0f;
    float MaxZ = 25.0f;
    float RangeX, RangeZ, IncrementX, IncrementZ;
    int xSize, zSize;
    public enum AgentType
    {
        Cube,
        Sphere,
        Cone,
        Tetrahedron
    }
    private AgentType agentType;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        SetMaxHP(100.0f);
        SetCurrentHPtoMax();
        GenerateBasicAgentGoals();
        GetActionsFromGoals();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isDead)
        {
            respawnTimer -= Time.deltaTime;
            RespawnAgent(respawnTimer);
        }
        else
        {
            WalkRandomly();
        }
    }
    protected virtual void FixedUpdate()
    {
        //do nothing
    }
    //--------------------- Gets and Sets -----------------------//
    public void SetAgentType(AgentType at) { agentType = at; }
    public void SetAgentID(int id) { AgentID = id; }
    public AgentType GetAgentType() { return agentType; }
    public int GetAgentID() { return AgentID; }
    public List<Role> GetRoleList() { return RoleList; }
    public List<Goal> GetGoalList() { return GoalList; }
    public List<Action> GetActionList(){ return ActionList; }
    public bool GetIsDead() { return isDead; }
    public void SetIsDead(bool value) { isDead = value; }
    public void SetMaxHP(float newMaxHPvalue)
    {
        MaxHP = newMaxHPvalue;
        HealthBar = GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>();
        HealthBar.value = ((int)newMaxHPvalue);
    }

    public void SetCurrentHPtoMax() { CurrentHP = MaxHP; HealthBar.value = (int)MaxHP; }
    public void TakeDamage(float dmg)
    {
        //Debug.Log("current Hp: " + CurrentHP);
        CurrentHP -= dmg;
        HealthBar.value -= (int)dmg;
        /*HealthBar.SetHealth((int)CurrentHP);*/
        if(CurrentHP <= 0)
        {
            KillAgent();
        }
    }
    public void Heal(float val)
    {
        if(CurrentHP + val >= MaxHP)
        {
            CurrentHP = MaxHP;
            HealthBar.value = (int)MaxHP; //works cuz they have the same scale
        }
        else
        {
            CurrentHP += val;
            HealthBar.value += (int)val;
        }
    }
    public bool HasCapturedFlag()
    {
        if (capturedFlag == null)
        {
            return false;
        }
        return true;
    }
    public void SetCapturedFlag(Transform flag)
    {
        capturedFlag = flag;
    }
    public void DropFlag()
    {
        capturedFlag = null;
    }
    public void ConsumeResourceToHeal()
    {
        //heal Self if damaged
        if(CurrentHP != MaxHP)
        {
            Heal(100);
        }
        else
        {
            Role TeammateRole = RoleList.Find((role) => role.GetName() == "TeammateRole");
            foreach (GameObject teammate in TeammateRole.GetTargetAgents())
            {
                teammate.GetComponent<Agent>().Heal(25);
            }
        }
    }
    public void GetActionsFromGoals()
    {
        //Debug.Log("GETTING ACTIONS...");
        //TODO: add check for repeated actions
        foreach(Goal g in GoalList)
        {
            //Debug.Log(g.GetName());
            ActionList.AddRange(g.GetActionsFromGoal());
        }
    }
    public void GetActionsFormSpecificGoal(Goal g)
    {
        if(GoalList.Contains(g))
        {
            ActionList.AddRange(g.GetActionsFromGoal());
        }
    }
    public void GenerateGoalsBasedOnRole(Role role)
    {
        //TODO
    }
    protected virtual void GenerateBasicAgentGoals()
    {
        GoalList.Add(new MoveToTargetCoordsGoal(transform, new Vector3(Random.Range(-28.0f,28.0f), 1.0f, Random.Range(-5.0f, 5.0f))));
    }
    public Vector3 GetAgentCurrentPosition()
    {
        return transform.position;
    }

    //might need to check if every case is covered
    public void RemoveActionsAssociatedToGoal(Goal goalBeingRemoved)
    {   
        //remove actions exclusive to this goal, leaving shared actions behind
        foreach(Action a in goalBeingRemoved.GetActionsFromGoal())
        {
            bool canBeRemoved = true;
            foreach (Goal g in GoalList)
            {
                if(g != goalBeingRemoved)
                {
                    foreach (Action b in g.GetActionsFromGoal())
                    {
                        if (a.GetName() == b.GetName())
                        {
                            canBeRemoved = false;
                            break; //no need to check further, since we now know we cant remove the action because it is shared with other goals
                        }
                    }
                    //check if we can also stop this loop
                    if (!canBeRemoved) { break; }
                }
            }
            if (canBeRemoved)
            {
                ActionList.Remove(a);
                //Debug.Log("ACTION REMOVED: " + a);
            }
        }                
    }
    public void RemoveAchivedGoal(Goal toRemove)
    {
        GoalList.Remove(toRemove);
    }

    public Action GetRandomActionToAchiveSpecifiedGoal(Goal g)
    {
        int numberOfActions = g.GetActionsFromGoal().Count; //-1 to get list index
        //Debug.Log(numberOfActions);
        Action RandomActionFromThisGoal = g.GetActionsFromGoal()[Random.Range(0, numberOfActions - 1)];
        //Debug.Log(RandomActionFromThisGoal.GetName());
        return RandomActionFromThisGoal;
    }

    public void PerformSimulActions()
    {
        Action LookAtAction = ActionList.Find((action) => action.GetName() == "LookAtCloserEnemyAction");
        if (LookAtAction != null) { LookAtAction.Perform(); }
    }

    public void KillAgent()
    {
        if (HasCapturedFlag())
        {
            Action CapFlag = ActionList.Find((action) => action.GetName() == "CaptureFlagAction");
            CapFlag.DropFlag();
        }
        isDead = true;
        LogAgentActionResult("Died");
        this.transform.position = new Vector3(0.0f, -50.0f, 0.0f);
        //Clear Goals and Actions
        ActionList = new List<Action>();
        respawnTimer = 0.05f * Time.realtimeSinceStartup + 2.0f; //consider other equation
        RespawnAgent(respawnTimer);
    }
    public void RespawnAgent(float respawnTimer)
    {
        if (respawnTimer <= 0)
        {
            isDead = false;
            SetMaxHP(100.0f);
            SetCurrentHPtoMax();
            //GetGoalsFromRoles();
            GetActionsFromGoals();
            DebugLogGoals();
            DebugLogActions();
            if (transform.gameObject.CompareTag("BlueTeam"))
            {
                transform.position = new Vector3(-12.87f + Random.Range(-2.5f, 2.5f), 1.0f, 14.77f + Random.Range(-2.5f, 2.5f)); //Hack: copy pasted value from AgentManager, consider doing this there/acess the variable
            }
            else if (transform.gameObject.CompareTag("RedTeam"))
            {
                transform.position = new Vector3(12.87f + Random.Range(-2.5f, 2.5f), 1.0f, 14.77f + Random.Range(-2.5f, 2.5f)); //Hack: copy pasted value from AgentManager, consider doing this there/acess the variable
            }
            else if (transform.gameObject.CompareTag("GreenTeam"))
            {
                transform.position = new Vector3(0.0f + Random.Range(-2.5f, 2.5f), 1.0f, -7.0f + Random.Range(-2.5f, 2.5f)); //Hack: copy pasted value from AgentManager, consider doing this there/acess the variable
            }
            LogAgentActionResult("Respawned");
        }
    }
    //-------------------------- Debug Methods -----------------------//

    void OnDrawGizmos()
    {
        Color color;
        color = Color.green;
        DrawHelperAtCenter(this.transform.forward, color, 2f);
    }
    private void DrawHelperAtCenter(Vector3 direction, Color color, float scale)
    {
        Gizmos.color = color;
        Vector3 destination = transform.position + direction * scale;
        Gizmos.DrawLine(transform.position, destination);
    }
    public void DebugLogActions()
    {   
        string DebugLogActionsString = "";
        foreach(Action a in ActionList)
        {
            DebugLogActionsString += " | " + a.GetName();
        }
        Debug.Log(DebugLogActionsString);
    }
    public void DebugLogGoals()
    {
        string DebugLogGoalsString = "";
        foreach (Goal a in GoalList)
        {
            DebugLogGoalsString += " | " + a.GetName();
        }
        Debug.Log(DebugLogGoalsString);
    }
    public void DebugLogRoles()
    {
        string DebugLogRolesString = "";
        foreach(Role r in RoleList)
        {
            DebugLogRolesString += " | " + r.GetName();
        }
        Debug.Log(DebugLogRolesString);
    }

    //------------------------ AI/Behavior methods -----------------------------//
    public void WalkRandomly()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "MoveToPositionGoal");
        //PerformSimulActions(); //look at doesn work with random Action (gets stuck just looking)
        ActionToExecute = GetRandomActionToAchiveSpecifiedGoal(GoalBeingPursued);
        if (ActionToExecute.Perform() == Action.State.Executed)
        {
            GoalBeingPursued.SetGoalState(Goal.State.Achieved);
            RemoveAchivedGoal(GoalBeingPursued);
            RemoveActionsAssociatedToGoal(GoalBeingPursued);
            GoalList.Add(new MoveToTargetCoordsGoal(transform, new Vector3(Random.Range(-12.0f, 12.0f), 1.0f, Random.Range(-7.0f, 22.0f))));
        }
    }

    public void ContestObjective()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "ContestObjectiveGoal");
        PerformSimulActions();
        ActionToExecute = ActionList.Find((action) => action.GetName() == "MoveToPositionAction");
        ActionToExecute.UpdateDirection();
        if (ActionToExecute.Perform() == Action.State.Executed)
        {
            GoalBeingPursued.SetGoalState(Goal.State.Achieved);
            RemoveAchivedGoal(GoalBeingPursued);
            RemoveActionsAssociatedToGoal(GoalBeingPursued);
            GameObject objective = GameObject.FindWithTag("Objective");
            Vector3 ObjPos = objective.transform.position;
            GoalList.Add(new ContestObjectiveGoal(transform, new Vector3(Random.Range(ObjPos.x -3.0f, ObjPos.x + 3.0f), 1.0f, Random.Range(ObjPos.z - 3.0f, ObjPos.z + 3.0f))));
            GetActionsFormSpecificGoal(GoalList.Find((goal) => goal.GetName() == "ContestObjectiveGoal"));
        }
    }
    public void CaptureFlag()
    {
        GoalBeingPursued = GoalList.Find((goal) => goal.GetName() == "CaptureFlagGoal");
        ActionToExecute = ActionList.Find((action) => action.GetName() == "CaptureFlagAction");
        //DebugLogActions();
        //Debug.Log(GoalBeingPursued + " | " + ActionToExecute);
        ActionToExecute.UpdateDirection();
        //if (ActionToExecute.GetClosestFlagPosition() != new Vector3(1000f, 1000f, 1000f))
        
        if (ActionToExecute.Perform() == Action.State.Executed)
        {
            GoalBeingPursued.SetGoalState(Goal.State.Achieved);
            RemoveAchivedGoal(GoalBeingPursued);
            RemoveActionsAssociatedToGoal(GoalBeingPursued);
            //GameObject objective = GameObject.FindWithTag("Objective"); //remove later and add look at resource action
            //Vector3 ObjPos = objective.transform.position;
            GoalList.Add(new CaptureFlagGoal(transform));
            GetActionsFormSpecificGoal(GoalList.Find((goal) => goal.GetName() == "CaptureFlagGoal"));
            //Debug.Log("Flag Captured");
        }

        /*else
        {
            Debug.Log("Waiting for Flags to spawn");
        }*/
    }
    //-------------------- LOG METHODS ---------------------------//
    public int[,] SetUpHeatMap()
    {
        RangeX = System.Math.Abs(MaxX - MinX);
        RangeZ = System.Math.Abs(MaxZ - MinZ);
        IncrementX = RangeX / xSize;
        IncrementZ = RangeZ / zSize;
        int[,] HeatMap = new int[xSize, zSize];
        
        for (int i = 0; i < xSize; i++)
        {
            for(int j = 0; j < zSize; j++)
            {
                HeatMap[i,j] = 0;
            }
        }
        return HeatMap;

    }
    public void UpdateHeatMap()
    {
        Vector3 currPos = this.transform.position;
        int indexX = 0, indexZ = 0; //ignore assigment
        for (int i = 0; i < xSize; i++)
        {
            if (currPos.x < MinX + i * IncrementX && currPos.x > MinX + (i + 1) * IncrementX)
            {
                indexX = i;
            }
        }
        for (int i = 0; i < zSize; i++)
        {
            if (currPos.z < MinZ + i * IncrementZ && currPos.z > MinZ + (i + 1) * IncrementZ)
            {
                indexZ = i;
            }
        }
        HeatMap[indexX, indexZ] ++;
    }
    public void LogAgentActionResult(string Info)
    {
        //this may be redundant, because file are already created, but oh well...
        string fileName = "\\" + agentType.ToString() + AgentID.ToString() + ".txt";
        string path = @"\Assets\Log";
        string dir = Directory.GetCurrentDirectory();
        string fullPath;

        fullPath = Path.GetFullPath(dir + path + fileName);
        //Debug.Log(fullPath);
        if (!File.Exists(fullPath))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(fullPath))
            {
                sw.WriteLine(Info);
            }
        }
        else
        {
            using (StreamWriter sw = File.AppendText(fullPath))
            {
                string myString = string.Format("{0,6:F3} | ({1,6:0.00}, {2,6:0.00}) | ", Time.timeSinceLevelLoad, transform.position.x, transform.position.z); 
                sw.WriteLine(myString +  Info);
            }
        }
    }
    public void InitAgentLog()
    {
        string fileName = agentType.ToString() + AgentID.ToString() + ".txt";
        string path = @"\Assets\Log\";
        string dir = Directory.GetCurrentDirectory();
        string fullPath;

        fullPath = Path.GetFullPath(dir + path);
        //delete previous files

        string[] txtList = Directory.GetFiles(fullPath, fileName);
        foreach (string f in txtList)
        {
            Debug.Log(f);
            File.Delete(f);
        }

        //create new files
        using (StreamWriter sw = File.CreateText(fullPath + fileName))
        {
            sw.WriteLine("Agent ID: " + GetAgentID().ToString() + "\nAgent Type: " + GetAgentType().ToString() +"\nTeam: " + transform.tag);

        }
    }
}
