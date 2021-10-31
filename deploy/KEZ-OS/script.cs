/*
 * KEZ-OS v0.01
 */

readonly ScriptScheduler SCRIPT_SCHEDULER;
readonly SchedulableScript debugScript;
public Program()
{
    SCRIPT_SCHEDULER = new ScriptScheduler();
    debugScript = new DebugScript();

    SCRIPT_SCHEDULER.RegisterScript(debugScript);
}

public void Save()
{

}

public void Main(string argument, UpdateType updateSource)
{
    SCRIPT_SCHEDULER.ContinueAll(updateSource);

    SCRIPT_SCHEDULER.DecodeArgument(argument).Run();

    SCRIPT_SCHEDULER.ScheduleAll(SetUpdateFrequency);
}

public void SetUpdateFrequency(UpdateFrequency frequency)
{
    Runtime.UpdateFrequency = frequency;
}

public class DebugScript : SchedulableScript
{
    private byte counter = 0;
    public DebugScript() : base(ScriptType.DEBUG)
    {

    }

    public override void Run()
    {
        Activate(UpdateFrequency.Update100);
        counter = 0;
    }

    protected override void DoContinue()
    {
        if (counter > 2) DeActivate();
        else counter++;
    }
}

public abstract class SchedulableScript
{
    public ScriptType ScriptType { get; }

    private bool ImActive;
    private UpdateFrequency nextFrequency = UpdateFrequency.None;

    protected SchedulableScript(ScriptType scriptType)
    {
        ScriptType = scriptType;
    }

public void Continue()
    {
        if (ImActive) DoContinue();
    }

protected abstract void DoContinue();

protected void Activate(UpdateFrequency frequency)
    {
        nextFrequency = frequency;
        ImActive = true;
    }

protected void DeActivate()
    {
        nextFrequency = UpdateFrequency.None;
        ImActive = false;
    }

public abstract void Run();

public UpdateFrequency GetSchedule()
    {
        return nextFrequency;
    }
}

public enum ScriptType : byte
{
    DEBUG
}

public class ScriptScheduler
{
    private readonly FrequencySchedule frequencySchedule = new FrequencySchedule();
    private readonly Dictionary<ScriptType, SchedulableScript> scripts = new Dictionary<ScriptType, SchedulableScript>();

public void RegisterScript(SchedulableScript script)
    {
        scripts.Add(script.ScriptType, script);
    }

public Dictionary<ScriptType, SchedulableScript> GetAllScripts()
    {
        return scripts;
    }

public SchedulableScript FindScriptByName(string name)
    {
        switch (name)
        {
            case "DEBUG": return scripts[ScriptType.DEBUG];
            default: return null;
        }
    }

public void ContinueAll(UpdateType updateType)
    {
        if((updateType & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) != 0)
        {
            foreach (var script in scripts.Values)
            {
                script.Continue();
            }
        }
    }

public SchedulableScript DecodeArgument(string argument)
    {
        return FindScriptByName(argument);
    }

public void ScheduleAll(Action<UpdateFrequency> schedulerAction)
    {
        frequencySchedule.Reset();

        foreach (var script in scripts.Values)
        {
            frequencySchedule.Evaulate(script.GetSchedule());
        }

        schedulerAction.Invoke(frequencySchedule.CalculateScheduleFrequency());
    }

}

public class FrequencySchedule
{
    private UpdateFrequency schdeuleFrequency;

    public bool schedule100 = false;
    public bool schedule10 = false;
    public bool schedule1 = false;

public void Reset()
    {
        schedule100 = false;
        schedule10 = false;
        schedule1 = false;
    }

public void Evaulate(UpdateFrequency frequency)
    {
        schedule100 = frequency == UpdateFrequency.Update100 || schedule100;
        schedule10 = frequency == UpdateFrequency.Update10 || schedule10;
        schedule1 = frequency == UpdateFrequency.Update1 || schedule1;
    }

public UpdateFrequency CalculateScheduleFrequency()
    {
        schdeuleFrequency = UpdateFrequency.None;

        schdeuleFrequency = schedule100 ? schdeuleFrequency | UpdateFrequency.Update100 : schdeuleFrequency;
        schdeuleFrequency = schedule10 ? schdeuleFrequency | UpdateFrequency.Update10 : schdeuleFrequency;
        schdeuleFrequency = schedule1 ? schdeuleFrequency | UpdateFrequency.Update1 : schdeuleFrequency;

        return schdeuleFrequency;
    }
}