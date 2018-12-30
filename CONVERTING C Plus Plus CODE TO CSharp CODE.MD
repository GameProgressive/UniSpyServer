# Converting C++ code to C# code
This is a tutorial that will conver converting the [legacy C++ code](https://www.github.com/GameProgressive/RetroSpyServerCXX) into the new C# base.

Only the submodules are required to be converted (the one present on RetroSpyServerCXX)

The HandleRequest is used on PrecenceServers.

Let's take as an example this function (playersearch/PSServer.cpp)
```
bool PSServer::OnValid(mdk_socket client, const char *buf, int)
{
	std::string buffer="";
	std::string email = "";
	CResultSet *result = NULL;

	if (!get_gs_data(buf, email, "email"))
	{
		return false;
	}

	result = new CResultSet();

	buffer = "SELECT COUNT(userid) FROM `users` WHERE `email` = '";
	if (!mdk_escape_query_string(m_lpDatabase, email))
	{
		delete result;
		return false;
	}
	buffer += email;
	buffer += "'";

	if (!result->ExecuteQuery(m_lpDatabase, buffer))
	{
		delete result;
		return false;
	}

	if (!result->GotoFirstRow())
		WriteTCP(client, "\\vr\\0\\final\\");

	if (result->GetIntFromRow(0) < 1)
		WriteTCP(client, "\\vr\\0\\final\\");
	else
		WriteTCP(client, "\\vr\\1\\final\\");

	delete result;
	return true;
}
```

Which is converted to this function (RetroSpyServer/Server/GPSPServer.cs)
```
        private void IsEmailValid(TCPStream stream, Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("email"))
            {
                SendError(stream, 1, "There was an error parsing an incoming request.");
                stream.Close();
            }

            try
            {
                if (databaseDriver.Query("SELECT userid FROM users WHERE LOWER(email)=@P0", dict["email"].ToLowerInvariant()).Count != 0)
                    stream.SendAsync(@"\vr\1\final\");
                else
                    stream.SendAsync(@"\vr\0\final\");
            }
            catch (Exception ex)
            {
                LogWriter.Log.Write(ex.Message, LogLevel.Error);
                SendError(stream, 4, "This request cannot be processed because of a database error.");
                stream.Close();
            }
        }
```

First, ```const char* buf``` is translated into a Dictionary: ```Dictionary<string, string> dict```, and ```mdk_socket``` is
translated to ```TCPStream```

Second, we have to check the parameters we are using with ```dict.ContainsKey```, and then access that.

In the C++ code, this action was done by using one function: ```get_gs_data(buffer, data_output, key)```.

In C#, this function could be translated to ```dict.ContainsKey(key)``` and ```data_output = dict[key]```

```ResultSet``` class is now ```DatabaseDriver```, we can perform a query with ```Query(query)```, it returns a ```List``` containg our
rows in a ```Dictionary<string, object>``` where the key is the column name.

An instance of ```DatabaseDriver``` is present on each server, this istance is called ```databaseDriver```.

```mdk_escape_query_string``` does not exist anymore, it's now converted as ```"LOWER(column_name)=@PX"``` where X is the number
of the argument passed in the Query function.

```CResultSet::GotoFirstRow``` it's no lenger required, since we can move to any row we want.
```CResultSet::GetIntFromRow``` translates into ```databaseDriver.Query(....)[row]["column name"], column names are now **required**

```WriteTCP``` gets translated into ```stream.SendAsync```

We can now close the stream with the function ```stream.Close()```, the old way was returning false at the function.

A new function ```SendError(TCPStream, ErrorCode, Message)``` could be used in order to send errors.
