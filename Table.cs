
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Collections.Generic;

struct AbilityOptionTable
{
    public string AbilityName { get; set; }
    public string GradeName { get; set; }
	
    public List<AbilityOption> Options { get; set; }
	
    public void SerializeThis(string path, string filename)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
			
        File.WriteAllText(path + '/' + filename, JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, //이스케이프시퀀스 작동하게
            WriteIndented = true, //줄바꿈 등 보기좋게
            IncludeFields = true //프로퍼티 저장
        }));
    }
}

struct AbilityValueOptionTable
{
    public string AbilityName { get; set; }
    public string GradeName { get; set; }
	
    public List<AbilityValueOption?> Options { get; set; }
	
    public void SerializeThis(string path, string filename)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
			
        File.WriteAllText(path + '/' + filename, JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, //이스케이프시퀀스 작동하게
            WriteIndented = true, //줄바꿈 등 보기좋게
            IncludeFields = true //프로퍼티 저장
        }));
    }
}

