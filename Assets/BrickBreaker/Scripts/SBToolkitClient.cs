using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq;
using System.Text;
using System.IO;


#region SBToolkitClient
public class SBToolkitClient : MonoBehaviour {

	private class SBUser
	{
		public string id;
		public string location_id;
		public bool alive;
	}

	public int port = 8001;
	private Thread _receiveThread;
	private UdpClient _udpClient;
	private bool _shouldRun;
	private string _nextStringToParse;
	private Dictionary<string, SBUser> _alive_users;
    public GameMaster master;

	private bool _rcvStart;

	void Start () {

        Debug.Log("Entering (as server)");
        _rcvStart = false;
        udpStart();
	}

	public void udpStart()
	{
		_shouldRun = true;
		_receiveThread = new Thread(
			new ThreadStart(_receiveData));
		_receiveThread.IsBackground = true;
		_receiveThread.Start();
		_alive_users = new Dictionary<string, SBUser>();

		_rcvStart = true;
	}

	void Update () 
	{
		if (_rcvStart )
		{
			if (_nextStringToParse != null)
			{
				SBToolkitMessage m = new SBToolkitMessage(_nextStringToParse);

				if (m.WellFormed)
				{
					foreach(KeyValuePair<string, SBUser> sbu in _alive_users)
					{
						sbu.Value.alive = false;
					}

                    foreach (SBToolkitUser u in m.FrameUsers)
					{
						if(_alive_users.ContainsKey(m.LocationID + "&&" + u.ID))
						{
							_alive_users[m.LocationID + "&&" + u.ID].alive = true;
						}
						else
						{
							SBUser sbuser = new SBUser();
							sbuser.id = u.ID;
							sbuser.location_id = m.LocationID;
							sbuser.alive = true;
							_alive_users[m.LocationID + "&&" + u.ID] = sbuser;
						}

                        master.SetPosition(u.ID, u.Left, u.Top);
					}

                    List<string> dead_users = new List<string>();
                    foreach (KeyValuePair<string, SBUser> sbu in _alive_users)
                    {
                        if (!sbu.Value.alive && sbu.Value.location_id == m.LocationID)
                            dead_users.Add(sbu.Key);
                    }

                    foreach (string s in dead_users)
                    {
                        SBUser sbu = _alive_users[s];
                        _alive_users.Remove(s);
                        master.RemovePlayer(sbu.id);
                    }
				}
				_nextStringToParse = null;
			}
		}
	}

	private void _receiveData()
	{
		Debug.Log ("listening udp started");
		_udpClient = new UdpClient(port);
		while (_shouldRun)
		{
			try
			{
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
				byte[] data = _udpClient.Receive(ref anyIP);
				string rcvString = Encoding.UTF8.GetString(data);
				
				_nextStringToParse = rcvString;
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
			}
		}
	}
	
	void OnApplicationQuit()
	{
		_shouldRun = false;
		try
		{
			_receiveThread.Abort();
			_udpClient.Close();
			Debug.Log("[UDP] socket closed");
		}
		catch (Exception e)
		{
			Debug.Log("[UDP] " + e.Message);
		}
	}
	
	void OnQuit()
	{
		OnApplicationQuit();
	}
}
#endregion
