using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;


[XmlRoot("GEDContainer")]
public class GEDContainer
{
	[XmlArray("savedPatterns"),XmlArrayItem("GameEditorDataTeste")]
	public List<GameEditorDataTeste> savedPatterns = new List<GameEditorDataTeste>();
}
