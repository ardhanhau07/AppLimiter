namespace PlayLimit.Models;

public class AppRule
{
    public int Id {get; set;}
    public string Name {get;set;} = "";
    public string ExePath {get;set;} = "";
    public string ProcessName {get;set;} = "";

    //time limit in minutes
    public int TimeLimit {get;set;}
    public bool Enabled {get; set;}
}