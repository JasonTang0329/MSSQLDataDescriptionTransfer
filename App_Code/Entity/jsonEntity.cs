using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

/// <summary>
/// jsonEntity 的摘要描述
/// </summary>
public class jsonEntity
{
    [JsonProperty("name")]
    public string name { get; set; }
    [JsonProperty("value")]
    public string value { get; set; }

}