using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



public enum SkeletonPositions
{
	HipCenter = 0, Spine, ShoulderCenter, Head, ShoulderLeft, ElbowLeft, WristLeft, HandLeft, ShoulderRight, ElbowRight,
	WristRight, HandRight, HipLeft, KneeLeft, AnkleLeft, FootLeft, HipRight, KneeRight, AnkleRight, FootRight, TotalJoints
}

public class ParsedUserData
{
	public string uniqueId;
	public float left;
	public float top;
	public float kRoomOri;
	public float kRoomLeft;
	public float kRoomTop;
	public Quaternion kinectOrientation;
	
	public List<Vector3> joints;
	
	private float stringToFloat(string f)
	{
		return float.Parse(f.Replace(',', '.'));
	}
	
	private Vector3 _stringPointToVector(string fff)
	{
		string [] vector = fff.Split(':');
		Vector3 retVector = new Vector3();
		
		retVector.x = stringToFloat(vector[0]);
		retVector.y = stringToFloat(vector[1]);
		retVector.z = stringToFloat(vector[2]);
		
		return retVector;
	}
	
	public ParsedUserData(string message)
	{	
		joints = new List<Vector3>();
		
		foreach (string s in message.Split('>'))
		{
			//Debug.Log("Message: " + s);
			string[] statement = s.Split('=');
			
			if (statement[0] == "uid")
			{
				uniqueId = statement[1];
			}
			else if (statement[0] == "left")
			{
				left = stringToFloat(statement[1]);
			}
			else if (statement[0] == "top")
			{
				top = stringToFloat(statement[1]);
			}
			else if (statement[0] == "kRoomOri")
			{
				kRoomOri = stringToFloat(statement[1]);
			}
			else if (statement[0] == "kRoomLeft")
			{
				kRoomLeft = stringToFloat(statement[1]);
			}
			else if (statement[0] == "kRoomTop")
			{
				kRoomTop = stringToFloat(statement[1]);
			}
			else if (statement[0] != "tid" && statement[0] != "conf" && statement[0] != "kOri" && statement[0] != "Position")
			{
				
				
				Vector3 j = _stringPointToVector(statement[1]); 
				joints.Add(j);	
				
				if (statement[0] == "Head")
				{
					Debug.Log(j.x);
				}
			}
		}
	}
}

public class SBTrackerMessageParser
{
	
	private ParsedUserData [] _users;
	public ParsedUserData [] Users
	{ get { return _users; } }
	
	public SBTrackerMessageParser(string line)
	{
		_parse(line);
	}
	
	private void _parse(string line)
	{	
		List<string> tokens = new List<string>(line.Split('/'));
		if (tokens[0] == "sb")
		{
			if (tokens[1] == "none")
			{
				_users = new ParsedUserData[0];
			}
			else
			{
				List<ParsedUserData> users = new List<ParsedUserData>();
				tokens.RemoveAt(0);
				tokens.RemoveAt(tokens.Count - 1);

				
				foreach (string token in tokens)
				{
					ParsedUserData ud = new ParsedUserData(token);
					users.Add(ud);
				
				}
				_users = users.ToArray();
			}
		}
	}
}
