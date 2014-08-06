using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class SBToolkitUser
{
	private string _id;
	public string ID
	{ get { return _id; }}

	private float _left;
	public float Left
	{ get { return _left; } }

	private float _top;
	public float Top
	{ get { return _top; } }

	public SBToolkitUser (string id, float left, float top)
	{
		_id = id; _left = left; _top = top;
	}

	public SBToolkitUser (string raw)
	{
		string [] statements = raw.Split(';');
		foreach (string statement in statements)
		{
			string [] tokens = statement.Split('=');
			if (tokens[0] == "left")
			{
				_left = float.Parse(tokens[1].Replace(",","."));
			}
			else if (tokens[0] == "top")
			{
				_top = float.Parse(tokens[1].Replace(",","."));
			}
			else if (tokens[0] == "UserID")
			{
				_id = tokens[1];
			}
		}
	}
}

public class SBToolkitMessage 
{

	private string _data;
	public string Data
	{ get { return _data; } }

	private  string _locationID;
	public string LocationID
	{ get { return _locationID; }}

	private  float _width;
	public float WallWidth
	{ get { return _width; }}
	
	private  float _depth;
	public float WallDepth
	{ get { return _depth; }}

	private List<SBToolkitUser> _frameUsers;
	public List<SBToolkitUser> FrameUsers
	{ get { return _frameUsers; } }

	// obsolete
	private List<string> _deadUsers;
	public List<string> DeadUsers
	{ get { return _deadUsers; } }

	private bool _wellFormed;
	public bool WellFormed
	{ get { return _wellFormed; } }

	public SBToolkitMessage(string data)
	{
		_data = data;
		_locationID = "";
		_frameUsers = new List<SBToolkitUser> ();
		_deadUsers = new List<string>();
		_wellFormed = _parse ();
	}

	private bool _parse()
	{
		List<string> usersdata = new List<string>(_data.Split ('/'));
		if (usersdata[0] == "sb")
		{
			usersdata.RemoveAt(0);
			foreach (string s in usersdata)
			{
				string [] tokens = s.Split('=');
				if(tokens[0] == "UserID")
					_frameUsers.Add(new SBToolkitUser(s));
				else if(tokens[0] == "DeadUserID")
					_deadUsers.Add(tokens[1]);
				else if(tokens[0] == "wall.id")
					_locationID = tokens[1];
				else if(tokens[0] == "wall.width")
					_width = float.Parse(tokens[1].Replace(",", "."));
				else if(tokens[0] == "wall.depth")
					_depth = float.Parse(tokens[1].Replace(",", "."));
			}

			return true;
		}
		return false;
	}
}