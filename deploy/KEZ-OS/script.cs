/*
 * KEZ-OS v0.01
 */

private readonly TerminalLogger logger;
private MyCommandLine commandLine;
private readonly BlockProvider blockProvider;

private readonly NamesScript namesScript;

private readonly ScriptRouter scriptRouter;
private readonly ContinuousServiceHandler continuousServiceHandler;

public Program()
{
    logger = new TerminalLogger(Echo);
    commandLine = new MyCommandLine();
    blockProvider = new BlockProvider(GridTerminalSystem.GetBlocks);

    namesScript = new NamesScript(logger, blockProvider, Me);

    scriptRouter = new ScriptRouter(logger);
    scriptRouter.RegisterScript(namesScript);

    continuousServiceHandler = new ContinuousServiceHandler(1);
    continuousServiceHandler.RegisterService(blockProvider);
}

public void Save()
{

}

public void Main(string argument, UpdateType updateSource)
{
    continuousServiceHandler.Continue();

    if (commandLine.TryParse(argument))
    {
        scriptRouter.Route(ref commandLine);
    }


}

public class BlockProvider : IContinuousService
{
    private readonly Action<List<IMyTerminalBlock>> getBlocks;
    private List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();

    private bool blocksWereSearched = false;

    public BlockProvider(Action<List<IMyTerminalBlock>> getBlocks)
    {
        this.getBlocks = getBlocks;
    }

    public void Continue()
    {
        blocksWereSearched = false;
    }

    public List<IMyTerminalBlock> GetBlocks()
    {
        if (!blocksWereSearched)
        {
            getBlocks.Invoke(blocks);
        }

        return blocks;
    }
}

public interface IContinuousService
{
    void Continue();
}

public class ContinuousServiceHandler
{
    private readonly IContinuousService[] services;
    private byte index = 0;

    public ContinuousServiceHandler(byte maxServices)
    {
        services = new IContinuousService[maxServices];
    }

    public void RegisterService(IContinuousService service)
    {
        if(index >= services.Length)
        {
            return;
        }

        services[index] = service;
        index++;
    }

    public void Continue()
    {
        for (int i = 0; i < services.Length; i++)
        {
            services[i].Continue();
        }
    }
}

public class NamesScript : Script
{
    private const string scriptName = "name";

    private const string switchForAllConnectedGrids = "all";
    private const string switchForAddAsPrefix = "prefix";

    private readonly Dictionary<string, Action> commands;
    private readonly BlockProvider blockProvider;
    private readonly IMyTerminalBlock myBlock;

    public NamesScript(TerminalLogger logger, BlockProvider blockProvider, IMyTerminalBlock myBlock) : base(logger)
    {
        this.blockProvider = blockProvider;
        this.myBlock = myBlock;

        commands = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
        {
            ["replace"] = Replace,
            ["replace-filtered"] = ReplaceFiltered,
            ["add"] = Add,
            ["add-filtered"] = AddFiltered,
        };
    }

    public override string ScriptName()
    {
        return scriptName;
    }

    protected override Dictionary<string, Action> Commands()
    {
        return commands;
    }

public void Add()
    {
        switch (commandLine.ArgumentCount)
        {
            case 2:
                logger.Debug("Missing Parameter 3!");
                break;
            default:
                break;
        }

        string text = commandLine.Argument(2);
        var blocks = blockProvider.GetBlocks();

        if (commandLine.Switch(switchForAddAsPrefix))
        {
            logger.Debug("Switch '" + switchForAddAsPrefix + "' found!");
            blocks.ForEach(block => block.CustomName = text + block.CustomName);
            return;
        }

        blocks.ForEach(block => block.CustomName += text);
    }

public void AddFiltered()
    {
        switch (commandLine.ArgumentCount)
        {
            case 2:
                logger.Debug("Missing Parameter 3 and 4!");
                break;
            case 3:
                logger.Debug("Missing Parameter 4!");
                break;
            default:
                break;
        }

        string filter = commandLine.Argument(2);
        string text = commandLine.Argument(3);
        var blocks = blockProvider.GetBlocks();

        if (commandLine.Switch(switchForAddAsPrefix))
        {
            logger.Debug("Switch '" + switchForAddAsPrefix + "' found!");
            blocks.ForEach(block => {
                if (block.CustomName.Contains(filter)) block.CustomName = text + block.CustomName;
            });
            return;
        }

        blocks.ForEach(block => {
            if (block.CustomName.Contains(filter)) block.CustomName += text;
        });
    }

public void Replace()
    {
        switch (commandLine.ArgumentCount)
        {
            case 2:
                logger.Debug("Missing Parameter 3 and 4!");
                break;
            case 3:
                logger.Debug("Missing Parameter 4!");
                break;
            default:
                break;
        }

        string original = commandLine.Argument(2);
        string replace = commandLine.Argument(3);

        var blocks = blockProvider.GetBlocks();

        if (!commandLine.Switch(switchForAllConnectedGrids))
        {
            blocks.ForEach(block => {
                if(block.IsSameConstructAs(myBlock)) ReplaceInBlock(block, original, replace);
            });
            return;
        }

        logger.Debug("Switch '" + switchForAllConnectedGrids + "' found!");
        blockProvider.GetBlocks().ForEach(block => {
            ReplaceInBlock(block, original, replace);
        });
    }

public void ReplaceFiltered()
    {
        switch (commandLine.ArgumentCount)
        {
            case 2:
                logger.Debug("Missing Parameter 3 and 4 and 5!");
                break;
            case 3:
                logger.Debug("Missing Parameter 4 and 5!");
                break;
            case 4:
                logger.Debug("Missing Parameter 5!");
                break;
            default:
                break;
        }

        string filter = commandLine.Argument(2);
        string original = commandLine.Argument(3);
        string replace = commandLine.Argument(4);

        var blocks = blockProvider.GetBlocks();

        if (!commandLine.Switch(switchForAllConnectedGrids))
        {
            blocks.ForEach(block => {
                if (block.IsSameConstructAs(myBlock) && block.CustomName.Contains(filter))
                {
                    ReplaceInBlock(block, original, replace);
                }
            });
            return;
        }

        logger.Debug("Switch '" + switchForAllConnectedGrids + "' found!");
        blockProvider.GetBlocks().ForEach(block => {
            if (block.CustomName.Contains(filter)) ReplaceInBlock(block, original, replace);
        });
    }

    private void ReplaceInBlock(IMyTerminalBlock block, string original, string replace)
    {
        block.CustomName = block.CustomName.Replace(original, replace);
    }
}

public abstract class Script
{
    protected MyCommandLine commandLine;
    protected readonly TerminalLogger logger;
    private Action action;

    public Script(TerminalLogger logger)
    {
        this.logger = logger;
    }

    public abstract string ScriptName();

    protected abstract Dictionary<string, Action> Commands();

    public void Run(ref MyCommandLine commandLine) {
        this.commandLine = commandLine;
        Dictionary<string, Action> commands = Commands();

        logger.Debug("Script: " + commandLine.Argument(0) + " !");

        string command = commandLine.Argument(1);

        if(!commands.TryGetValue(command, out action))
        {
            logger.Debug("Command: " + command + " not found !");
            return;
        }

        logger.Debug("Command: " + command + " !");
        action.Invoke();
    }
}

public class ScriptRouter
{
    private readonly Dictionary<string, Script> scripts = new Dictionary<string, Script>();
    private Script script;

    private readonly TerminalLogger logger;

    public ScriptRouter(TerminalLogger logger)
    {
        this.logger = logger;
    }

    public void Route(ref MyCommandLine commandLine)
    {
        string scriptName = commandLine.Argument(0);
        if (!scripts.TryGetValue(scriptName, out script))
        {
            logger.Debug("'" + scriptName + "' Not found!");
            return;

        }

        script.Run(ref commandLine);
    }

    public void RegisterScript(Script script)
    {
        scripts[script.ScriptName()] = script;
    }
}

public class TerminalLogger
{
    private readonly Action<string> echo;
    public TerminalLogger(Action<string> echo)
    {
        this.echo = echo;
    }

    public void Debug(string msg)
    {
        echo.Invoke(msg);
    }
}